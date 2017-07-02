// programmed by Adrian Magdina in 2013
// in this file is the implementation of codebehind for Help dialog.

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CAExplorerNamespace
{
    /// <summary>
    /// Interaction logic for ViewHelp.xaml
    /// </summary>
    public partial class ViewHelp : UserControl
    {
        public ViewHelp()
        {
            InitializeComponent();
        }

        //closing dialog after pressing ok.
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            ContentPresenter aCP = this.TemplatedParent as ContentPresenter;
            if (aCP != null)
            {
                Grid aGrid = aCP.Parent as Grid;

                if (aGrid != null)
                {
                    Window aWindow = aGrid.Parent as Window;
                    if (aWindow != null)
                    {
                        aWindow.Close();
                    }
                }
            }
        }
    }
}
