// programmed by Adrian Magdina in 2013
// in this file is the implementation of model for CA Grid

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CAExplorerNamespace
{
    public delegate void CurrentIterationChanged(object sender, EventArgs e);
    public delegate void MaximumIterationChanged(object sender, EventArgs e);

    //in this class is stored the model of specific CA.
    public class CAGrid2DM
    {
        #region Constructors

        private CAGrid2DM()
        {
        }

        public CAGrid2DM(int columnsIn, int rowsIn, ICARuleFamily caRuleIn, ICAGridCellInitialization caGridCellInitializationIn)
        {
            myCell2DArray = new CellM[columnsIn, rowsIn];

            myCARule = caRuleIn;

            if (caRuleIn != null && caGridCellInitializationIn != null)
            {
                caGridCellInitializationIn.CreateAndInitializeCells(ref myCell2DArray, myCARule.NumberOfStates);
            }

            MaximumIteration = 0;
            CurrentIteration = 0;
        }

        #endregion

        #region Methods

        // returns the cell model with specific column and row.
        public CellM GetCellModel(int xIn, int yIn)
        {
            return Cell2DArray[xIn, yIn];
        }

        // adds next iteration of CA.
        public void AddNextIteration()
        {
            //computes next iteration for all cell models in this CA.
            ComputeAndSetNextStatesForAllCells();
            //increments the iteration count for this CA.
            CurrentIteration++;

            //if current iteration is higher than maximum defined iteration count, than all iterations above maximum will be deleted.
            //this is because of sparing the memory 
            int aToRemove = (CurrentIteration - Constants.MaxNrOfIterations);
            if (aToRemove > 0)
            {
                foreach (CellM aCellM in myCell2DArray)
                {
                    aCellM.RemoveFromCellStatesList(0, aToRemove);
                }

                CurrentIteration -= aToRemove;
            }

            if (CurrentIteration > MaximumIteration)
            {
                MaximumIteration = CurrentIteration;
            }
        }

        private void ComputeAndSetNextStatesForAllCells()
        {
            var aComputedCellStatesArray = new int[myCell2DArray.GetLength(0), myCell2DArray.GetLength(1)];

            //for all cells will be the next state computed and kept in an array.
            for (int x = 0; x < myCell2DArray.GetLength(0); x++)
            {
                for (int y = 0; y < myCell2DArray.GetLength(1); y++)
                {
                    int aNextState = ComputeNextStateForOneCell(myCell2DArray[x, y], x, y);
                    aComputedCellStatesArray[x, y] = aNextState;
                }
            }

            //the next state is copied to cell models
            for (int x = 0; x < myCell2DArray.GetLength(0); x++)
            {
                for (int y = 0; y < myCell2DArray.GetLength(1); y++)
                {
                    myCell2DArray[x, y].AddNextState(aComputedCellStatesArray[x, y]);
                }
            }
        }

        //the next state for specific cell is computed with specific neighborhood.
        private int ComputeNextStateForOneCell(CellM cellModelIn, int xIn, int yIn)
        {
            ICARuleData aCARuleData = myCARule.CARuleData;
            IList<NeighborhoodAreaItem> aNeighorboodArrayStates = CANeighborhood.GetNeighborhood(myCell2DArray, xIn, yIn, aCARuleData.CANeighborhoodType, aCARuleData.CANeighborhoodRange);

            return myCARule.ComputeCellState(aNeighorboodArrayStates, cellModelIn.CurrentCellState);
        }

        private void SetIterationToAllCells(int iterationIn)
        {
            if (iterationIn < 0)
            {
                throw new CAExplorerException("Iteration number is lower than zero!");
            }

            for (int x = 0; x < Cell2DArray.GetLength(0); x++)
            {
                for (int y = 0; y < Cell2DArray.GetLength(1); y++)
                {
                    Cell2DArray[x, y].CurrentCellIndex = iterationIn;
                }
            }
        }

        #endregion

        #region Properties

        public TesselationShapes2D TesselationShape
        {
            get
            {
                return myTesselationShape;
            }
            set
            {
                myTesselationShape = value;
            }
        }

        public CellM[,] Cell2DArray
        {
            get
            {
                return myCell2DArray;
            }
            set
            {
                myCell2DArray = value;
            }
        }

        public int Rows
        {
            get
            {
                return myCell2DArray.GetLength(1);
            }
        }

        public int Columns
        {
            get
            {
                return myCell2DArray.GetLength(0);
            }
        }

        public int? NumberOfStates
        {
            get
            {
                if (myCARule != null)
                {
                    return myCARule.NumberOfStates;
                }
                else
                {
                    return null;
                }
            }
        }

        //here is stored the specific Rule and all information connected to rule and rule computation.
        public ICARuleFamily CARule
        {
            get
            {
                return myCARule;
            }
            set
            {
                myCARule = value;
            }
        }

        public int CurrentIteration
        {
            get
            {
                return myCurrentIteration;
            }
            set
            {
                if (myCurrentIteration != value)
                {
                    myCurrentIteration = value;
                    SetIterationToAllCells(myCurrentIteration);
                    if (CurrentIterationChangedEvent != null)
                    {
                        CurrentIterationChangedEvent(this, null);
                    }
                }
            }
        }

        public int MaximumIteration
        {
            get
            {
                return myMaximumIteration;
            }
            private set
            {
                if (myMaximumIteration != value)
                {
                    myMaximumIteration = value;
                    if (MaximumIterationChangedEvent != null)
                    {
                        MaximumIterationChangedEvent(this, null);
                    }
                }
            }
        }

        //method how the CA will be initialized is stored here.
        public CAGridInitializationMethodTypes CAGridInitializationMethodType
        {
            get
            {
                return myCAGridInitializationMethodType;
            }
            set
            {
                myCAGridInitializationMethodType = value;
            }
        }

        //the neighborhood type that is used in this CA.
        public CANeighborhoodTypes CANeighborhoodType
        {
            get
            {
                return myCANeighborhoodType;
            }
            set
            {
                myCANeighborhoodType = value;
            }
        }

        //size of the neighborhood area.
        public int CANeighborhoodSize
        {
            get
            {
                return myCANeighborhoodSize;
            }
            set
            {
                myCANeighborhoodSize = value;
            }
        }

        #endregion

        #region Members
        
        public event CurrentIterationChanged CurrentIterationChangedEvent;
        public event MaximumIterationChanged MaximumIterationChangedEvent;

        private TesselationShapes2D myTesselationShape;

        private CellM[,] myCell2DArray = null;

        private ICARuleFamily myCARule = null;

        private CAGridInitializationMethodTypes myCAGridInitializationMethodType;

        private CANeighborhoodTypes myCANeighborhoodType;
        private int myCANeighborhoodSize = 0;

        private int myCurrentIteration = -1;
        private int myMaximumIteration = -1;

        #endregion
    }
}
