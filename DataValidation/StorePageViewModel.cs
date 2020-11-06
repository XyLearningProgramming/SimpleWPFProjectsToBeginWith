using StoreDatabase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace DataValidation
{
	public class StorePageViewModel: INotifyPropertyChanged, INotifyDataErrorInfo
	{
		protected void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
		}
		public event PropertyChangedEventHandler PropertyChanged;

		#region Properties

		#region database related
		private bool? isDirty = null;
		public bool? IsDirty { get => isDirty; set { isDirty = value; NotifyPropertyChanged(); } }
		private static StoreDb storeDb = new StoreDb();
		public static StoreDb StoreDb
		{
			get { return storeDb; }
		}

		private static StoreDbDataSet storeDbDataSet = new StoreDbDataSet();
		public static StoreDbDataSet StoreDbDataSet
		{
			get { return storeDbDataSet; }
		}
		private ObservableCollection<Product> products = new ObservableCollection<Product>();
		public ObservableCollection<Product> Products { get => products; set {
				products = value;
				NotifyPropertyChanged();
			} }
		public ICollectionView ProductCollectionView
		{
			get
			{
				return CollectionViewSource.GetDefaultView(Products);
			}
		}
		#endregion

		#region edit Menu
		private Product productChosen = new Product("","",0,"");
		public Product ProductChosen
		{
			get => productChosen;
			set => productChosen = value;
		}
		public string ModelNumber { get => ProductChosen.ModelNumber; set
			{
				// rule: only digit and alpha characters
				// implemented via INotifyDataErrorInfo
				bool valid = true;
				foreach(char c in value)
				{
					if(Char.IsLetterOrDigit(c) == false)
					{
						valid = false;
						break;
					}
				}
				if(!valid)
				{
					this.SetErrors(new List<string>() { "The modelname can only contain digit and alpha numbers" });
				}
				else
				{
					this.ClearErrors();
				}
				//!!! data will still be changed into invalid value
				ProductChosen.ModelNumber = value;
				NotifyPropertyChanged();
			}
		}
		public string ModelName
		{
			get => ProductChosen.ModelName;
			set
			{
				ProductChosen.ModelName = value;
				NotifyPropertyChanged();
			}
		}
		public Decimal UnitCost 
		{
			get => ProductChosen.UnitCost;
			set
			{
				ProductChosen.UnitCost = value;
				NotifyPropertyChanged();
			}
		}
		public string Description
		{
			get => ProductChosen.Description;
			set
			{
				ProductChosen.Description = value;
				NotifyPropertyChanged();
			}
		}
		#endregion

		private ICommand refreshCommand;
		public ICommand RefreshCommand
		{
			get
			{
				if(refreshCommand == null)
				{
					refreshCommand = new RelayCommand(param => this.RefreshDatabase(), 
						param=>IsDirty??true);
				}
				return refreshCommand;
			}
		}
		private ICommand selectProductCommand;
		public ICommand SelectProductCommand
		{
			get
			{
				if(selectProductCommand == null)
				{
					// selection changed logic
					selectProductCommand = new RelayCommand((object param) =>
					{
						if(param is Product selectedItem)
						{
							ProductChosen = selectedItem;
							NotifyPropertyChanged("");
						}
					});
				}
				return selectProductCommand;
			}
		}
		private ICommand updateProductCommand;
		public ICommand UpdateProductCommand 
		{
			get
			{
				if(updateProductCommand == null)
				{
					updateProductCommand = new RelayCommand(param =>
					{
						// update logic goes here
						IsDirty = true;
					},
					param =>
					{
						// can update logic goes here
						if(errors.Count > 0) return false;
						else return true;
					});
				}
				return updateProductCommand;
			}
		}

		#endregion

		#region INotifyDataErrorInfo implementation
		private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
		private void SetErrors(List<string> propertyErrors,[CallerMemberName]string propertyName="")
		{
			if(propertyName == "") return;
			// remove errors existing in the name of this property
			errors.Remove(propertyName);
			// add new errors to this property 
			errors.Add(propertyName, propertyErrors);
			//invoke error notification event for view
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}
		private void ClearErrors([CallerMemberName]string propertyName = "")
		{
			if(propertyName == "") return;
			errors.Remove(propertyName);
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}
		public IEnumerable GetErrors(string propertyName)
		{
			if(string.IsNullOrEmpty(propertyName))
			{
				return errors.Values;
			}
			else
			{
				if(errors.ContainsKey(propertyName)) return errors[propertyName];
				else return null;
			}
		}
		public bool HasErrors
		{
			get => errors.Count > 0;
		}
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
		#endregion

		private void RefreshDatabase()
		{
			Products = new ObservableCollection<Product>(storeDb.GetProducts());
			IsDirty = false;
			//MessageBox.Show(Products.Count.ToString());
			ProductCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("CategoryName"));
		}
	}
}
