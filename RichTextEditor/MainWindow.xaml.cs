using RichTextEditor.Utils;
using RichTextEditor.ViewModels;
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

namespace RichTextEditor
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

		private void richTextBox_MouseDown(object sender, MouseEventArgs args)
		{
			//TODO: respond to text select 
			if(args.RightButton == MouseButtonState.Pressed)
			{
				TextPointer location = rtb_Editor.GetPositionFromPoint(Mouse.GetPosition(rtb_Editor), true);
				TextRange wordRange = WordBreaker.GetWordRange(location);
				//TODO: what to do with this text maybe add an option to highlight it in context menu?
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			(this.DataContext as RootViewModel).OnTextEditorWindowLoaded();
		}

		private void tb_XamlText_LostFocus(object sender, RoutedEventArgs e)
		{
			tb_XamlText.GetBindingExpression(TextBox.TextProperty).UpdateSource();
		}
	}
}
