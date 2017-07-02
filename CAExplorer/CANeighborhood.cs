// programmed by Adrian Magdina in 2013
// in this file is the implementation of class for getting the cells which are forming a neighborhood.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAExplorerNamespace
{
    //struct which stores a value and position of neighborhood cell
    public struct NeighborhoodAreaItem
    {
        public NeighborhoodAreaItem(int leftIn, int upIn, int stateIn)
        {
            left = leftIn;
            up = upIn;
            state = stateIn;
        }

        public int left;
        public int up;
        public int state;
    }

    //static class for finding the neighborhood cells of a cell
    public static class CANeighborhood
    {
        public static IList<NeighborhoodAreaItem> GetNeighborhood(CellM[,] cell2DArrayIn, int xIn, int yIn, CANeighborhoodTypes caNeighborhoodTypeIn, int caNeighborhoodRangeIn)
        {
            var aNeighborhoodArrayList = new List<NeighborhoodAreaItem>();

            if (caNeighborhoodTypeIn == CANeighborhoodTypes.Moore) //finding Moore neighborhood
            {
                //computing start and end position for possible neighborhood area
                int xStart = xIn - caNeighborhoodRangeIn;
                int xEnd = xIn + caNeighborhoodRangeIn;

                int yStart = yIn - caNeighborhoodRangeIn;
                int yEnd = yIn + caNeighborhoodRangeIn;

                //going through all neighborhood positions
                for (int x = xStart; x <= xEnd; x++)
                {
                    for (int y = yStart; y <= yEnd; y++)
                    {
                        int xTemp = x;
                        int yTemp = y;

                        //if the neighborhood is outside of the area of CA, than use the cells from other side (end=start)
                        if (x < 0)
                        {
                            xTemp = cell2DArrayIn.GetLength(0) + x;
                        }
                        else if (x >= cell2DArrayIn.GetLength(0))
                        {
                            xTemp = x - cell2DArrayIn.GetLength(0);
                        }

                        //if the neighborhood is outside of the area of CA, than use the cells from other side (end=start)
                        if (y < 0)
                        {
                            yTemp = cell2DArrayIn.GetLength(1) + y;
                        }
                        else if (y >= cell2DArrayIn.GetLength(1))
                        {
                            yTemp = y - cell2DArrayIn.GetLength(1);
                        }

                        //add the found neighborhood cell to neighborhood cell list
                        aNeighborhoodArrayList.Add(new NeighborhoodAreaItem(x - xIn, y - yIn, cell2DArrayIn[xTemp, yTemp].CurrentCellState));
                    }
                }
            }
            else if (caNeighborhoodTypeIn == CANeighborhoodTypes.VonNeumann) //finding Von Neumann neighborhood
            {
                //computing start and end position for possible neighborhood area
                int xStart = xIn - caNeighborhoodRangeIn;
                int xEnd = xIn + caNeighborhoodRangeIn;

                int yStart = yIn - caNeighborhoodRangeIn;
                int yEnd = yIn + caNeighborhoodRangeIn;

                //going through all neighborhood positions
                for (int x = xStart; x <= xEnd; x++)
                {
                    for (int y = yStart; y <= yEnd; y++)
                    {
                        //sorting out all positions which are outside of Von Neumann Neighborhood
                        if ((Math.Abs(xIn - x) + Math.Abs(yIn - y)) > caNeighborhoodRangeIn)
                        {
                            continue;
                        }

                        int xTemp = x;
                        int yTemp = y;

                        //if the neighborhood is outside of the area of CA, than use the cells from other side (end=start)
                        if (x < 0)
                        {
                            xTemp = cell2DArrayIn.GetLength(0) + x;
                        }
                        else if (x >= cell2DArrayIn.GetLength(0))
                        {
                            xTemp = x - cell2DArrayIn.GetLength(0);
                        }

                        //if the neighborhood is outside of the area of CA, than use the cells from other side (end=start)
                        if (y < 0)
                        {
                            yTemp = cell2DArrayIn.GetLength(1) + y;
                        }
                        else if (y >= cell2DArrayIn.GetLength(1))
                        {
                            yTemp = y - cell2DArrayIn.GetLength(1);
                        }

                        //add the found neighborhood cell to neighborhood cell list
                        aNeighborhoodArrayList.Add(new NeighborhoodAreaItem(x - xIn, y - yIn, cell2DArrayIn[xTemp, yTemp].CurrentCellState));
                    }
                }
            }
            else
            {
                throw new CAExplorerException();
            }

            return aNeighborhoodArrayList; //list of all cells forming a neighborhood of input cell.
        }

    }
}
