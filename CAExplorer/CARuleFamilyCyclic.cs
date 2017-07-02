// programmed by Adrian Magdina in 2013
// in this file is the implementation of cyclic rule family.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAExplorerNamespace
{
    //imlementation of Cyclic CA Rule family
    public class CARuleFamilyCyclic : ICARuleFamily
    {
        #region Constructors

        private CARuleFamilyCyclic()
        {
        }

        public CARuleFamilyCyclic(ICARuleData caRuleDataIn)
        {
            myCARuleData = caRuleDataIn;
            myNumberOfStates = (int)caRuleDataIn.CountOfStates;
        }

        #endregion

        #region Methods
        
        //computing next cell state for current cell state and current cell neighborhood
        public int ComputeCellState(IList<NeighborhoodAreaItem> cellValuesIn, int currentCellStateIn)
        {
            int aCellsWithNextState = 0;
            int aNextState = 0;

            if ( currentCellStateIn == (CARuleData.CountOfStates - 1))
            {
                aNextState = 0;
            }
            else
            {
                aNextState = currentCellStateIn + 1;
            }
            
            //determining how many cells in neighborhood have next cell state
            foreach (NeighborhoodAreaItem aNAItem in cellValuesIn)
            {
                 if (aNAItem.state == aNextState)
                {
                    aCellsWithNextState++;
                 }
            }

            //if cells with next cell state are more than threshold than return computed next cell state as next cell state
            if (aCellsWithNextState >= CARuleData.Threshold)
            {
                return aNextState;
            }

            //if here, return current cell state as next cell state
            return currentCellStateIn;
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

        //obtaining CA rule family string from enum
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
                return CARuleFamilies.Cyclic;
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