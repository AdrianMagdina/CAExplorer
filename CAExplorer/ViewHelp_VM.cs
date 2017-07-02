// programmed by Adrian Magdina in 2013
// in this file is the implementation of viewmodel for Help dialog.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace CAExplorerNamespace
{
    public sealed class ViewHelpVM : ViewModelBase
    {
        public ViewHelpVM()
        {
            HelpDocumentFileName = "CAExplorerHelp.cs";
        }

        public string HelpDocumentFileName
        {
            get
            {
                return myHelpDocumentFileName;
            }
            set
            {
                myHelpDocumentFileName = value;
                OnPropertyChanged("HelpDocumentFileName");
            }
        }

        private string myHelpDocumentFileName = null;
    }
}
