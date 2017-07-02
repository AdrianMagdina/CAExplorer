// programmed by Adrian Magdina in 2013
// in this file is the implementation of codebehind for Main Window.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Effects;

namespace CAExplorerNamespace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowCA : Window
    {
        public MainWindowCA()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mwCA_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CAMainWindowVM aCAMainWindowVM = this.DataContext as CAMainWindowVM;

            if (aCAMainWindowVM != null)
            {
                try
                {
                    //during closing configuration file is saved.
                    aCAMainWindowVM.SaveApplicationSettings();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    aCAMainWindowVM.Dispose();
                }
            }
        }

        private void mwCA_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ViewModelBase aViewModelBase = DataContext as ViewModelBase;
            if (aViewModelBase != null)
            {
                //adding this ViewModel = DataContext and this Window to ViewAndViewModelMappings collection.
                //this is needed because during showing of other Window it is needed to know which is the parent window.
                DialogMediator.ViewAndViewModelMappings.Add(aViewModelBase, this);
                DialogMediator.MainWindowView = this;
            }

            CAMainWindowVM aCAMainWindowVM = DataContext as CAMainWindowVM;

            if (aCAMainWindowVM != null)
            {
                //during opening loading of the configuration file.
                aCAMainWindowVM.LoadData();
            }
        }
    }
}
