using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItemsControl
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : RibbonWindow
	{
		public MainWindow()
		{
			InitializeComponent();
			this.DataContext = this;

		}
		private static Product defaultProduct = new Product() { Time = "XX", Date = "XXX" };
		public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>() { defaultProduct };
		
	}

	public class Product {
		public string Time;
		public string Date;
	}
}
