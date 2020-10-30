using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Windows.Input;

namespace VeryBasicMVVM
{
	public class ViewModel: ViewModelBase
	{
		private Student_Model _student;
		private ObservableCollection<Student_Model> _students;
		private ICommand _submitCommand;
		public Student_Model Student 
		{
			get => _student;
			set
			{
				_student = value;
				NotifyPropertyChanged();
			}
		}
		public ObservableCollection<Student_Model> Students
		{
			get => _students;
			set
			{
				_students = value;
				//NotifyPropertyChanged();
			}
		}
		public ICommand SubmitCommand
		{
			get
			{
				if(_submitCommand == null)
				{
					_submitCommand = new RelayCommand(param=>this.Submit(),
						param=>String.IsNullOrEmpty(Student.Name)==false && Student.Age>=0 && Student.Age<=200);
				}
				return _submitCommand;
			}
		}

		public ViewModel()
		{
			Student = new Student_Model();
			Students = new ObservableCollection<Student_Model>();
			//Students.CollectionChanged += Students_CollectionChanged;
		}
		//void Students_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		//{
		//	NotifyPropertyChanged(nameof(Students));
		//}

		private void Submit()
		{
			Student.JoiningDate = DateTime.Today;
			Students.Add(Student);
			Student = new Student_Model();
		}
	}
}
