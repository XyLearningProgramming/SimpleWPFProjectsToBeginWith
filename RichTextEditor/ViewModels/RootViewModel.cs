using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RichTextEditor.ViewModels
{
	/// <summary>
	/// The view model as root that connects to all other sub viewmodels
	/// </summary>
	public class RootViewModel : BaseViewModel
	{
		private string windowTitle = "";
		#region properties
		public string WindowTitle { get => windowTitle; set { SetProperty(ref windowTitle, value); } }
		public bool IsApplicationLoaded { get; set; } = false;
		public TextEditorPanelViewModel TextEditorPanelViewModel { get; }
		public AllCommands Commands { get; }
		#endregion

		public RootViewModel() : base()
		{
			TextEditorPanelViewModel = new TextEditorPanelViewModel(this);
			Commands = new AllCommands(this);
		}

		public void UpdateFormattedWindowTitle()
		{
			// A TextEditor - *NewPage.xaml Directory:...
			StringBuilder sb = new StringBuilder();
			sb.Append("A TextEditor - ");
			if(Commands.SaveAsFile.CanExecute(new object()))
			{
				sb.Append("*");
			}
			if(string.IsNullOrEmpty(TextEditorPanelViewModel.OpenedFileName) == false)
			{
				sb.Append(Path.GetFileName(TextEditorPanelViewModel.OpenedFileName));
				sb.Append(" (");
				sb.Append(TextEditorPanelViewModel.OpenedFileName);
				sb.Append(")");
			}
			WindowTitle = sb.ToString();
		}

		internal void OnApplicationLoad()
		{
			if(IsApplicationLoaded) return;
			IsApplicationLoaded = true;

			TextEditorPanelViewModel.OnApplicationLoaded();
		}
		internal void OnTextEditorWindowLoaded()
		{
			TextEditorPanelViewModel.OnTextEditorWindowLoaded();
			UpdateFormattedWindowTitle();
		}
	}
}
