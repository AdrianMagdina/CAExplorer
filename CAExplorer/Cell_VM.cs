// programmed by Adrian Magdina in 2013
// in this file is the implementation of viewmodel for a cell from CA.

using System;
using System.Windows.Input;

namespace CAExplorerNamespace
{
    public delegate void CellStateChangedDelegate(Object sender, EventArgs e);

    //viewmodel of Cell
    public sealed class CellVM : ViewModelBase, IDisposable
    {
        #region Constructors

        private CellVM()
        {
        }

        public CellVM(CellM cellModelIn, int xIn, int yIn)
        {
            X = xIn;
            Y = yIn;

            myCellModel = cellModelIn;

            myNewPossibleCellState = Constants.BaseCellStateForAllAvailableCAs;

            myCellModel.CurrentCellStateChanged += ChangeFillColor;
            myCellModel.CurrentCellStateChanged += NotifyAboutStateChanged;

            CellStateChangedCommand = new RelayCommand(new Action<object>(CellStateChanged));
        }

        #endregion

        #region Methods

        //if cell state changed then message, then tell all who want to know
        public void CellStateChanged(object messageIn)
        {
            myCellModel.CurrentCellState = myNewPossibleCellState;

            if (CellStateChangedEvent != null)
            {
                CellStateChangedEvent(this, null);
            }
        }

        public void ChangeFillColor(object sender, EventArgs e)
        {
            OnPropertyChanged("FillColor");
        }

        public void NotifyAboutStateChanged(object sender, EventArgs e)
        {
            OnPropertyChanged("State");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposingIn)
        {
            if (disposingIn)
            {
                myCellModel.CurrentCellStateChanged -= ChangeFillColor;
                myCellModel.CurrentCellStateChanged -= NotifyAboutStateChanged;
            }
        }
        
        //implemenation if IDataErrorInfo - validation of input data.
        public string this[string columnName]
        {
            get
            {
                string aResult = null;

                //validation of input for NewPossibleCellState
                if (columnName == "NewPossibleCellState")
                {
                    if (NewPossibleCellState < Constants.BaseCellStateForAllAvailableCAs || NewPossibleCellState > this.CellModel.CountOfStates)
                    {
                        aResult = "Value of Cell Size X must be between: " + Constants.BaseCellStateForAllAvailableCAs + " - " + this.CellModel.CountOfStates + "!";
                    }
                }

                return aResult;
            }
        }

        #endregion

        #region Properties

        //x position of this cell in points.
        public double X
        {
            get
            {
                return myX;
            }
            set
            {
                myX=value;
                OnPropertyChanged("X");
            }
        }

        //y position of this cell in points.
        public double Y
        {
            get
            {
                return myY;
            }
            set
            {
                myY = value;
                OnPropertyChanged("Y");
            }
        }

        //model for this viewmodel.
        public CellM CellModel
        {
            get
            {
                return myCellModel;
            }
            set
            {
                myCellModel = value;
            }
        }

        //is this cell selected.
        public bool IsSelected
        {
            get
            {
                return myIsSelected;
            }
            set
            {
                myIsSelected = value;
            }
        }

        //was this cell marked.
        public bool IsMarked
        {
            get
            {
                return myIsMarked;
            }
            set
            {
                myIsMarked = value;
            }
        }

        //current state of this cell.
        public int State
        {
            get
            {
                return myCellModel.CurrentCellState;
            }
        }

        public int Column
        {
            get
            {
                return myCellModel.X;
            }
        }

        public int Row
        {
            get
            {
                return myCellModel.Y;
            }
        }

        //new cell state that was entered
        public int NewPossibleCellState
        {
            get
            {
                return myNewPossibleCellState;
            }
            set
            {
                if (NewPossibleCellState >= Constants.BaseCellStateForAllAvailableCAs && NewPossibleCellState < this.CellModel.CountOfStates)
                {
                    myNewPossibleCellState = value;
                    OnPropertyChanged("NewPossibleCellState");
                }
            }
        }

        public ICommand CellStateChangedCommand
        {
            get
            {
                return myCellStateChangedCommand;
            }
            set
            {
                myCellStateChangedCommand = value;
            }
        }

        #endregion

        #region Members

        public event CellStateChangedDelegate CellStateChangedEvent;

        private CellM myCellModel = null;

        private double myX = 0;
        private double myY = 0;

        private bool myIsSelected = false;
        private bool myIsMarked = false;

        private int myNewPossibleCellState = 0;

        private ICommand myCellStateChangedCommand = null;

        #endregion
    }
}
