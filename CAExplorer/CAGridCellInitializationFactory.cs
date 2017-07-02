// programmed by Adrian Magdina in 2013
// in this file is the implementation of factory for creation of instances for CA Grid initialization.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAExplorerNamespace
{
    //in this factory, depending on method of CA grid initialization - instance of specific class is created (which corresponds to wanted CA grid cell initialization).
    public static class CAGridCellInitializationFactory
    {
        public static ICAGridCellInitialization CreateCAGridCellInitialization( CAGridInitializationMethodTypes caGridInitializationMethodTypesIn)
        {
            ICAGridCellInitialization aCAGridCellInitialization = null;

            if (caGridInitializationMethodTypesIn == CAGridInitializationMethodTypes.RandomAllValues)
            {
                aCAGridCellInitialization = new CAGridCellInitializationRandomAllValues();
            }
            else if (caGridInitializationMethodTypesIn == CAGridInitializationMethodTypes.RandomOnlyZeroAndMaximum)
            {
                aCAGridCellInitialization = new CAGridCellInitializationRandomOnlyZeroAndMaximum();
            }
            else if (caGridInitializationMethodTypesIn == CAGridInitializationMethodTypes.RandomUpperHalf)
            {
                aCAGridCellInitialization = new CAGridCellInitializationUpperHalf();
            }
            else if (caGridInitializationMethodTypesIn == CAGridInitializationMethodTypes.RandomUpperHalfZeroAndMaximum)
            {
                aCAGridCellInitialization = new CAGridCellInitializationUpperHalfZeroAndMaximum();
            }
            else if (caGridInitializationMethodTypesIn == CAGridInitializationMethodTypes.RandomLowerHalf)
            {
                aCAGridCellInitialization = new CAGridCellInitializationLowerHalf();
            }
            else if (caGridInitializationMethodTypesIn == CAGridInitializationMethodTypes.RandomLowerHalfZeroAndMaximum)
            {
                aCAGridCellInitialization = new CAGridCellInitializationLowerHalfZeroAndMaximum();
            }
            else if (caGridInitializationMethodTypesIn == CAGridInitializationMethodTypes.RandomUpperLeft)
            {
                aCAGridCellInitialization = new CAGridCellInitializationUpperLeft();
            }
            else if (caGridInitializationMethodTypesIn == CAGridInitializationMethodTypes.RandomUpperLeftZeroAndMaximum)
            {
                aCAGridCellInitialization = new CAGridCellInitializationUpperLeftZeroAndMaximum();
            }
            else if (caGridInitializationMethodTypesIn == CAGridInitializationMethodTypes.RandomLowerLeft)
            {
                aCAGridCellInitialization = new CAGridCellInitializationLowerLeft();
            }
            else if (caGridInitializationMethodTypesIn == CAGridInitializationMethodTypes.RandomLowerLeftZeroAndMaximum)
            {
                aCAGridCellInitialization = new CAGridCellInitializationLowerLeftZeroAndMaximum();
            }
            else if (caGridInitializationMethodTypesIn == CAGridInitializationMethodTypes.AllZero)
            {
                aCAGridCellInitialization = new CAGridCellInitializationAllZero();
            }
            else if (caGridInitializationMethodTypesIn == CAGridInitializationMethodTypes.AllMaximum)
            {
                aCAGridCellInitialization = new CAGridCellInitializationAllMaximum();
            }

            return aCAGridCellInitialization;
        }
    }
}
