using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace BombDropper
{
	public class BombGame: INotifyPropertyChanged
	{
		private static BombGame _instance = null;
		public static BombGame Instance { get
			{
				return _instance;
			}
			private set => _instance = value; }
		public static void Init(Canvas canvas)
		{
			_instance = new BombGame(canvas);
		}

		private bool gameRunning = false;
		public bool GameRunning { get=>gameRunning; set 
			{
				gameRunning = value;
				NotifyPropertyChanged();
			} }
		public string StatusDisplay 
		{
			get 
			{
				if(GameRunning == false && droppedCount==0) return "No bombs have been dropped. Press start to play.";
				else 
				{
					if(droppedCount >= maxDropped) return "Game Over.";
					return $"You have dropped {droppedCount}/{maxDropped} bombs and saved {savedCount}."; 
				}
			}
			set { NotifyPropertyChanged(); }
		}
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged([CallerMemberName]String propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private int droppedCount = 0;
		private int savedCount = 0;
				
		private DispatcherTimer bombTimer;
		private double initialSecondsBetween = 1.3D;
		private double initialSecondsToFall = 3.5D;
		private double exitSeconds = 2D;
		private int maxDropped = 10;
		private double secondsBetween;
		private double secondsToFall;
		private Canvas gameAreaCache = null;
		private Dictionary<Bomb, Storyboard> bombStoryDict = new Dictionary<Bomb, Storyboard>();


		private BombGame(Canvas gameArea_)
		{
			bombTimer = new DispatcherTimer();
			bombTimer.Tick += bombTimerTick;
			GameRunning = false;
			NotifyPropertyChanged(nameof(GameRunning));
			NotifyPropertyChanged(nameof(StatusDisplay));
			gameAreaCache = gameArea_;
		}

		private void bombTimerTick(object sender, EventArgs args)
		{
			//create bomb
			Bomb bomb = new Bomb();
			bomb.IsFalling = true;
			//position bomb
			Random random = new Random();
			bomb.SetValue(Canvas.LeftProperty, (double)random.Next(10, (int)gameAreaCache.ActualWidth - 10));
			bomb.SetValue(Canvas.ToolTipProperty, -100D);

			// attach click events
			bomb.MouseLeftButtonDown += Bomb_MouseLeftButtonDown;
			//start animation
			Storyboard storyboard = bomb.FindResource("BombFallingStoryBoard") as Storyboard;
			DoubleAnimation fallingAnimation = storyboard.Children[0] as DoubleAnimation;
			Storyboard.SetTarget(fallingAnimation,bomb);
			fallingAnimation.To = gameAreaCache.ActualHeight;
			fallingAnimation.From = -100;
			fallingAnimation.Duration = TimeSpan.FromSeconds(secondsToFall);
			bombStoryDict.Add(bomb, storyboard);

			//add bomb to canvas
			gameAreaCache.Children.Add(bomb);

			storyboard.Duration = fallingAnimation.Duration;
			storyboard.Completed += Storyboard_Completed;
			bomb.BeginStoryboard(storyboard);
		}

		private void Storyboard_Completed(object sender, EventArgs e)
		{
			ClockGroup clockGroup = (ClockGroup)sender;
			DoubleAnimation fallingAnimation = clockGroup.Children[0].Timeline as DoubleAnimation;
			Bomb fallBomb = Storyboard.GetTarget(fallingAnimation) as Bomb;

			if(fallBomb.IsFalling)
			{
				droppedCount++;
			}
			else
			{
				savedCount++;
			}

			// update display
			NotifyPropertyChanged(nameof(StatusDisplay));

			//check game over
			if(droppedCount>= maxDropped)
			{
				GameOver();
			}
			else
			{
				// remove the storyboard in canvas and in current dictionary
				Storyboard storyboard = clockGroup.Timeline as Storyboard;
				storyboard.Stop();
				bombStoryDict.Remove(fallBomb);
				gameAreaCache.Children.Remove(fallBomb);
			}
		}

		private void GameOver()
		{
			// stop dispatchertimer and remove all bombs on the screen
			bombTimer.Stop();
			//NotifyPropertyChanged(nameof(StatusDisplay));
			foreach(var item in bombStoryDict)
			{
				item.Value.Stop();
				gameAreaCache.Children.Remove(item.Key);
			}
			bombStoryDict.Clear();
			GameRunning = false;
			NotifyPropertyChanged(nameof(GameRunning));
			NotifyPropertyChanged(nameof(StatusDisplay));
		}

		private void Bomb_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			// get bomb
			Bomb bombClicked = sender as Bomb;
			bombClicked.IsFalling = false;

			// stop falling animation
			Storyboard storyboard = bombStoryDict[bombClicked];
			storyboard.Stop();
			storyboard.Children.Clear();

			Storyboard exitingStoryBoard = bombClicked.FindResource("BombExitStoryBoard") as Storyboard;
			DoubleAnimation risingAnimation = exitingStoryBoard.Children[0] as DoubleAnimation;
			risingAnimation.Duration = TimeSpan.FromSeconds(exitSeconds);
			double currentTop = Canvas.GetTop(bombClicked);
			risingAnimation.From = currentTop;
			risingAnimation.To = 0;
			Storyboard.SetTarget(risingAnimation, bombClicked);

			DoubleAnimation slidingAnimation = exitingStoryBoard.Children[1] as DoubleAnimation;
			double currentLeft = Canvas.GetLeft(bombClicked);
			if(currentLeft < gameAreaCache.ActualWidth / 2)
			{
				slidingAnimation.To = -100;
			}
			else
			{
				slidingAnimation.To = gameAreaCache.ActualWidth + 100;
			}
			slidingAnimation.From = currentLeft;
			slidingAnimation.Duration = TimeSpan.FromSeconds(exitSeconds);
			Storyboard.SetTarget(slidingAnimation, bombClicked);

			bombClicked.SetValue(Canvas.ZIndexProperty, 0);
			exitingStoryBoard.Duration = TimeSpan.FromSeconds(exitSeconds);
			exitingStoryBoard.Begin();
		}

		public void GameStart()
		{
			GameRunning = true;
			NotifyPropertyChanged(nameof(GameRunning));
			NotifyPropertyChanged(nameof(StatusDisplay));

			//reset the game
			droppedCount = 0;
			savedCount = 0;
			secondsBetween = initialSecondsBetween;
			secondsToFall = initialSecondsToFall;
			bombStoryDict.Clear();

			bombTimer.Interval = TimeSpan.FromSeconds(secondsBetween);
			bombTimer.Start();
		}
	}
}
