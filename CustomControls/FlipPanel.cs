using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace CustomControlsClient
{
	[TemplatePart(Name = "FlipButton", Type = typeof(ToggleButton)),
	TemplatePart(Name = "FlipButtonAlternate", Type = typeof(ToggleButton)),
	TemplateVisualState(Name = "Normal", GroupName = "ViewStates"),
	TemplateVisualState(Name = "Flipped", GroupName = "ViewStates")]
	public class FlipPanel: System.Windows.Controls.Control
	{
		#region dependency properties
		public static readonly DependencyProperty FrontContentProperty = DependencyProperty.Register("FrontContent", typeof(object),
			typeof(FlipPanel), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnContentChanged)));
		public object FrontContent
		{
			get => GetValue(FrontContentProperty);
			set => SetValue(FrontContentProperty, value);
		}
		public static readonly DependencyProperty BackContentProperty = DependencyProperty.Register("BackContent", typeof(object),
			typeof(FlipPanel), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnContentChanged)));
		public object BackContent
		{
			get => GetValue(BackContentProperty);
			set => SetValue(BackContentProperty, value);
		}
		public static readonly DependencyProperty IsFlippedProperty = DependencyProperty.Register("IsFlipped", typeof(bool),
			typeof(FlipPanel), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnPanelFlip)));
		public bool IsFlipped
		{
			get => (bool) GetValue(IsFlippedProperty);
			set => SetValue(IsFlippedProperty, value);
		}
		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius),
			typeof(FlipPanel), null);
		public CornerRadius CornerRadius
		{
			get => (CornerRadius)GetValue(CornerRadiusProperty);
			set => SetValue(CornerRadiusProperty, value);
		}
		#endregion

		#region event
		private static void OnPanelFlip(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((FlipPanel)sender).ChangeVisualState(true);
			//raise event
			RoutedPropertyChangedEventArgs<bool> e = new RoutedPropertyChangedEventArgs<bool>((bool)args.OldValue, (bool)args.NewValue);
			e.RoutedEvent = OnFlipEvent;
			((FlipPanel)sender).RaiseEvent(e);
		}
		public static readonly RoutedEvent OnFlipEvent = EventManager.RegisterRoutedEvent("OnFlip",RoutingStrategy.Bubble,
			typeof(RoutedPropertyChangedEventHandler<bool>), typeof(FlipPanel));
		public event RoutedPropertyChangedEventHandler<bool> OnFlip { add => AddHandler(OnFlipEvent, value); remove => RemoveHandler(OnFlipEvent, value); }

		private static void OnContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			// raise event
			RoutedPropertyChangedEventArgs<object> e = new RoutedPropertyChangedEventArgs<object>(args.OldValue,args.NewValue);
			e.RoutedEvent = ContentChangedEvent;
			((FlipPanel)sender).RaiseEvent(e);
			
		}
		public static readonly RoutedEvent ContentChangedEvent = EventManager.RegisterRoutedEvent("ContentChanged", RoutingStrategy.Bubble,
			typeof(RoutedPropertyChangedEventHandler<object>), typeof(FlipPanel));
		public event RoutedPropertyChangedEventHandler<object> ContentChanged { add => AddHandler(ContentChangedEvent, value); remove => RemoveHandler(ContentChangedEvent, value); }
		#endregion

		static FlipPanel()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(FlipPanel), new FrameworkPropertyMetadata(typeof(FlipPanel)));
		}

		#region control logic
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if(GetTemplateChild("FlipButton") is ToggleButton flipButton)
			{
				flipButton.Click += flipButon_Click;
			}
			if(GetTemplateChild("FlipButtonAlternate") is ToggleButton alternateButton)
			{
				alternateButton.Click += flipButon_Click;
			}
			this.ChangeVisualState(false);
		}

		private void flipButon_Click(object sender, RoutedEventArgs args)
		{
			this.IsFlipped = !this.IsFlipped;
		}

		private void ChangeVisualState(bool useTransitions)
		{
			if(!this.IsFlipped)
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Flipped", useTransitions);
			}

			if(FrontContent is UIElement front)
			{
				if(IsFlipped)
				{
					front.Visibility = Visibility.Hidden;
				}
				else
				{
					front.Visibility = Visibility.Visible;
				}
			} 
			if(BackContent is UIElement back)
			{
				if(IsFlipped)
				{
					back.Visibility = Visibility.Visible;
				}
				else
				{
					back.Visibility = Visibility.Hidden;
				}
			}
		}

		#endregion
	}
}
