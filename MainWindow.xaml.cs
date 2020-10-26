using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BombDropper
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void cv_GameBackground_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			// set new clip region for game canvas
			RectangleGeometry rect = new RectangleGeometry() {Rect=new Rect(0,0,cv_GameBackground.ActualWidth,cv_GameBackground.ActualHeight) };
			cv_GameBackground.Clip = rect;
		}

		private void cmdStart_Click(object sender, RoutedEventArgs args)
		{
			BombGame.Instance.GameStart();
			this.DataContext = BombGame.Instance;
		}

		private void Window_ContentRendered(object sender, EventArgs e)
		{
			BombGame.Init(this.cv_GameBackground);
			//BombGame.Instance.GameStart();
		}
	}
}
