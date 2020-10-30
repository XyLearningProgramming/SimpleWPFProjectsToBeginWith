using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace VeryBasicMVVM
{
	/// <summary>
	/// ViewModelBase implements INotifyPropertyChanged so that it can be reused by other derived view models.
	/// INotifyPropertyChanged is for view to notice whenever a property in model is changed
	/// </summary>
	public class ViewModelBase : INotifyPropertyChanged
	{
		protected void NotifyPropertyChanged([CallerMemberName] string PropertyName= "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
		}
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
