using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CustomControlsClient
{
	[TemplatePart(Name = "PART_RedSlider", Type = typeof(Slider))]
	[TemplatePart(Name = "PART_BlueSlider", Type = typeof(Slider))]
	[TemplatePart(Name = "PART_GreenSlider", Type = typeof(Slider))]
	[TemplatePart(Name = "PART_PreviewColorBrush", Type = typeof(SolidColorBrush))]
	public class ColorPicker: System.Windows.Controls.Control
	{
		#region dependency property
		public static DependencyProperty ColorProperty;
		public Color Color 
		{ 
			get => (Color)GetValue(ColorProperty);
			set => SetValue(ColorProperty, value);
		}
		public static DependencyProperty RedProperty;
		public byte Red
		{
			get => (byte)GetValue(RedProperty);
			set => SetValue(RedProperty, value);
		}
		public static DependencyProperty GreenProperty;
		public byte Green
		{
			get => (byte)GetValue(GreenProperty);
			set => SetValue(GreenProperty, value);
		}
		public static DependencyProperty BlueProperty;
		public byte Blue
		{
			get => (byte)GetValue(BlueProperty);
			set => SetValue(BlueProperty, value);
		}
		#endregion

		static ColorPicker()
		{
			// this class is using a style different than the default one. New style is defined in themes/generic.xaml
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));

			ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(ColorPicker),
				new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorChanged)));
			RedProperty = DependencyProperty.Register("Red", typeof(byte), typeof(ColorPicker),
				new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));
			BlueProperty = DependencyProperty.Register("Blue", typeof(byte), typeof(ColorPicker),
				new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));
			GreenProperty = DependencyProperty.Register("Green", typeof(byte), typeof(ColorPicker),
				new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColorRGBChanged)));

			//binding commands
			CommandManager.RegisterClassCommandBinding(typeof(ColorPicker), new CommandBinding(ApplicationCommands.Undo,
				UndoCommand_Executed, UndoCommand_CanExecute));
		}

		/// <summary>
		/// Binding commands when instantiate this control
		/// </summary>
		public ColorPicker()
		{
			//SetUpCommands();
		}

		private Color? previousColor;

		private static void OnColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			ColorPicker picker = sender as ColorPicker;
			Color oldColor = (Color)args.OldValue;

			if(picker.isDragging==false)
				picker.previousColor = oldColor;

			Color newColor = (Color)args.NewValue;
			picker.Red = newColor.R; picker.Green = newColor.G; picker.Blue = newColor.B;
			picker.OnColorChanged(oldColor, newColor);
		}
		private static void OnColorRGBChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			ColorPicker picker = sender as ColorPicker;

			//picker.previousColor = picker.Color;
			if(args.Property == RedProperty) 
			{
				picker.Red = (byte)args.NewValue; 
			}else if(args.Property == BlueProperty)
			{
				picker.Blue = (byte)args.NewValue;
			}
			else if(args.Property == GreenProperty)
			{
				picker.Green = (byte)args.NewValue;
			}
			picker.Color = Color.FromRgb(picker.Red, picker.Green, picker.Blue);
		}

		/// <summary>
		/// Calling routedEvents when color is changed
		/// </summary>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		private void OnColorChanged(Color oldValue, Color newValue)
		{
			RoutedPropertyChangedEventArgs<Color> args = new RoutedPropertyChangedEventArgs<Color>(oldValue, newValue);
			args.RoutedEvent = ColorPicker.ColorChangedEvent;
			RaiseEvent(args);
		}

		// self-defined routed events
		// registered in static constructor
		public static readonly RoutedEvent ColorChangedEvent = EventManager.RegisterRoutedEvent("ColorChanged", RoutingStrategy.Bubble,
			typeof(RoutedPropertyChangedEventHandler<Color>), typeof(ColorPicker));
		// public event for subscribers
		public event RoutedPropertyChangedEventHandler<Color> ColorChanged
		{
			add => AddHandler(ColorChangedEvent, value);
			remove => RemoveHandler(ColorChangedEvent, value);
		}

		#region custom command binding
		//private void SetUpCommands()
		//{
		//	CommandBinding binding = new CommandBinding(ApplicationCommands.Undo, UndoCommand_Executed, UndoCommand_CanExecute);
		//	this.CommandBindings.Add(binding);
		//}

		//private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs args)
		//{
		//	this.Color = previousColor.Value;
		//}
		//private void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs args)
		//{
		//	args.CanExecute = this.previousColor.HasValue;
		//}

		//another way is to do this in class constructor
		private static void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs args)
		{
			ColorPicker picker = sender as ColorPicker;
			picker.Color = picker.previousColor.Value;
		}
		private static void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = ((ColorPicker)sender).previousColor.HasValue;
		}
		#endregion

		#region template binding
		// solve dragging update problems by adding another bool property to bind
		public bool isDragging { get; set; } = false;
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			foreach(string target in new string[] { "PART_RedSlider", "PART_BlueSlider", "PART_GreenSlider" })
			{
				if(GetTemplateChild(target) is Slider slider)
				{
					Binding binding = new Binding(target.Substring(5,target.Length-5-"Slider".Length)) {Source=this,Mode=BindingMode.TwoWay };
					slider.SetBinding(Slider.ValueProperty, binding);
					// solving the previous color updatre problem
					//slider.DragEnter += (s, e) => { previousColor = this.Color; };
					slider.AddHandler(Thumb.DragStartedEvent, new RoutedEventHandler((s, e) => { isDragging = true; }));
					slider.AddHandler(Thumb.DragCompletedEvent, new RoutedEventHandler((s, e) => { isDragging = false; }));
				}
			}

			if(GetTemplateChild("PART_PreviewColorBrush") is SolidColorBrush brush)
			{
				Binding binding = new Binding("Color") { Source = brush, Mode = BindingMode.OneWayToSource };
				this.SetBinding(ColorProperty, binding);
			}
		}
		#endregion
	}
}
