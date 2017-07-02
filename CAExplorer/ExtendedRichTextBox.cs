// programmed by Adrian Magdina in 2013
// in this file is the implementation of extended RichTextBox control.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.IO;

namespace CAExplorerNamespace
{
    //implementation of ExtendedRichTextBox - RichTextBox control enhanced with loading text from file.
    public class ExtendedRichTextBox : RichTextBox
    {
        public ExtendedRichTextBox()
        {
        }

        //if file name changes, then load the text from file to control.
        public static void OnDocumentFileNameChanged(DependencyObject doIn, DependencyPropertyChangedEventArgs args)
        {
            ExtendedRichTextBox aERTB = doIn as ExtendedRichTextBox;

            if (aERTB != null)
            {
                TextRange aTextRange = null;

                if (File.Exists("CAExplorerHelp.rtf"))
                {
                    aTextRange = new TextRange(aERTB.Document.ContentStart, aERTB.Document.ContentEnd);

                    using (FileStream aFileStream = new FileStream("./CAExplorerHelp.rtf", FileMode.Open))
                    {
                        aTextRange.Load(aFileStream, DataFormats.Rtf);
                    }
                }
            }
        }

        public string DocumentFileName
        {
            get
            {
                return (string)GetValue(DocumentFileNameDependencyProperty);
            }
            set
            {
                SetValue(DocumentFileNameDependencyProperty, value);
            }
        }

        public static readonly DependencyProperty DocumentFileNameDependencyProperty = DependencyProperty.Register("DocumentFileName", typeof(string),
                                                                                                        typeof(ExtendedRichTextBox), new FrameworkPropertyMetadata(OnDocumentFileNameChanged));
    }
}
