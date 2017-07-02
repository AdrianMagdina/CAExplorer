// programmed by Adrian Magdina in 2013
//in this file is the implementation of viewmodel for SetCAColors dialog.

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
    //implementation of StateAndColor class - encapsulates State and Color of that State in one class.
    public class StateAndColor
    {
        public StateAndColor()
        {
            State = 0;
            StateColor = Colors.Black;
        }

        public StateAndColor(int stateIn, Color stateColorIn)
        {
            State = stateIn;
            StateColor = stateColorIn;
        }

        public StateAndColor(StateAndColor stateAndColorIn)
        {
            State = stateAndColorIn.State;
            StateColor = stateAndColorIn.StateColor;
        }

        public int State {get; set;}

        public Color StateColor {get; set;}
    }

    //SetCAColors viewmodel implementation
    public class SetCAColorsVM : ViewModelBase
    {
        #region Constructors

        public SetCAColorsVM()
        {
            myStateColorsCollection = new ObservableCollection<StateAndColor>();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        //number of states in CA.
        public int StateCount
        {
            get
            {
                return myStateCount;
            }
            set
            {
                myStateCount = value;
            }
        }

        public ICommand OkPushedCommand
        {
            get
            {
                return myOkPushedCommand;
            }
            set
            {
                myOkPushedCommand = value;
            }
        }

        //chosen grid color.
        public Color SelectedGridColor
        {
            get
            {
                return mySelectedGridColor;
            }
            set
            {
                mySelectedGridColor = value;
                OnPropertyChanged("SelectedGridColor");
            }
        }

        //chosen selection frame color.
        public Color SelectedSelectionFrameColor
        {
            get
            {
                return mySelectedSelectionFrameColor;
            }
            set
            {
                mySelectedSelectionFrameColor = value;
                OnPropertyChanged("SelectedSelectionFrameColor");
            }
        }

        //chosen color of marked cells.
        public Color SelectedMarkingColor
        {
            get
            {
                return mySelectedMarkingColor;
            }
            set
            {
                mySelectedMarkingColor = value;
                OnPropertyChanged("SelectedMarkingColor");
            }
        }

        //chosen color of frame if mouse pointer is currently over the cell.
        public Color SelectedMouseOverColor
        {
            get
            {
                return mySelectedMouseOverColor;
            }
            set
            {
                mySelectedMouseOverColor = value;
                OnPropertyChanged("SelectedMouseOverColor");
            }
        }

        //chosen background color of frame.
        public Color SelectedBackgroundColor
        {
            get
            {
                return mySelectedBackgroundColor;
            }
            set
            {
                mySelectedBackgroundColor = value;
                OnPropertyChanged("SelectedBackgroundColor");
            }
        }

        public String StateColorsCountLabel
        {
            get
            {
                return "This CA has " + myStateCount + " States.";
            }
        }

        //chosen starting interpolation color.
        public Color SelectedStartInterpColor
        {
            get
            {
                return mySelectedStartInterpColor;
            }
            set
            {
                mySelectedStartInterpColor = value;
                OnPropertyChanged("SelectedStartInterpColor");
            }
        }

        //chosen ending interpolation color.
        public Color SelectedEndInterpColor
        {
            get
            {
                return mySelectedEndInterpColor;
            }
            set
            {
                mySelectedEndInterpColor = value;
                OnPropertyChanged("SelectedEndInterpColor");
            }
        }

        //chosen colors that are directly used by different states.
        public ObservableCollection<StateAndColor> StateColorsCollection
        {
            get
            {
                return myStateColorsCollection;
            }
        }

        //setting state colors directly radiobutton is enabled.
        public bool SetStateColorsDirectlyRBEnabled
        {
            get
            {
                return mySetStateColorsDirectlyRBEnabled;
            }
            set
            {
                mySetStateColorsDirectlyRBEnabled = value;
            }
        }

        //setting state colors directly was checked - chosen.
        public bool SetStateColorsDirectlyChecked
        {
            get
            {
                return mySetStateColorsDirectlyChecked;
            }
            set
            {
                mySetStateColorsDirectlyChecked = value;
                OnPropertyChanged("SetStateColorsDirectlyChecked");
                OnPropertyChanged("SetStateColorsDirectlyGBEnabled");
                OnPropertyChanged("SetStateColorsInterpGBEnabled");
            }
        }

        //setting interpolation of state colors checked - chosen.
        public bool SetStateColorsInterpChecked
        {
            get
            {
                return mySetStateColorsInterpChecked;
            }
            set
            {
                mySetStateColorsInterpChecked = value;
                OnPropertyChanged("SetStateColorsInterpChecked");
                OnPropertyChanged("SetStateColorsDirectlyGBEnabled");
                OnPropertyChanged("SetStateColorsInterpGBEnabled");
            }
        }

        //setting state colors directly groupbox is enabled.
        public bool SetStateColorsDirectlyGBEnabled
        {
            get
            {
                return SetStateColorsDirectlyChecked;
            }
        }

        //setting interpolated state colors groupbox is enabled.
        public bool SetStateColorsInterpGBEnabled
        {
            get
            {
                return SetStateColorsInterpChecked;
            }
        }

        #endregion

        #region Members

        private ICommand myOkPushedCommand;

        private Color mySelectedGridColor;
        private Color mySelectedSelectionFrameColor;
        private Color mySelectedMarkingColor;
        private Color mySelectedMouseOverColor;
        private Color mySelectedBackgroundColor;
        private Color mySelectedStartInterpColor;
        private Color mySelectedEndInterpColor;

        private bool mySetStateColorsDirectlyRBEnabled = false;

        private bool mySetStateColorsDirectlyChecked = false;
        private bool mySetStateColorsInterpChecked = true;

        private int myStateCount = 0;

        private ObservableCollection<StateAndColor> myStateColorsCollection = null;
        
        #endregion
    }
}
