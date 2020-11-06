using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace RichTextEditor.Utils.Obselete
{
    /// <summary>
    /// This is another way to bind string to richtextbox. However, without the help of formatters, such binding lacks variety
    /// This is obselete.
    /// </summary>
    [Obsolete]
    public class RichTextBoxHelper : DependencyObject
    {
        private static HashSet<Thread> _recursionProtection = new HashSet<Thread>();
        private static FlowDocument errorTemplateText;

        static RichTextBoxHelper()
        {
            // initialize the error template when not parsing xaml correctly
            errorTemplateText = new FlowDocument();
            Bold bold = new Bold();
            bold.Inlines.Add(new Run() { Text = "Oops, " });
            Run run = new Run() { Text = "I cannot parse Xaml correctly, please correct in its raw format." };
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(bold);
            paragraph.Inlines.Add(run);
            errorTemplateText.Blocks.Add(paragraph);
        }
        public static string GetDocumentXaml(DependencyObject obj)
        {
            return (string)obj.GetValue(DocumentXamlProperty);
        }

        public static void SetDocumentXaml(DependencyObject obj, string value)
        {
            _recursionProtection.Add(Thread.CurrentThread);
            obj.SetValue(DocumentXamlProperty, value);
            _recursionProtection.Remove(Thread.CurrentThread);
        }

        public static readonly DependencyProperty DocumentXamlProperty = DependencyProperty.RegisterAttached(
            "DocumentXaml",
            typeof(string),
            typeof(RichTextBoxHelper),
            new FrameworkPropertyMetadata(
                "",
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (obj, e) => {
                    if(_recursionProtection.Contains(Thread.CurrentThread))
                        return;

                    var richTextBox = (RichTextBox)obj;

                // Parse the XAML to a document (or use XamlReader.Parse())

                try
                    {
                        var stream = new MemoryStream(Encoding.UTF8.GetBytes(GetDocumentXaml(richTextBox)));
                        var doc = (FlowDocument)XamlReader.Load(stream);

                    // Set the document
                    richTextBox.Document = doc;
                    }
                    catch(Exception)
                    {
                        richTextBox.Document = RichTextBoxHelper.errorTemplateText;
                    }

                // When the document changes update the source
                richTextBox.TextChanged += (obj2, e2) =>
                    {
                        RichTextBox richTextBox2 = obj2 as RichTextBox;
                        if(richTextBox2 != null)
                        {
                            SetDocumentXaml(richTextBox, XamlWriter.Save(richTextBox2.Document));
                        }
                    };
                }
            )
        );
    }
}
