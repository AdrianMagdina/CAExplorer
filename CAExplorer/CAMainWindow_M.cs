// programmed by Adrian Magdina in 2013
// in this file is the implementation of model for Main Window data.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAExplorerNamespace
{
    //class with implementation of model for main window.
    public class CAMainWindowM
    {
        #region Constructors

        public CAMainWindowM()
        {
            myCAGrid2DModelList = new List<CAGrid2DM>();

            myListOfCARules = new List<ICARuleData>();
        }

        #endregion

        #region Methods

        //load all available CA Rules in all available families
        public void LoadAvailableCARules()
        {
            string aFilename = @".\CARules.xml";

            CARulesRW.ReadCARules(aFilename, ref myListOfCARules);
        }

        #endregion

        #region Properties

        //list of CAGrid2D models that are currently added.
        public IList<CAGrid2DM> CAGrid2DModelList
        {
            get
            {
                return myCAGrid2DModelList;
            }
        }

        //list of all CA rules that were loaded.
        public IList<ICARuleData> ListOfCARules
        {
            get
            {
                return myListOfCARules;
            }
        }

        #endregion

        #region Members

        private IList<ICARuleData> myListOfCARules = null;
        private IList<CAGrid2DM> myCAGrid2DModelList = null;

        #endregion

    }
}
