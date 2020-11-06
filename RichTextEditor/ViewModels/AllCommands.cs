using Microsoft.Win32;
using RichTextEditor.ViewModels.Fondations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;

namespace RichTextEditor.ViewModels
{
	public class AllCommands
	{
		private IAsyncCommand openFile;
		private IAsyncCommand saveAsFile;
		private DelegateCommand newFile;

		public AllCommands(RootViewModel viewModel)
		{
			RootViewModel = viewModel;
		}

		public RootViewModel RootViewModel { get; }
		public IAsyncCommand OpenFile => openFile ??
			(openFile = new AsyncCommand(async param =>
			{
				await OpenFileAsync(param);
			}));
		public IAsyncCommand SaveAsFile => saveAsFile ??
			(saveAsFile = new AsyncCommand(async o =>
				{
					await SaveAsFileAsync(o);
				},
				//(o) => { return true; })
				(o)=>{ return RootViewModel.TextEditorPanelViewModel.BackUpNotes != RootViewModel.TextEditorPanelViewModel.Notes; })
		);
		public DelegateCommand NewFile => newFile ??
			(newFile = new DelegateCommand(o =>
			{
				// if unsaved then pop a saving window?
				if(this.SaveAsFile.CanExecute(new object()) == true)
				{
					MessageBoxResult result = MessageBox.Show("There is unsaved content. Do you want to save it?",
						"Unsaved Content Exists",
						MessageBoxButton.YesNo);
					if(result == MessageBoxResult.Yes)
					{
						this.SaveAsFile.Execute(new object());
						return;
					}
				}
				RootViewModel.TextEditorPanelViewModel.OpenedFileName = "New.txt";
				RootViewModel.TextEditorPanelViewModel.BackUpNotes = "";
				RootViewModel.TextEditorPanelViewModel.Notes = "";
				RootViewModel.UpdateFormattedWindowTitle();
			}));

		private async Task OpenFileAsync(object parameter)
		{
			//TODO: if opening with dirty content (not saving yet)
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Filter = "XAML Files (*.xaml)|*.xaml|RichText Files (*.rtf)|*.rtf|Text Files (*txt)|*.txt|All Files (*.*)|*.*";
			string openedFilaName = "";
			if(openFile.ShowDialog() == true)
			{
				string parsedXamlString = "";
				openedFilaName = openFile.FileName;
				try
				{
					parsedXamlString = await File.ReadAllTextAsync(openedFilaName,Encoding.UTF8);
				}
				catch (Exception ex)
				{
					// error opening files then pop out messagebox
					MessageBox.Show(Application.Current.MainWindow,
						$"Error happened while opening and parsing files\r\n{ex.Message}",
						$"{nameof(RootViewModel.TextEditorPanelViewModel)} Error",
						MessageBoxButton.OK,
						MessageBoxImage.Error,
						MessageBoxResult.OK);
					return;
				}
				//attribute parsedstring to viewmodel and record name
				await Application.Current.Dispatcher.BeginInvoke(()=> {
					RootViewModel.TextEditorPanelViewModel.OpenedFileName = openedFilaName;
					RootViewModel.TextEditorPanelViewModel.BackUpNotes = parsedXamlString;
					RootViewModel.TextEditorPanelViewModel.Notes = parsedXamlString;
					RootViewModel.UpdateFormattedWindowTitle();
				});
			}
		}
		private async Task SaveAsFileAsync(object parameter)
		{
			SaveFileDialog saveFile = new SaveFileDialog();
			saveFile.Filter = "XAML Files (*.xaml)|*.xaml|RichText Files (*.rtf)|*.rtf|All Files (*.*)|*.*";
			await Application.Current.Dispatcher.BeginInvoke(() => {
				saveFile.FileName = Path.GetFileName(RootViewModel.TextEditorPanelViewModel.OpenedFileName);
			});
			if(saveFile.ShowDialog() == true)
			{
				string toBeSaved = "";
				await Application.Current.Dispatcher.BeginInvoke(() => { toBeSaved = RootViewModel.TextEditorPanelViewModel.Notes; });
				await File.WriteAllTextAsync(saveFile.FileName, toBeSaved, Encoding.UTF8);
				RootViewModel.UpdateFormattedWindowTitle();
			}
		}
	}
}
