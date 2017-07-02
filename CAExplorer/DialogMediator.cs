// programmed by Adrian Magdina in 2013
// in this file is the implementation of mediator for showing dialogs, messageboxes.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;

namespace CAExplorerNamespace
{
    public static class DialogMediator
    {
        #region Constructors
        #endregion

        #region Methods

        //showing messagebox.
        public static MessageBoxResult ShowMessageBox(object ownerViewModelIn, string captionIn, string messageIn, MessageBoxButton mbbIn, MessageBoxImage mbiIn)
        {
            Window aOwner = null;

            // if ownerViewModelIn is null - default owner will be used - MainWindowView
            if (ownerViewModelIn != null)
            {
                myViewAndViewModelMappings.TryGetValue(ownerViewModelIn as ViewModelBase, out aOwner);
            }
            else
            {
                aOwner = myMainWindowView;
            }

            MessageBoxResult aResult = MessageBox.Show(aOwner, messageIn, captionIn, mbbIn, mbiIn);

            return aResult;
        }

        //showing open file dialog.
        public static bool? ShowOpenFileDialog(object ownerViewModelIn, string fileExtensionFilterIn, string defaultFileExtensionIn, out string fileNameRW)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            Window aOwner = null;
            myViewAndViewModelMappings.TryGetValue(ownerViewModelIn as ViewModelBase, out aOwner);

            dlg.DefaultExt = defaultFileExtensionIn;
            dlg.Filter = fileExtensionFilterIn;

            bool? aDialogResult = dlg.ShowDialog(aOwner);

            fileNameRW = dlg.FileName;

            return aDialogResult;
        }

        //showing save file dialog.
        public static bool? ShowSaveFileDialog(object ownerViewModelIn, string fileExtensionFilterIn, string defaultFileExtensionIn, out string fileNameRW)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            Window aOwner = null;
            myViewAndViewModelMappings.TryGetValue(ownerViewModelIn as ViewModelBase, out aOwner);

            dlg.DefaultExt = defaultFileExtensionIn;
            dlg.Filter = fileExtensionFilterIn;

            bool? aDialogResult = dlg.ShowDialog(aOwner);

            fileNameRW = dlg.FileName;

            return aDialogResult;
        }

        //showing modal dialog window.
        public static bool? ShowModalDialog(string captionIn, object viewModelIn, object ownerViewModelIn)
        {
            CACommonDialog dialog = new CACommonDialog();

            Window aOwner = null;
            myViewAndViewModelMappings.TryGetValue(ownerViewModelIn as ViewModelBase, out aOwner);

            dialog.Title = captionIn;
            dialog.Owner = aOwner;
            dialog.DataContext = viewModelIn;

            // Show dialog
            return dialog.ShowDialog();
        }

        //showing modeless dialog window.
        public static void ShowModelessDialog(string captionIn, object viewModelIn, object ownerViewModelIn)
        {
            CACommonDialog dialog = new CACommonDialog();

            Window aOwner = null;
            myViewAndViewModelMappings.TryGetValue(ownerViewModelIn as ViewModelBase, out aOwner);

            dialog.Title = captionIn;
            dialog.Owner = aOwner;
            dialog.DataContext = viewModelIn;

            // Show dialog
            dialog.Show();

            return;
        }

        //dictionary that maps view to viewmodel
        public static IDictionary<ViewModelBase, Window> ViewAndViewModelMappings
        {
            get
            {
                return myViewAndViewModelMappings;
            }
        }

        public static Window MainWindowView
        {
            get
            {
                return myMainWindowView;
            }
            set
            {
                myMainWindowView = value;
            }
        }

        #endregion

        #region Members

        private static IDictionary<ViewModelBase, Window> myViewAndViewModelMappings = new Dictionary<ViewModelBase, Window>();
        private static Window myMainWindowView = null;

        #endregion
    }
}
