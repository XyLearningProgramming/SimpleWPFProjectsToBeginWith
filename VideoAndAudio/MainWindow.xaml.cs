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

namespace VideoAndAudio
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		// resources' pack URI
		public string MpgURIString { get; private set; } = "pack://application,,,component/resources/test.mpg";
		public string Mp3URIString { get; private set; } = "pack://application,,,component/resources/test.mp3";
		public string WavURIString { get; private set; } = "pack://application,,,component/resources/test.wav";

		public MainWindow()
		{
			InitializeComponent();
		}
	}
}
