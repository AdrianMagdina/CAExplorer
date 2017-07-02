// programmed by Adrian Magdina in 2013
// in this file is the implementation of class for storing data for specific rule.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAExplorerNamespace
{
    //in this class is the data needed for functioning of specific rule, most rules don't need all data members.
    public class CARuleData : ICARuleData
    {
        #region Constructors

        public CARuleData()
        {
        }

        #endregion

        #region Properties

        public IList<int> Survival
        {
            get
            {
                return mySurvival;
            }
            set
            {
                mySurvival = value;
            }
        }

        public IList<int> Birth
        {
            get
            {
                return myBirth;
            }
            set
            {
                myBirth = value;
            }
        }

        public int? CountOfStates
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

        public int? Threshold
        {
            get
            {
                return myThreshold;
            }
            set
            {
                myThreshold = value;
            }
        }

        public CARuleFamilies CARuleFamily
        {
            get
            {
                return myCaRuleFamily;
            }
            set
            {
                myCaRuleFamily = value;
            }
        }

        public string CARuleName
        {
            get
            {
                return myCARuleName;
            }
            set
            {
                myCARuleName = value;
            }
        }

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

        public int CANeighborhoodRange
        {
            get
            {
                return myCANeighborhoodRange;
            }
            set
            {
                myCANeighborhoodRange = value;
            }
        }

        #endregion

        #region Members

        private IList<int> mySurvival = null;
        private IList<int> myBirth = null;
        private int? myCountOfStates = null;

        private int? myThreshold = null; // for Cyclic CA

        private CARuleFamilies myCaRuleFamily;
        private string myCARuleName = null;

        private CANeighborhoodTypes myCANeighborhoodType;
        private int myCANeighborhoodRange = 0;

        #endregion
    }
}

