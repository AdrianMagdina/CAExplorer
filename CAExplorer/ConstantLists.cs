// programmed by Adrian Magdina in 2013
// in this file is the implementation of lists with constants.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace CAExplorerNamespace
{
    public sealed class ComboBoxItem
    {
        public ComboBoxItem()
        {
            ComboBoxId = 0;
            ComboBoxString = null;
        }

        public int ComboBoxId { get; set; }
        public string ComboBoxString { get; set; }
    }

    //constant lists of constant values used in this application are defined here. (most lists are used in comboboxes)
    public static class ConstantLists
    {
        public static IList<ComboBoxItem> CAGridThicknessItems
        {
            get
            {
                return myCAGridThicknessItems;
            }
        }

        private static readonly IList<ComboBoxItem> myCAGridThicknessItems =
            new List<ComboBoxItem> { 
                new ComboBoxItem{ComboBoxId=(int)LineThickness.None, ComboBoxString="None"}, 
                new ComboBoxItem{ComboBoxId=(int)LineThickness.Thin, ComboBoxString="Thin"},
                new ComboBoxItem{ComboBoxId=(int)LineThickness.Medium, ComboBoxString="Medium"}, 
                new ComboBoxItem{ComboBoxId=(int)LineThickness.Thick, ComboBoxString="Thick"}
            };

        public static string FirstTimeCAGridThicknessItem
        {
            get
            {
                return myFirstTimeCAGridThicknessItem;
            }
        }

        private const string myFirstTimeCAGridThicknessItem = "Thin";

        public static IList<ComboBoxItem> CASelFrameThicknessItems
        {
            get
            {
                return myCASelFrameThicknessItems;
            }
        }

        private static readonly IList<ComboBoxItem> myCASelFrameThicknessItems =
            new List<ComboBoxItem> { 
                new ComboBoxItem{ComboBoxId = (int)LineThickness.None, ComboBoxString="None"}, 
                new ComboBoxItem{ComboBoxId = (int)LineThickness.Thin, ComboBoxString="Thin"},
                new ComboBoxItem{ComboBoxId = (int)LineThickness.Medium, ComboBoxString="Medium"}, 
                new ComboBoxItem{ComboBoxId = (int)LineThickness.Thick, ComboBoxString="Thick"}
            };

        public static string FirstTimeCASelFrameThicknessItem
        {
            get
            {
                return myFirstTimeCASelFrameThicknessItem;
            }
        }

        private const string myFirstTimeCASelFrameThicknessItem = "Thin";

        public static IList<ComboBoxItem> CARuleFamilyItems
        {
            get
            {
                return myCARuleFamilyItems;
            }
        }

        private static readonly IList<ComboBoxItem> myCARuleFamilyItems =
            new List<ComboBoxItem> {
            new ComboBoxItem{ComboBoxId=(int)CARuleFamilies.Life, ComboBoxString="Life"},
            new ComboBoxItem{ComboBoxId=(int)CARuleFamilies.Generations, ComboBoxString="Generations"},
            new ComboBoxItem{ComboBoxId=(int)CARuleFamilies.Cyclic, ComboBoxString="Cyclic"}
        };

        public static string FirstTimeCARuleFamily
        {
            get
            {
                return myFirstTimeCARuleFamily;
            }
        }

        private const string myFirstTimeCARuleFamily = "Life";

        public static IList<ComboBoxItem> CAInitializationMethodItems
        {
            get
            {
                return myCAInitializationMethodItems;
            }
        }

        private static readonly IList<ComboBoxItem> myCAInitializationMethodItems =
            new List<ComboBoxItem> {
                new ComboBoxItem{ComboBoxId=(int)CAGridInitializationMethodTypes.RandomAllValues, ComboBoxString="Random All Values"},
                new ComboBoxItem{ComboBoxId=(int)CAGridInitializationMethodTypes.RandomOnlyZeroAndMaximum, ComboBoxString="Random Only Zero And Maximum"},
                new ComboBoxItem{ComboBoxId=(int)CAGridInitializationMethodTypes.RandomUpperHalf, ComboBoxString="Random Upper Half"},
                new ComboBoxItem{ComboBoxId=(int)CAGridInitializationMethodTypes.RandomUpperHalfZeroAndMaximum, ComboBoxString="Random Upper Half Only Zero And Maximum"},
                new ComboBoxItem{ComboBoxId=(int)CAGridInitializationMethodTypes.RandomLowerHalf, ComboBoxString="Random Lower Half"},
                new ComboBoxItem{ComboBoxId=(int)CAGridInitializationMethodTypes.RandomLowerHalfZeroAndMaximum, ComboBoxString="Random Lower Half Only Zero And Maximum"},
                new ComboBoxItem{ComboBoxId=(int)CAGridInitializationMethodTypes.RandomUpperLeft, ComboBoxString="Random Upper Left"},
                new ComboBoxItem{ComboBoxId=(int)CAGridInitializationMethodTypes.RandomUpperLeftZeroAndMaximum, ComboBoxString="Random Upper Left Only Zero And Maximum"},
                new ComboBoxItem{ComboBoxId=(int)CAGridInitializationMethodTypes.RandomLowerLeft, ComboBoxString="Random Lower Left"},
                new ComboBoxItem{ComboBoxId=(int)CAGridInitializationMethodTypes.RandomLowerLeftZeroAndMaximum, ComboBoxString="Random Lower Left Only Zero And Maximum"},
                new ComboBoxItem{ComboBoxId=(int)CAGridInitializationMethodTypes.AllZero, ComboBoxString="All Zero"},
                new ComboBoxItem{ComboBoxId=(int)CAGridInitializationMethodTypes.AllMaximum, ComboBoxString="All Maximum"}
            };

        public static string FirstTimeCAInitializationMethod
        {
            get
            {
                return myFirstTimeCAInitializationMethod;
            }
        }

        private const string myFirstTimeCAInitializationMethod = "Random All Values";

        public static IList<ComboBoxItem> CANeighborhoodItems
        {
            get
            {
                return myCANeighborhoodItems;
            }
        }

        private static readonly IList<ComboBoxItem> myCANeighborhoodItems =
            new List<ComboBoxItem> {
                new ComboBoxItem{ComboBoxId=(int)CANeighborhoodTypes.Moore, ComboBoxString="Moore"},
                new ComboBoxItem{ComboBoxId=(int)CANeighborhoodTypes.VonNeumann, ComboBoxString="Von Neumann"}
            };

        public static IDictionary<LineThickness, int> LineThicknessValue
        {
            get
            {
                return myLineThicknessValue;
            }
        }

        private static readonly IDictionary<LineThickness, int> myLineThicknessValue = new Dictionary<LineThickness, int> {
            {LineThickness.None, 0},
            {LineThickness.Thin, 1},
            {LineThickness.Medium, 2},
            {LineThickness.Thick, 3}
        };
    }
}
