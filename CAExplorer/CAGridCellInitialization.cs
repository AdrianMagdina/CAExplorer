// programmed by Adrian Magdina in 2013
// in this file is the implementation of classes that do the initialization of CA Grid

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAExplorerNamespace
{
    // all cells of grid will be initialized with random value from all possible states
    public class CAGridCellInitializationRandomAllValues : ICAGridCellInitialization
    {
        public CAGridCellInitializationRandomAllValues()
        {
        }

        public void CreateAndInitializeCells(ref CellM[,] cellsMIn, int countOfStatesIn)
        {
            Random aRandom = new Random();

            //going through all cells and setting the appropriate value.
            for (int column = 0; column < cellsMIn.GetLength(0); column++)
            {
                for (int row = 0; row < cellsMIn.GetLength(1); row++)
                {
                    int aRandomNumber = aRandom.Next(0, countOfStatesIn);
                    var aCellModel = new CellM(aRandomNumber, column, row, countOfStatesIn);

                    cellsMIn[column, row] = aCellModel;
                }
            }
        }

        public CAGridInitializationMethodTypes CAGridInitializationMethod
        {
            get
            {
                return CAGridInitializationMethodTypes.RandomAllValues;
            }
        }
    }

    // all cells of grid will be initialized with random value either zero or maximum state value
    public class CAGridCellInitializationRandomOnlyZeroAndMaximum : ICAGridCellInitialization
    {
        public CAGridCellInitializationRandomOnlyZeroAndMaximum()
        {
        }

        public void CreateAndInitializeCells(ref CellM[,] cellsMIn, int countOfStatesIn)
        {
            Random aRandom = new Random();

            //going through all cells and setting the appropriate value.
            for (int column = 0; column < cellsMIn.GetLength(0); column++)
            {
                for (int row = 0; row < cellsMIn.GetLength(1); row++)
                {
                    int aRandomNumber = aRandom.Next(0, 2);
                    if (aRandomNumber == 1)
                    {
                        aRandomNumber = countOfStatesIn - 1;
                    }

                    var aCellModel = new CellM(aRandomNumber, column, row, countOfStatesIn);

                    cellsMIn[column, row] = aCellModel;
                }
            }
        }

        public CAGridInitializationMethodTypes CAGridInitializationMethod
        {
            get
            {
                return CAGridInitializationMethodTypes.RandomOnlyZeroAndMaximum;
            }
        }
    }

    // only upper half cells of grid will be initialized with random value.
    public class CAGridCellInitializationUpperHalf : ICAGridCellInitialization
    {
        public CAGridCellInitializationUpperHalf()
        {
        }

        public void CreateAndInitializeCells(ref CellM[,] cellsMIn, int countOfStatesIn)
        {
            Random aRandom = new Random();

            int aUpToRow = (int)((double)cellsMIn.GetLength(1) / 2.0);

            //going through all cells and setting the appropriate value.
            for (int column = 0; column < cellsMIn.GetLength(0); column++)
            {
                for (int row = 0; row < cellsMIn.GetLength(1); row++)
                {
                    int aRandomNumber = 0;

                    if (row < aUpToRow)
                    {
                        aRandomNumber = aRandom.Next(0, countOfStatesIn);
                    }

                    var aCellModel = new CellM(aRandomNumber, column, row);

                    cellsMIn[column, row] = aCellModel;
                }
            }
        }

        public CAGridInitializationMethodTypes CAGridInitializationMethod
        {
            get
            {
                return CAGridInitializationMethodTypes.RandomUpperHalf;
            }
        }
    }

    // only upper half cells of grid will be initialized with random value - zero or maximum.
    public class CAGridCellInitializationUpperHalfZeroAndMaximum : ICAGridCellInitialization
    {
        public CAGridCellInitializationUpperHalfZeroAndMaximum()
        {
        }

        public void CreateAndInitializeCells(ref CellM[,] cellsMIn, int countOfStatesIn)
        {
            Random aRandom = new Random();

            int aUpToRow = (int)((double)cellsMIn.GetLength(1) / 2.0);

            //going through all cells and setting the appropriate value.
            for (int column = 0; column < cellsMIn.GetLength(0); column++)
            {
                for (int row = 0; row < aUpToRow; row++)
                {
                    int aRandomNumber = 0;

                    if (row < aUpToRow)
                    {
                        aRandomNumber = aRandom.Next(0, 2);
                    }

                    if (aRandomNumber == 1)
                    {
                        aRandomNumber = countOfStatesIn - 1;
                    }

                    var aCellModel = new CellM(aRandomNumber, column, row, countOfStatesIn);

                    cellsMIn[column, row] = aCellModel;
                }
            }
        }

        public CAGridInitializationMethodTypes CAGridInitializationMethod
        {
            get
            {
                return CAGridInitializationMethodTypes.RandomUpperHalfZeroAndMaximum;
            }
        }
    }


    // only lower half cells of grid will be initialized with random value.
    public class CAGridCellInitializationLowerHalf : ICAGridCellInitialization
    {
        public CAGridCellInitializationLowerHalf()
        {
        }

        public void CreateAndInitializeCells(ref CellM[,] cellsMIn, int countOfStatesIn)
        {
            Random aRandom = new Random();

            int aFromRow = (int)((double)cellsMIn.GetLength(1) / 2.0);

            //going through all cells and setting the appropriate value.
            for (int column = 0; column < cellsMIn.GetLength(0); column++)
            {
                for (int row = 0; row < cellsMIn.GetLength(1); row++)
                {
                    int aRandomNumber = 0;

                    if (row >= aFromRow)
                    {
                        aRandomNumber = aRandom.Next(0, countOfStatesIn);
                    }

                    var aCellModel = new CellM(aRandomNumber, column, row);

                    cellsMIn[column, row] = aCellModel;
                }
            }
        }

        public CAGridInitializationMethodTypes CAGridInitializationMethod
        {
            get
            {
                return CAGridInitializationMethodTypes.RandomLowerHalf;
            }
        }
    }

    // only lower half cells of grid will be initialized with random value - zero or maximum.
    public class CAGridCellInitializationLowerHalfZeroAndMaximum : ICAGridCellInitialization
    {
        public CAGridCellInitializationLowerHalfZeroAndMaximum()
        {
        }

        public void CreateAndInitializeCells(ref CellM[,] cellsMIn, int countOfStatesIn)
        {
            Random aRandom = new Random();

            int aFromRow = (int)((double)cellsMIn.GetLength(1) / 2.0);

            //going through all cells and setting the appropriate value.
            for (int column = 0; column < cellsMIn.GetLength(0); column++)
            {
                for (int row = 0; row < cellsMIn.GetLength(1); row++)
                {
                    int aRandomNumber = 0;

                    if (row >= aFromRow)
                    {
                        aRandomNumber = aRandom.Next(0, 2);
                    }

                    if (aRandomNumber == 1)
                    {
                        aRandomNumber = countOfStatesIn - 1;
                    }

                    var aCellModel = new CellM(aRandomNumber, column, row, countOfStatesIn);

                    cellsMIn[column, row] = aCellModel;
                }
            }
        }

        public CAGridInitializationMethodTypes CAGridInitializationMethod
        {
            get
            {
                return CAGridInitializationMethodTypes.RandomLowerHalfZeroAndMaximum;
            }
        }
    }

    // only cells in upper left corner will be initialized with random value.
    public class CAGridCellInitializationUpperLeft : ICAGridCellInitialization
    {
        public CAGridCellInitializationUpperLeft()
        {
        }

        public void CreateAndInitializeCells(ref CellM[,] cellsMIn, int countOfStatesIn)
        {
            Random aRandom = new Random();

            int aUpToRow = (int)((double)cellsMIn.GetLength(1) / 2.0);
            int aUpToColumn = (int)((double)cellsMIn.GetLength(0) / 2.0);

            //going through all cells and setting the appropriate value.
            for (int column = 0; column < cellsMIn.GetLength(0); column++)
            {
                for (int row = 0; row < cellsMIn.GetLength(1); row++)
                {
                    int aRandomNumber = 0;

                    if (row < aUpToRow && column < aUpToColumn)
                    {
                        aRandomNumber = aRandom.Next(0, countOfStatesIn);
                    }

                    var aCellModel = new CellM(aRandomNumber, column, row, countOfStatesIn);

                    cellsMIn[column, row] = aCellModel;
                }
            }
        }

        public CAGridInitializationMethodTypes CAGridInitializationMethod
        {
            get
            {
                return CAGridInitializationMethodTypes.RandomUpperLeft;
            }
        }
    }

    // only cells in upper left corner will be initialized with random value - zero or maximum.
    public class CAGridCellInitializationUpperLeftZeroAndMaximum : ICAGridCellInitialization
    {
        public CAGridCellInitializationUpperLeftZeroAndMaximum()
        {
        }

        public void CreateAndInitializeCells(ref CellM[,] cellsMIn, int countOfStatesIn)
        {
            Random aRandom = new Random();

            int aUpToRow = (int)((double)cellsMIn.GetLength(1) / 2.0);
            int aUpToColumn = (int)((double)cellsMIn.GetLength(0) / 2.0);

            //going through all cells and setting the appropriate value.
            for (int column = 0; column < cellsMIn.GetLength(0); column++)
            {
                for (int row = 0; row < cellsMIn.GetLength(1); row++)
                {
                    int aRandomNumber = 0;

                    if (row < aUpToRow && column < aUpToColumn)
                    {
                        aRandomNumber = aRandom.Next(0, 2);
                    }

                    if (aRandomNumber == 1)
                    {
                        aRandomNumber = countOfStatesIn - 1;
                    }

                    var aCellModel = new CellM(aRandomNumber, column, row, countOfStatesIn);

                    cellsMIn[column, row] = aCellModel;
                }
            }
        }

        public CAGridInitializationMethodTypes CAGridInitializationMethod
        {
            get
            {
                return CAGridInitializationMethodTypes.RandomUpperHalfZeroAndMaximum;
            }
        }
    }

    // only cells in lower left corner will be initialized with random value.
    public class CAGridCellInitializationLowerLeft : ICAGridCellInitialization
    {
        public CAGridCellInitializationLowerLeft()
        {
        }

        public void CreateAndInitializeCells(ref CellM[,] cellsMIn, int countOfStatesIn)
        {
            Random aRandom = new Random();

            int aFromRow = (int)((double)cellsMIn.GetLength(1) / 2.0);
            int aUpToColumn = (int)((double)cellsMIn.GetLength(0) / 2.0);

            //going through all cells and setting the appropriate value.
            for (int column = 0; column < cellsMIn.GetLength(0); column++)
            {
                for (int row = 0; row < cellsMIn.GetLength(1); row++)
                {
                    int aRandomNumber = 0;

                    if (row >= aFromRow && column < aUpToColumn)
                    {
                        aRandomNumber = aRandom.Next(0, countOfStatesIn);
                    }

                    var aCellModel = new CellM(aRandomNumber, column, row, countOfStatesIn);

                    cellsMIn[column, row] = aCellModel;
                }
            }
        }

        public CAGridInitializationMethodTypes CAGridInitializationMethod
        {
            get
            {
                return CAGridInitializationMethodTypes.RandomLowerLeft;
            }
        }
    }

    // only cells in lower left corner will be initialized with random value - zero or maximum.
    public class CAGridCellInitializationLowerLeftZeroAndMaximum : ICAGridCellInitialization
    {
        public CAGridCellInitializationLowerLeftZeroAndMaximum()
        {
        }

        public void CreateAndInitializeCells(ref CellM[,] cellsMIn, int countOfStatesIn)
        {
            Random aRandom = new Random();

            int aFromRow = (int)((double)cellsMIn.GetLength(1) / 2.0);
            int aUpToColumn = (int)((double)cellsMIn.GetLength(0) / 2.0);

            //going through all cells and setting the appropriate value.
            for (int column = 0; column < cellsMIn.GetLength(0); column++)
            {
                for (int row = 0; row < cellsMIn.GetLength(1); row++)
                {
                    int aRandomNumber = 0;

                    if (row >= aFromRow && column < aUpToColumn)
                    {
                        aRandomNumber = aRandom.Next(0, 2);
                    }

                    if (aRandomNumber == 1)
                    {
                        aRandomNumber = countOfStatesIn - 1;
                    }

                    var aCellModel = new CellM(aRandomNumber, column, row, countOfStatesIn);

                    cellsMIn[column, row] = aCellModel;
                }
            }
        }

        public CAGridInitializationMethodTypes CAGridInitializationMethod
        {
            get
            {
                return CAGridInitializationMethodTypes.RandomLowerLeftZeroAndMaximum;
            }
        }
    }

    // all cells will be set to maximum value.
    public class CAGridCellInitializationAllMaximum : ICAGridCellInitialization
    {
        public CAGridCellInitializationAllMaximum()
        {
        }

        public void CreateAndInitializeCells(ref CellM[,] cellsMIn, int countOfStatesIn)
        {
            //going through all cells and setting the appropriate value.
            for (int column = 0; column < cellsMIn.GetLength(0); column++)
            {
                for (int row = 0; row < cellsMIn.GetLength(1); row++)
                {
                    var aCellModel = new CellM(countOfStatesIn-1, column, row, countOfStatesIn);

                    cellsMIn[column, row] = aCellModel;
                }
            }
        }

        public CAGridInitializationMethodTypes CAGridInitializationMethod
        {
            get
            {
                return CAGridInitializationMethodTypes.AllMaximum;
            }
        }
    }

    // all cells will be set to zero value.
    public class CAGridCellInitializationAllZero : ICAGridCellInitialization
    {
        public CAGridCellInitializationAllZero()
        {
        }

        public void CreateAndInitializeCells(ref CellM[,] cellsMIn, int countOfStatesIn)
        {
            //going through all cells and setting the appropriate value.
            for (int column = 0; column < cellsMIn.GetLength(0); column++)
            {
                for (int row = 0; row < cellsMIn.GetLength(1); row++)
                {
                    var aCellModel = new CellM(0, column, row, countOfStatesIn);

                    cellsMIn[column, row] = aCellModel;
                }
            }
        }

        public CAGridInitializationMethodTypes CAGridInitializationMethod
        {
            get
            {
                return CAGridInitializationMethodTypes.AllZero;
            }
        }
    }
}
