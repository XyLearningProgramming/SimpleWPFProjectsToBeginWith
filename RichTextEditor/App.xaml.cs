using RichTextEditor.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RichTextEditor
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static RootViewModel RootViewModel => Current.Resources[nameof(RootViewModel)] as RootViewModel;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			Current.MainWindow = new MainWindow();
			Current.MainWindow.Loaded += (o, e) => RootViewModel.OnApplicationLoad();
			Current.MainWindow.Show();
			
		}
	}
}
