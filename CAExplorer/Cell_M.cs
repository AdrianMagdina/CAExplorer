// programmed by Adrian Magdina in 2013
// in this file is the implementation of model for a cell from CA.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAExplorerNamespace
{
    public delegate void CurrentCellStateChangedEventHandler(object sender, EventArgs e);

    //model of Cell
    public class CellM
    {
        #region Contructors

        private CellM()
        {
        }

        public CellM(int xIn, int yIn, int countOfStatesIn)
            : this()
        {
            myCellStates = new List<int>();
            myNeighborhoodCells = new List<CellM>();

            myX = xIn;
            myY = yIn;

            myCountOfStates = countOfStatesIn;
        }

        public CellM(int currentCellStateIn, int xIn, int yIn, int countOfStatesIn)
            : this()
        {
            myCellStates = new List<int>();
            myNeighborhoodCells = new List<CellM>();

            myCellStates.Add(currentCellStateIn);
            CurrentCellIndex = 0;

            myCountOfStates = countOfStatesIn;

            myX = xIn;
            myY = yIn;
        }

        #endregion

        #region Methods

        //adding next state to CellStates list
        public void AddNextState(int nextStateIn)
        {
            CellStates.Add(nextStateIn);
        }

        public void RemoveFromCellStatesList(int startIndexIn, int numberToRemoveIn)
        {
            List<int> aCellStatesList = myCellStates as List<int>;
            if (aCellStatesList != null)
            {
                aCellStatesList.RemoveRange(startIndexIn, numberToRemoveIn);
            }
        }

        #endregion

        #region Properties

        //current cell index
        public int CurrentCellIndex
        {
            get
            {
                return myCurrentCellIndex;
            }
            set
            {
                if (myCurrentCellIndex != value)
                {
                    myCurrentCellIndex = value;

                    if (CurrentCellStateChanged != null)
                    {
                        CurrentCellStateChanged(this, new EventArgs());
                    }
                }
            }
        }

        //current cell state - state of cell with current cell index
        public int CurrentCellState
        {
            get
            {
                return myCellStates[CurrentCellIndex];
            }
            set
            {
                if (myCellStates[CurrentCellIndex] != value)
                {
                    myCellStates[CurrentCellIndex] = value;

                    if (CurrentCellStateChanged != null)
                    {
                        CurrentCellStateChanged(this, new EventArgs());
                    }
                }
            }
        }

        //list of former and current cell states
        public IList<int> CellStates
        {
            get
            {
                return myCellStates;
            }
        }

        //neighborhood cells for this cell
        public IList<CellM> NeighborhoodCells
        {
            get
            {
                return myNeighborhoodCells;
            }
        }

        //x value of cell in CA (column)
        public int X
        {
            get
            {
                return myX;
            }
            set
            {
                myX = value;
            }
        }

        //y value of cell in CA (row)
        public int Y
        {
            get
            {
                return myY;
            }
            set
            {
                myY = value;
            }
        }

        //how many states has this CA.
        public int CountOfStates
        {
            get
            {
                return myCountOfStates;
            }
            set
            {
                myCountOfStates = value;
            }
        }

        #endregion

        #region Members

        public event CurrentCellStateChangedEventHandler CurrentCellStateChanged;

        private IList<int> myCellStates = null;

        private IList<CellM> myNeighborhoodCells = null;

        private int myCurrentCellIndex = -1;

        private int myX = 0;
        private int myY = 0;

        private int myCountOfStates = 0;

        #endregion
    }
}
