using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace ControlTemplateBrowser
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

		private void lst_Types_SelectionChanged(object sender, SelectionChangedEventArgs args)
		{
			try
			{
				Type type = (Type)lst_Types.SelectedItem;
				//instantiate type
				ConstructorInfo info = type.GetConstructor(System.Type.EmptyTypes);
				Control control = info.Invoke(null) as Control;

				Window window = control as Window;
				if(window != null)
				{
					// create the window but keep minimized
					window.WindowState = WindowState.Minimized;
					window.ShowInTaskbar = false;
					window.Show();
				}
				else
				{
					// add to the grid but keep hidden
					control.Visibility = Visibility.Collapsed;
					grid_TemplateDisplay.Children.Add(control);
				}

				// get the template
				ControlTemplate template = control.Template;
				// get xaml for template
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = true;
				StringBuilder sb = new StringBuilder();
				XmlWriter writer = XmlWriter.Create(sb, settings);
				XamlWriter.Save(template,writer);

				// display template
				tb_TemplateDisplay.Text = sb.ToString();

				// remove window
				if(window != null)
				{
					window.Close();
				}
				else
				{
					grid_TemplateDisplay.Children.Remove(control);
				}
			}
			catch (Exception err)
			{
				tb_TemplateDisplay.Text = $"Error generating template: {err.Message}";
			}
		}

		/// <summary>
		/// Loaded method is called when the element "is laid out, rendered, and ready for interaction."
		/// https://stackoverflow.com/questions/18452756/whats-the-difference-between-the-window-loaded-and-window-contentrendered-event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Type controlType = typeof(Control);
			List<Type> derivedTypes = new List<Type>();

			Assembly assembly = Assembly.GetAssembly(controlType);
			foreach(Type type in assembly.GetTypes())
			{
				if(type.IsSubclassOf(controlType) && !type.IsAbstract && type.IsPublic)
				{
					derivedTypes.Add(type);
				}
			}

			//sort by name
			derivedTypes.Sort(new TypeComparer());
			lst_Types.ItemsSource = derivedTypes;
		}
	}

	internal class TypeComparer: IComparer<Type>
	{
		public int Compare(Type x, Type y)
		{
			return x.Name.CompareTo(y.Name);
		}
	}
}
