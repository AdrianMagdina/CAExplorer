// programmed by Adrian Magdina in 2013
// in this file is the implementation of viewbehind for "Application settings" dialog

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
    /// Interaction logic for AppSettings.xaml
    /// </summary>
    public partial class AppSettings : UserControl
    {
        public AppSettings()
        {
            InitializeComponent();
        }

        private bool IsValid(DependencyObject node)
        {
            // Check if dependency object was passed
            if (node != null)
            {
                // Check if dependency object is valid.
                // NOTE: Validation.GetHasError works for controls that have validation rules attached 
                bool isValid = !Validation.GetHasError(node);
                if (!isValid)
                {
                    // If the dependency object is invalid, and it can receive the focus,
                    // set the focus
                    if (node is IInputElement) Keyboard.Focus((IInputElement)node);
                    return false;
                }
            }

            // If this dependency object is valid, check all child dependency objects
            foreach (object subnode in LogicalTreeHelper.GetChildren(node))
            {
                if (subnode is DependencyObject)
                {
                    // If a child dependency object is invalid, return false immediately,
                    // otherwise keep checking
                    if (IsValid((DependencyObject)subnode) == false) return false;
                }
            }

            // All dependency objects are valid
            return true;
        }

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
                        if (this.IsValid(this as DependencyObject) == true)
                        {
                            aWindow.DialogResult = true;
                            aWindow.Close();
                        }
                    }
                }
            }
        }
    }
}
