using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;

namespace RichTextEditor.Utils.Converters
{
	public class FileNameToTextFormatterConverter : IValueConverter
	{
		private readonly WrappedFormatter rtfFormatter = new WrappedFormatter(new Xceed.Wpf.Toolkit.RtfFormatter());
		private readonly WrappedFormatter plainTextFormatter = new WrappedFormatter(new Xceed.Wpf.Toolkit.PlainTextFormatter());
		private readonly WrappedFormatter xamlFormatter = new WrappedFormatter(new Xceed.Wpf.Toolkit.XamlFormatter());
		private readonly WrappedFormatter flowDocumentFormatter = new WrappedFormatter(new FlowDocumentFormatter());
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value is String str)
			{
				string ext = Path.GetExtension(str).ToLower();
				switch(ext)
				{
					case ".rtf":
						return rtfFormatter;
					case ".xaml":
					case ".xml":
						return xamlFormatter;
					case ".flowdoc":
						return flowDocumentFormatter;
					case ".txt":
					default:
						return plainTextFormatter;
				}
			}
			throw new ArgumentException(nameof(value));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}

	// convert pure flowdocument
	public class FlowDocumentFormatter : Xceed.Wpf.Toolkit.ITextFormatter
	{
		public string GetText(FlowDocument document)
		{
			return XamlWriter.Save(document);
		}

		public void SetText(FlowDocument document, string text)
		{
			try
			{
				// if text is empty, then it is trying to renew string
				if(string.IsNullOrEmpty(text))
				{
					document.Blocks.Clear();
					return;
				}
				// each flowdocument owns block. The only way is to copy them though it's very inefficient
				FlowDocument parsed = XamlReader.Parse(text) as FlowDocument;
				CopyTwoFlowDocument.Copy(parsed, document);
			}catch(Exception e) // throw every parsed error out
			{
				throw e;
			}
		}
	}

	public class WrappedFormatter : Xceed.Wpf.Toolkit.ITextFormatter
	{
		private Xceed.Wpf.Toolkit.ITextFormatter textFormatter;
		public WrappedFormatter(Xceed.Wpf.Toolkit.ITextFormatter textFormatter_)
		{
			textFormatter = textFormatter_;
		}
		public string GetText(FlowDocument document)
		{
			return textFormatter.GetText(document);
		}

		public void SetText(FlowDocument document, string text)
		{
			try
			{
				textFormatter.SetText(document, text);
				//for whatever reason settext is not successful and leads to a blank flowdocument
				if(document.Blocks.Count==0 && string.IsNullOrEmpty(text)==false)
					throw new InvalidDataException();
			}
			catch(Exception e)
			{
				ErrorHandler.DisplayErrorInEditor(e, document);
			}
		}
	}

	public class ErrorHandler
	{
		private static FlowDocument errorTemplate;
		static ErrorHandler()
		{
			errorTemplate = (FlowDocument) Application.Current.TryFindResource("ErrorFillTextInEditor");
		}
		public static void DisplayErrorInEditor(Exception e, FlowDocument target)
		{
			errorTemplate.Blocks.Add(new Paragraph(new Run() { Text = e.Message }));
			CopyTwoFlowDocument.Copy(errorTemplate,target);
			errorTemplate.Blocks.Remove(errorTemplate.Blocks.LastBlock);
		}
	}

	public class CopyTwoFlowDocument 
	{ 
		public static void Copy(FlowDocument from, FlowDocument to)
		{
			to.Blocks.Clear();
			AddDocument(from, to);
		}
		public static void AddDocument(FlowDocument from, FlowDocument to)
		{
			TextRange range = new TextRange(from.ContentStart, from.ContentEnd);
			MemoryStream stream = new MemoryStream();
			System.Windows.Markup.XamlWriter.Save(range, stream);
			range.Save(stream, DataFormats.XamlPackage);
			TextRange range2 = new TextRange(to.ContentEnd, to.ContentEnd);
			range2.Load(stream, DataFormats.XamlPackage);
		}
	}

}
