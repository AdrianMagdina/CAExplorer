// programmed by Adrian Magdina in 2013
// in this file is the definition of interfaces and enums.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAExplorerNamespace
{
    public enum TesselationShapes2D
    {
        None,
        EquilateralTriangle,
        Square,
        Hexagon
    }

    public enum GridThicknessTypes
    {
        Thin,
        Medium,
        Thick
    }

    public enum LineThickness
    {
        None,
        Thin,
        Medium,
        Thick
    };

    public enum CARuleFamilies
    {
        Life,
        Cyclic,
        Generations,
    }

    public enum CAGridInitializationMethodTypes
    {
        RandomAllValues,
        RandomOnlyZeroAndMaximum,
        RandomUpperHalf,
        RandomUpperHalfZeroAndMaximum,
        RandomLowerHalf,
        RandomLowerHalfZeroAndMaximum,
        RandomUpperLeft,
        RandomUpperLeftZeroAndMaximum,
        RandomLowerLeft,
        RandomLowerLeftZeroAndMaximum,
        AllZero,
        AllMaximum
    }

    public enum CANeighborhoodTypes
    {
        Moore,
        VonNeumann
    }

    public interface ICARuleData
    {
        CARuleFamilies CARuleFamily { get; set; }
        string CARuleName { get; set; }

        IList<int> Survival { get; set; }
        IList<int> Birth { get; set; }
        int? CountOfStates { get; set; }
        int? Threshold { get; set; }

        CANeighborhoodTypes CANeighborhoodType { get; set; }
        int CANeighborhoodRange { get; set; }
    }

    public interface ICARuleFamily
    {
        int ComputeCellState(IList<NeighborhoodAreaItem> cellValuesIn, int currentCellStateIn);
        int NumberOfStates { get; set; }
        string CARuleFamilyString { get; }
        CARuleFamilies CARuleFamily { get; }
        ICARuleData CARuleData { get; }
    }

    public interface ICAGridCellInitialization
    {
        void CreateAndInitializeCells(ref CellM[,] cellsMIn, int countOfStatesIn);

        CAGridInitializationMethodTypes CAGridInitializationMethod { get; }
    }
}

