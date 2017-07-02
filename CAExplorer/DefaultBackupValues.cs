// programmed by Adrian Magdina in 2013
// in this file is the implementation of default backup values (used if e.g. the file was not found).

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace CAExplorerNamespace
{
    //default backup values, these values will be used as default values if loading of default values from file failed.
    public static class DefaultBackupValues
    {
        #region Properties

        public static Color DefaultBackupGridColor
        {
            get
            {
                return Colors.Black;
            }
        }

        public static Color DefaultBackupSelectionFrameColor
        {
            get
            {
                return Colors.Orange;
            }
        }

        public static Color DefaultBackupMarkingColor
        {
            get
            {
                return Colors.OrangeRed;
            }
        }

        public static Color DefaultBackupMouseOverColor
        {
            get
            {
                return Colors.Yellow;
            }
        }

        public static Color DefaultBackupBackgroundColor
        {
            get
            {
                return Colors.Gray;
            }
        }

        public static Color DefaultBackupStartInterpColor
        {
            get
            {
                return Colors.LightBlue;
            }
        }

        public static Color DefaultBackupEndInterpColor
        {
            get
            {
                return Colors.DarkBlue;
            }
        }

        public static IList<StateAndColor> DefaultBackupStateColors
        {
            get
            {
                return myDefaultBackupStateColors;
            }
        }

        #endregion

        #region Members

        private static IList<StateAndColor> myDefaultBackupStateColors =
                        new List<StateAndColor> { 
                        new StateAndColor(0, Colors.White),
                        new StateAndColor(1, Colors.MediumBlue),
                        new StateAndColor(2, Colors.DarkSeaGreen),
                        new StateAndColor(3, Colors.LightGreen),
                        new StateAndColor(4, Colors.DarkGreen),
                        new StateAndColor(5, Colors.Brown),
                        new StateAndColor(6, Colors.Gold),
                        new StateAndColor(7, Colors.Red)
                        };

        #endregion
    }
}
