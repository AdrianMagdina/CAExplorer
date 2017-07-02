// programmed by Adrian Magdina in 2013
// in this file is the implementation of viewmodel for Properties dialog.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO;

namespace CAExplorerNamespace
{
    //viewmodel for CAProperties dialog
    public sealed class CAPropertiesDialogVM : ViewModelBase
    {
        #region Constructors

        private CAPropertiesDialogVM()
        {
        }

        public CAPropertiesDialogVM(IList<ICARuleData> listOfCARulesIn, IList<CAGrid2DVM> grid2DViewModelListIn)
        {
            myListOfCARules = listOfCARulesIn;

            myGrid2DViewModelList = grid2DViewModelListIn;

            ReadOnlyCAProperty = false;

            myCAGridThicknessItems = new ObservableCollection<string>();

            foreach (ComboBoxItem aGridThickness in ConstantLists.CAGridThicknessItems)
            {
                myCAGridThicknessItems.Add(aGridThickness.ComboBoxString);
            }

            myCASelFrameThicknessItems = new ObservableCollection<string>();

            foreach (ComboBoxItem aSelFrameThickness in ConstantLists.CASelFrameThicknessItems)
            {
                myCASelFrameThicknessItems.Add(aSelFrameThickness.ComboBoxString);
            }

            myCARuleFamilyItems = new ObservableCollection<string>();

            foreach (ComboBoxItem aCARuleFamilyItem in ConstantLists.CARuleFamilyItems)
            {
                myCARuleFamilyItems.Add(aCARuleFamilyItem.ComboBoxString);
            }

            if (myCAGridThicknessSelectedItem == null)
            {
                if (myCAGridThicknessItems.Count > 0)
                {
                    myCAGridThicknessSelectedItem = ConstantLists.FirstTimeCAGridThicknessItem;
                }
            }

            if (myCASelFrameThicknessSelectedItem == null)
            {
                if (myCASelFrameThicknessItems.Count > 0)
                {
                    myCASelFrameThicknessSelectedItem = ConstantLists.FirstTimeCASelFrameThicknessItem;
                }
            }

            if (myCARuleFamilyItems.Count > 0)
            {
                myCARuleFamilySelectedItem = ConstantLists.FirstTimeCARuleFamily;
            }

            CARuleFamilies? aCARuleFamilyEnum = null;

            foreach (ComboBoxItem aCARuleFamilyEnumItem in ConstantLists.CARuleFamilyItems)
            {
                if (aCARuleFamilyEnumItem.ComboBoxString == CARuleFamilySelectedItem)
                {
                    aCARuleFamilyEnum = (CARuleFamilies)aCARuleFamilyEnumItem.ComboBoxId;
                }
            }

            myCARuleItems = new ObservableCollection<string>();

            foreach (ICARuleData aCARuleDataItem in myListOfCARules)
            {
                if (aCARuleDataItem.CARuleFamily == aCARuleFamilyEnum)
                {
                    CARuleItems.Add(aCARuleDataItem.CARuleName);
                }
            }

            myCAGridCellInitializationItems = new ObservableCollection<string>();

            foreach (ComboBoxItem aCAInitializationItem in ConstantLists.CAInitializationMethodItems)
            {
                myCAGridCellInitializationItems.Add(aCAInitializationItem.ComboBoxString);
            }

            if (myCAGridCellInitializationSelectedItem == null)
            {
                if (myCAGridCellInitializationItems.Count > 0)
                {
                    myCAGridCellInitializationSelectedItem = ConstantLists.FirstTimeCAInitializationMethod;
                }
            }

            if (myCARuleItems.Count > 0)
            {
                CARuleSelectedItem = myCARuleItems[0];
            }
        }

        #endregion

        #region Methods

        //IDataErrorInfo member, fields are tested here if they meet all requirements
        public string this[string columnName]
        {
            get
            {
                string aResult = null;

                //CA Name is tested here.
                if (columnName == "CAName")
                {
                    bool aFileNameValid = true;

                    foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                    {
                        if (CAName.Contains(c))
                        {
                            aFileNameValid = false;
                        }
                    }

                    if (String.IsNullOrEmpty(CAName) || aFileNameValid == false)
                    {
                        aResult = "Name of CA must have length of at least one character, and must be a valid Filename!";
                    }
                    else
                    {
                        if (myGrid2DViewModelList != null)
                        {
                            bool aCANameWasFound = false;

                            foreach (CAGrid2DVM aCAGrid2DVM in myGrid2DViewModelList)
                            {
                                if ((CAName == aCAGrid2DVM.CAName) && (CAName != OriginalCAName))
                                {
                                    aCANameWasFound = true;
                                    break;
                                }
                            }

                            if (aCANameWasFound == true)
                            {
                                aResult = "It already exists a CA with selected name. Please choose different name!";
                            }
                        }
                    }
                }
                else if (columnName == "CARuleSelectedItem") //selected CA rule is tested here.
                {
                    if (CARuleSelectedItem == null)
                    {
                        aResult = "There must be a CA Rule selected!";
                    }
                }

                return aResult;
            }
        }

        #endregion

        #region Properties

        public int CARows
        {
            get
            {
                return myCARows;
            }
            set
            {
                myCARows = value;
                OnPropertyChanged("CARows");
            }
        }

        public int CAColumns
        {
            get
            {
                return myCAColumns;
            }
            set
            {
                myCAColumns = value;
                OnPropertyChanged("CAColumns");
            }
        }

        public int CACellSizeX
        {
            get
            {
                return myCACellSizeX;
            }
            set
            {
                myCACellSizeX = value;
                OnPropertyChanged("CACellSizeX");
            }
        }

        public int CACellSizeY
        {
            get
            {
                return myCACellSizeY;
            }
            set
            {
                myCACellSizeY = value;
                OnPropertyChanged("CACellSizeY");
            }
        }
        
        //selected CA grid thickness
        public string CAGridThicknessSelectedItem
        {
            get
            {
                return myCAGridThicknessSelectedItem;
            }
            set
            {
                myCAGridThicknessSelectedItem = value;
                OnPropertyChanged("CAGridThicknessSelectedItem");
            }
        }

        //selected CA selection frame thickness
        public string CASelFrameThicknessSelectedItem
        {
            get
            {
                return myCASelFrameThicknessSelectedItem;
            }
            set
            {
                myCASelFrameThicknessSelectedItem = value;
                OnPropertyChanged("CASelFrameThicknessSelectedItem");
            }
        }

        //CA rule family - if rule family changes rules must change as well - they must belong to new family
        public string CARuleFamilySelectedItem
        {
            get
            {
                return myCARuleFamilySelectedItem;
            }
            set
            {
                myCARuleFamilySelectedItem = value;

                CARuleFamilies? aCARuleFamilyEnum = null;

                foreach (ComboBoxItem aCARuleFamilyEnumItem in ConstantLists.CARuleFamilyItems)
                {
                    if (aCARuleFamilyEnumItem.ComboBoxString == CARuleFamilySelectedItem)
                    {
                        aCARuleFamilyEnum = (CARuleFamilies)aCARuleFamilyEnumItem.ComboBoxId;
                    }
                }

                CARuleItems.Clear();

                foreach (ICARuleData aCARuleDataItem in myListOfCARules)
                {
                    if (aCARuleDataItem.CARuleFamily == aCARuleFamilyEnum)
                    {
                        CARuleItems.Add(aCARuleDataItem.CARuleName);
                    }
                }

                if (myCARuleItems.Count > 0)
                {
                    CARuleSelectedItem = myCARuleItems[0];
                }

                OnPropertyChanged("CARuleFamilySelectedItem");
                OnPropertyChanged("CARuleSelectedItem");
                OnPropertyChanged("CARuleItems");
            }
        }

        //selected initialization method for CA
        public string CAGridCellInitializationSelectedItem
        {
            get
            {
                return myCAGridCellInitializationSelectedItem;
            }
            set
            {
                myCAGridCellInitializationSelectedItem = value;
                OnPropertyChanged("CAGridCellInitializationSelectedItem");
            }
        }

        //selected CA rule
        public string CARuleSelectedItem
        {
            get
            {
                return myCARuleSelectedItem;
            }
            set
            {
                myCARuleSelectedItem = value;
                OnPropertyChanged("CARuleSelectedItem");
            }
        }

        public ObservableCollection<string> CAGridThicknessItems
        {
            get
            {
                return myCAGridThicknessItems;
            }
        }

        public ObservableCollection<string> CASelFrameThicknessItems
        {
            get
            {
                return myCASelFrameThicknessItems;
            }
        }

        public ObservableCollection<string> CARuleFamilyItems
        {
            get
            {
                return myCARuleFamilyItems;
            }
        }

        public ObservableCollection<string> CARuleItems
        {
            get
            {
                return myCARuleItems;
            }
        }

        public ObservableCollection<string> CAGridCellInitializationItems
        {
            get
            {
                return myCAGridCellInitializationItems;
            }
        }

        public string CAName
        {
            get
            {
                return myCAName;
            }
            set
            {
                myCAName = value;
                OnPropertyChanged("CAColumns");
            }
        }

        public string OriginalCAName
        {
            get
            {
                return myOriginalCAName;
            }
            set
            {
                myOriginalCAName = value;
            }
        }

        public bool DialogResult
        {
            get
            {
                return myDialogResult;
            }
        }

        public bool ReadOnlyCAProperty
        {
            get
            {
                return myReadOnlyCAProperty;
            }
            set
            {
                myReadOnlyCAProperty = value;
                OnPropertyChanged("ReadOnlyCAProperty");
            }
        }

        public bool EnabledCAProperty
        {
            get
            {
                return !myReadOnlyCAProperty;
            }
        }

        //list of CAGrid viewmodels - needed to find out if the new CA name already exists
        public IList<CAGrid2DVM> Grid2DViewModelList
        {
            get
            {
                return myGrid2DViewModelList;
            }
        }

        #endregion

        #region Members

        private bool myDialogResult = false;

        private string myCAName;

        private int myCARows = 0;
        private int myCAColumns = 0;

        private int myCACellSizeX = 0;
        private int myCACellSizeY = 0;

        private string myCAGridThicknessSelectedItem = null;
        private string myCASelFrameThicknessSelectedItem = null;
        private string myCARuleFamilySelectedItem = null;
        private string myCARuleSelectedItem = null;
        private string myCAGridCellInitializationSelectedItem = null;

        private readonly ObservableCollection<string> myCAGridThicknessItems = null;
        private readonly ObservableCollection<string> myCASelFrameThicknessItems = null;
        private readonly ObservableCollection<string> myCARuleFamilyItems = null;
        private readonly ObservableCollection<string> myCARuleItems = null;
        private readonly ObservableCollection<string> myCAGridCellInitializationItems = null;

        private bool myReadOnlyCAProperty = false;

        private IList<CAGrid2DVM> myGrid2DViewModelList = null;

        private IList<ICARuleData> myListOfCARules = null;

        private string myOriginalCAName = null;

        #endregion
    }
}
