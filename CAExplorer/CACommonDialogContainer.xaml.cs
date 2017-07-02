// programmed by Adrian Magdina in 2013
// in this file is the implementation of viewbehind for Window that is a container for User controls where dialogs are implemented

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CAExplorerNamespace
{
    public delegate void CloseDialogEventDelegate(object sender, RoutedEventArgs e);

    public interface IDialogResultVMHelper
    {
        event EventHandler DialogResultTrueEvent;
    }

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CACommonDialog : Window
    {
        public CACommonDialog()
        {
            InitializeComponent();
        }

        private void windowCACommonDialog_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //adding DataContext and this Window to ViewAndViewModelMappings collection.
            //this is needed because during showing of other Window it is needed to know which is the parent window.
            ViewModelBase aViewModelBase = DataContext as ViewModelBase;
            DialogMediator.ViewAndViewModelMappings.Add(aViewModelBase, this);
        }

    }
}
