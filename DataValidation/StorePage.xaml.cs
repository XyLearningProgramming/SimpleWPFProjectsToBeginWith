﻿using StoreDatabase;
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

namespace DataValidation
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class StorePage : Window
	{

		public StorePage()
		{
			InitializeComponent();
		}

		private void validationError(object sender, ValidationErrorEventArgs e)
		{
			if(e.Action==ValidationErrorEventAction.Added)
			{
				MessageBox.Show(e.Error.ErrorContent.ToString());
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			FocusManager.SetFocusedElement(this, (Button)sender);
		}
	}
}
