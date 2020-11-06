using RichTextEditor.Utils.Converters;
using RichTextEditor.ViewModels.Fondations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;

namespace RichTextEditor.ViewModels
{
	public sealed class TextEditorPanelViewModel: AttachedViewModels
	{
		private string _notes = "";
		private bool isRawXamlShowing = false;
		private string _backupNotes = "";
		private StringXamlToPrettyConverter stringXamlToPrettyConverter;
		private string _openedFileName = "./Default.flowDoc";
		public string Notes
		{
			get => _notes;
			set
			{
				SetProperty(ref _notes, value);
				(Root.Commands.SaveAsFile as AsyncCommand).RaiseCanExecuteChanged();
			}
		}
		public string BackUpNotes
		{
			get => _backupNotes;
			set 
			{
				SetProperty(ref _backupNotes, value);
				(Root.Commands.SaveAsFile as AsyncCommand).RaiseCanExecuteChanged();
			}
		}
		public bool IsRawXamlShowing { get => isRawXamlShowing; set => SetProperty(ref isRawXamlShowing, value); }

		public FlowDocument NotesFlowDocument {
			set => Notes = XamlWriter.Save(value); 
		}

		public string OpenedFileName { get => _openedFileName; set => SetProperty(ref _openedFileName, value); }

		public TextEditorPanelViewModel(RootViewModel root) : base(root) 
		{
			stringXamlToPrettyConverter = Application.Current.TryFindResource("StringXamlToPrettyConverter") as StringXamlToPrettyConverter;
		}

		internal override void OnApplicationLoaded()
		{
			base.OnApplicationLoaded();
		}
		internal void OnTextEditorWindowLoaded()
		{
			// set notes to default template
			FlowDocument defaultFlowDocument = Application.Current.TryFindResource("DefaultFillTextInEditor") as FlowDocument;
			NotesFlowDocument = defaultFlowDocument;
			Root.UpdateFormattedWindowTitle();
			//TextRange range = new TextRange(defaultFlowDocument.ContentStart,defaultFlowDocument.ContentEnd);
			//using(MemoryStream ms = new MemoryStream())
			//{
			//	range.Save(ms, DataFormats.Xaml);
			//	Notes = new StreamReader(ms).ReadToEnd();
			//}
		}
	}
}
