// programmed by Adrian Magdina in 2013
// in this file is the implementation of constants.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAExplorerNamespace
{
    //constant values that are used in this application are defined here.
    public static class Constants
    {
        public static int MaxColorCountForDirectColors
        {
            get
            {
                return 8;
            }
        }

        public static int MaxNrOfCellularAutomatons
        {
            get
            {
                return 50;
            }
        }

        public static int MaxNrOfIterations
        {
            get
            {
                return 100;
            }
        }

        public static int TimerSpeedInitializationValue
        {
            get
            {
                return 1200;
            }
        }

        public static int TimerSpeedSlow
        {
            get
            {
                return 2200;
            }
        }

        public static int TimerSpeedMedium
        {
            get
            {
                return 1200;
            }
        }

        public static int TimerSpeedFast
        {
            get
            {
                return 200;
            }
        }

        public static int BaseCellStateForAllAvailableCAs
        {
            get
            {
                return 0;
            }
        }

        public static int MaxCAColumns
        {
            get
            {
                return 100;
            }
        }

        public static int MinCAColumns
        {
            get
            {
                return 10;
            }
        }

        public static int MaxCARows
        {
            get
            {
                return 100;
            }
        }

        public static int MinCARows
        {
            get
            {
                return 10;
            }
        }

        public static int MaxCellSizeX
        {
            get
            {
                return 30;
            }
        }

        public static int MinCellSizeX
        {
            get
            {
                return 5;
            }
        }

        public static int MaxCellSizeY
        {
            get
            {
                return 30;
            }
        }

        public static int MinCellSizeY
        {
            get
            {
                return 5;
            }
        }

    }
}
