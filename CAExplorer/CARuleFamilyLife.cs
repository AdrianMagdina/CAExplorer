// programmed by Adrian Magdina in 2013
// in this file is the implementation of life rule family.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAExplorerNamespace
{
    //implementation of Life CA rule family
    public class CARuleFamilyLife : ICARuleFamily
    {
        #region Constructors

        private CARuleFamilyLife()
        {
        }

        public CARuleFamilyLife(ICARuleData caRuleDataIn)
        {
            myCARuleData = caRuleDataIn;
            myNumberOfStates = 2;
        }

        #endregion

        #region Methods

        public int ComputeCellState(IList<NeighborhoodAreaItem> cellValuesIn, int currentCellStateIn)
        {
            int aCellsOn = 0;

            //determining how many neighborhood cells are on (have maximum cell state value)
            foreach (NeighborhoodAreaItem aNAItem in cellValuesIn)
            {
                if (aNAItem.state == 1)
                {
                    aCellsOn++;
                }
            }

            //if this cell is on than cell count in on state is minus one (because this cell is in the neighborhood as well).
            if (currentCellStateIn == 1)
            {
                aCellsOn--;
            }

            //if number of cells on is equal to defined Birth number for this rule, then cell is born - set to 1.
            foreach (int aBirth in myCARuleData.Birth)
            {
                if (aCellsOn == aBirth)
                {
                    return 1;
                }
            }

            //if number of cells on is equal to defined Survival number for this rule, and cell hase maximum value, than stays in maximum value.
            foreach (int aSurvival in myCARuleData.Survival)
            {
                if (aCellsOn == aSurvival)
                {
                    return currentCellStateIn;
                }
            }

            //if here return 0 as cell state.
            return 0;
        }

        #endregion

        #region Properties

        public int NumberOfStates
        {
            get
            {
                return myNumberOfStates;
            }
            set
            {
                myNumberOfStates = value;
            }
        }

        //obtaining CA rule family string from enum.
        public string CARuleFamilyString
        {
            get
            {
                string aCARuleFamilyString = null;

                foreach (ComboBoxItem aCARuleFamilyItem in ConstantLists.CARuleFamilyItems)
                {
                    if ((CARuleFamilies)aCARuleFamilyItem.ComboBoxId == CARuleFamily)
                    {
                        aCARuleFamilyString = aCARuleFamilyItem.ComboBoxString;
                        break;
                    }
                }

                return aCARuleFamilyString;
            }
        }

        public CARuleFamilies CARuleFamily
        {
            get
            {
                return CARuleFamilies.Life;
            }
        }

        //data for this specific rule of this rule family.
        public ICARuleData CARuleData
        {
            get
            {
                return myCARuleData;
            }
        }

        #endregion

        # region Members

        private int myNumberOfStates;

        private ICARuleData myCARuleData;

        #endregion
    }
}
