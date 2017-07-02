// programmed by Adrian Magdina in 2013
// in this file is the implementation of View Model for "Application settings" dialog

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
    public class AppSettingsVM : ViewModelBase
    {
        #region Constructors

        public AppSettingsVM()
        {
            myDefaultStateColorsCollection = new ObservableCollection<StateAndColor>();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        //default color of grid
        public Color SelectedDefaultGridColor
        {
            get
            {
                return mySelectedDefaultGridColor;
            }
            set
            {
                mySelectedDefaultGridColor = value;
                OnPropertyChanged("SelectedDefaultGridColor");
            }
        }

        //default color shown if the cell is currently selected
        public Color SelectedDefaultSelectionFrameColor
        {
            get
            {
                return mySelectedDefaultSelectionFrameColor;
            }
            set
            {
                mySelectedDefaultSelectionFrameColor = value;
                OnPropertyChanged("SelectedDefaultSelectionFrameColor");
            }
        }

        //default color which is shown, if the cell was marked
        public Color SelectedDefaultMarkingColor
        {
            get
            {
                return mySelectedDefaultMarkingColor;
            }
            set
            {
                mySelectedDefaultMarkingColor = value;
                OnPropertyChanged("SelectedDefaultMarkingColor");
            }
        }

        //default color which is shown if mouse pointer is over cell
        public Color SelectedDefaultMouseOverColor
        {
            get
            {
                return mySelectedDefaultMouseOverColor;
            }
            set
            {
                mySelectedDefaultMouseOverColor = value;
                OnPropertyChanged("SelectedDefaultMouseOverColor");
            }
        }

        //default color which is shown if the cell is not marked, not selected, and the mouse pointer is not over cell.
        public Color SelectedDefaultBackgroundColor
        {
            get
            {
                return mySelectedDefaultBackgroundColor;
            }
            set
            {
                mySelectedDefaultBackgroundColor = value;
                OnPropertyChanged("SelectedDefaultBackgroundColor");
            }
        }

        public int DefaultStateColorsCount
        {
            get
            {
                return myDefaultStateColorsCollection.Count;
            }
        }

        //default start interpolation color
        public Color SelectedDefaultStartInterpColor
        {
            get
            {
                return mySelectedDefaultStartInterpColor;
            }
            set
            {
                mySelectedDefaultStartInterpColor = value;
                OnPropertyChanged("SelectedDefaultStartInterpColor");
            }
        }

        //default end interpolation color
        public Color SelectedDefaultEndInterpColor
        {
            get
            {
                return mySelectedDefaultEndInterpColor;
            }
            set
            {
                mySelectedDefaultEndInterpColor = value;
                OnPropertyChanged("SelectedDefaultEndInterpColor");
            }
        }

        //default colors for direct colors of states
        public ObservableCollection<StateAndColor> DefaultStateColorsCollection
        {
            get
            {
                return myDefaultStateColorsCollection;
            }
        }

        #endregion
        
        #region Members

        private Color mySelectedDefaultGridColor;
        private Color mySelectedDefaultSelectionFrameColor;
        private Color mySelectedDefaultMarkingColor;
        private Color mySelectedDefaultMouseOverColor;
        private Color mySelectedDefaultBackgroundColor;
        private Color mySelectedDefaultStartInterpColor;
        private Color mySelectedDefaultEndInterpColor;

        private ObservableCollection<StateAndColor> myDefaultStateColorsCollection = null; //collection with default state colors

        #endregion
    }
}
