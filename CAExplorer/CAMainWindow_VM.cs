// programmed by Adrian Magdina in 2013
// in this file is the implementation of viewmodel for Main Window.

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
using System.Globalization;

namespace CAExplorerNamespace
{
    public class CAMainWindowVM : ViewModelBase, IDisposable
    {
        #region Constructors

        public CAMainWindowVM()
        {
            myGrid2DViewModelList = new ObservableCollection<CAGrid2DVM>();

            myCAMainWindowModel = new CAMainWindowM();

            CreateNewCACommand = new RelayCommand(new Action<object>(CreateAndAddNewCA));
            ModifySelectedCACommand = new RelayCommand(new Action<object>(ModifySelectedCA));
            MoveSelectedCAUpCommand = new RelayCommand(new Action<object>(MoveSelectedCAUp));
            MoveSelectedCADownCommand = new RelayCommand(new Action<object>(MoveSelectedCADown));
            DeleteSelectedCACommand = new RelayCommand(new Action<object>(DeleteSelectedCA));
            ApplicationSettingsCommand = new RelayCommand(new Action<object>(ShowAppSettings));
            ShowViewHelpCommand = new RelayCommand(new Action<object>(ShowViewHelp));
            ShowAboutCAExplorerCommand = new RelayCommand(new Action<object>(ShowAboutCAExplorer));
            SetCAColorsInSelectedCACommand = new RelayCommand(new Action<object>(SetCAColorsInSelectedCA));
            LoadCAFromFileCommand = new RelayCommand(new Action<object>(LoadCAFromFile));
            SaveCAToFileCommand = new RelayCommand(new Action<object>(SaveCAToFile));
            LoadBackgroundFromFileCommand = new RelayCommand(new Action<object>(LoadBackgroundImageFromFile));

            myDefaultStateColorsCollection = new List<StateAndColor>();
        }

        #endregion

        #region Methods

        public void LoadData()
        {
            //loading of all CA Rules
            myCAMainWindowModel.LoadAvailableCARules();

            //loading of application configuration (default colors are loaded here).
            LoadApplicationSettings();

            //loading of saves CAs
            LoadAllAvailableCAs(); 
        }

        //creating and adding new CA
        public void CreateAndAddNewCA(object messageIn)
        {
            if (mySelectedCAGrid2DViewModel != null)
            {
                mySelectedCAGrid2DViewModel.StopTimer();
            }

            if (myGrid2DViewModelList.Count < Constants.MaxNrOfCellularAutomatons)
            {
                try
                {
                    //instantiating Properties viewmodel for configuration of new CA.
                    var aCAPropertiesDialogVM = new CAPropertiesDialogVM(myCAMainWindowModel.ListOfCARules, myGrid2DViewModelList);

                    bool? aDialogResult = null;

                    var aCAGrid2DViewModel = new CAGrid2DVM();

                    aCAPropertiesDialogVM.ReadOnlyCAProperty = false;

                    aCAPropertiesDialogVM.CAColumns = Constants.MinCAColumns;
                    aCAPropertiesDialogVM.CARows = Constants.MinCARows;
                    aCAPropertiesDialogVM.CACellSizeX = Constants.MinCellSizeX;
                    aCAPropertiesDialogVM.CACellSizeY = Constants.MinCellSizeY;

                    bool aCANameWasFound = true;

                    int i = 0;

                    string aNewCAName = null;

                    //finding new default name of new CA, format is CA + increasing number
                    do
                    {
                        aCANameWasFound = true;

                        foreach (CAGrid2DVM aCAGrid2DVM in myGrid2DViewModelList)
                        {
                            if (aCAGrid2DVM.CAName == "CA" + i.ToString(CultureInfo.CurrentCulture))
                            {
                                aCANameWasFound = false;
                                break;
                            }
                        }

                        if (aCANameWasFound == true)
                        {
                            aNewCAName = "CA" + i.ToString(CultureInfo.CurrentCulture);
                        }

                        i++;

                    } while (aCANameWasFound != true);

                    aCAPropertiesDialogVM.CAName = aNewCAName;

                    //showing Properties dialog for configuration of new CA.
                    aDialogResult = DialogMediator.ShowModalDialog("New CA Properties", aCAPropertiesDialogVM, this);

                    if (aDialogResult == true)
                    {
                        //copying the inputted data from properties viewmodel to CAGrid viewmodel.
                        CopyFromCAPropertiesVM_To_CAGrid2DVM(aCAPropertiesDialogVM, true, ref aCAGrid2DViewModel);

                        //adding new CA to list
                        myGrid2DViewModelList.Add(aCAGrid2DViewModel);

                        //setting newly added CA as selected
                        SelectedCAGrid2DViewModel = aCAGrid2DViewModel;
                    }
                }
                catch (Exception ex)
                {
                    DialogMediator.ShowMessageBox(null, "Exception during creating of CA", "Following exception occured : \n" + ex.Message, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    throw;
                }
            }
            else
            {
                DialogMediator.ShowMessageBox(this, "Add New CA", "There can be maximum " + Constants.MaxNrOfCellularAutomatons + " CAs loaded! \n" +
                                              "Delete some CA first, before creating new CA.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //add specific input CA to CA list.
        public void AddNewCA(CAGrid2DVM aCAGrid2DVM)
        {
            if (myGrid2DViewModelList.Count < Constants.MaxNrOfCellularAutomatons)
            {
                myGrid2DViewModelList.Add(aCAGrid2DVM);
            }
            else
            {
                DialogMediator.ShowMessageBox(this, "Add New CA", "There can be maximum " + Constants.MaxNrOfCellularAutomatons + " CAs loaded! \n" +
                                              "Delete some CA first, before creating new CA.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //creating new empty CA
        public CAGrid2DVM CreateNewEmptyCA()
        {
            var aCAGrid2DVM = new CAGrid2DVM();

            return aCAGrid2DVM;
        }

        //modifying the existing CA
        public void ModifySelectedCA(object messageIn)
        {
            if (mySelectedCAGrid2DViewModel != null)
            {
                try
                {
                    //stopping last selected CA before continuing with modification
                    mySelectedCAGrid2DViewModel.StopTimer();

                    //instantiating Properties viewmodel for modification of CA.
                    var aCAPropertiesDialogVM = new CAPropertiesDialogVM(myCAMainWindowModel.ListOfCARules, myGrid2DViewModelList);

                    bool? aDialogResult = null;

                    aCAPropertiesDialogVM.ReadOnlyCAProperty = true;

                    //copying data from CA Grid viewmodel to Properties viewmodel.
                    CopyFromCAGrid2DVM_To_CAPropertiesVM(mySelectedCAGrid2DViewModel, ref aCAPropertiesDialogVM);

                    //showing Properties dialog
                    aDialogResult = DialogMediator.ShowModalDialog("Modify CA Properties", aCAPropertiesDialogVM, this);

                    if (aDialogResult == true)
                    {
                        //copying modified data from Properties viewmodel to CAGrid viewmodel.
                        CopyFromCAPropertiesVM_To_CAGrid2DVM(aCAPropertiesDialogVM, false, ref mySelectedCAGrid2DViewModel);

                        //redrawing CA after modification
                        mySelectedCAGrid2DViewModel.RedrawGrid();
                    }
                }
                catch (Exception ex)
                {
                    DialogMediator.ShowMessageBox(null, "Exception during modification of CA", "Following exception occured : \n" + ex.Message, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    throw;
                }
            }
        }

        //moving selected CA up in the list
        public void MoveSelectedCAUp(object messageIn)
        {
            for (int i = 0; i < myGrid2DViewModelList.Count; i++)
            {
                if (myGrid2DViewModelList[i] == mySelectedCAGrid2DViewModel)
                {
                    if (i > 0)
                    {
                        myGrid2DViewModelList.Move(i, i - 1);
                        break;
                    }
                }
            }
        }

        //moving selected CA down in the list.
        public void MoveSelectedCADown(object messageIn)
        {
            for (int i = 0; i < myGrid2DViewModelList.Count; i++)
            {
                if (myGrid2DViewModelList[i] == mySelectedCAGrid2DViewModel)
                {
                    if (i < (myGrid2DViewModelList.Count - 1))
                    {
                        myGrid2DViewModelList.Move(i, i + 1);
                        break;
                    }
                }
            }
        }

        //deleting selected CA from list and if file exists, then deleting file as well.
        public void DeleteSelectedCA(object messageIn)
        {
            if (mySelectedCAGrid2DViewModel != null)
            {
                mySelectedCAGrid2DViewModel.StopTimer();

                MessageBoxResult aMBR;

                aMBR = DialogMediator.ShowMessageBox(null, "Delete CA", "Do you really want to delete the selected CA", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (aMBR == MessageBoxResult.Yes)
                {
                    for (int i = 0; i < myGrid2DViewModelList.Count; i++)
                    {
                        if (myGrid2DViewModelList[i] == mySelectedCAGrid2DViewModel)
                        {
                            CAMainWindowRW.DeleteSpecificCA(mySelectedCAGrid2DViewModel);

                            mySelectedCAGrid2DViewModel.Dispose();

                            int aNewSelectedCAIndex = i + 1; // selected will be next CA after deleted

                            if (i == (myGrid2DViewModelList.Count - 1))
                            {
                                aNewSelectedCAIndex = (myGrid2DViewModelList.Count - 2);
                            }

                            if (i >= 1)
                            {
                                SelectedCAGrid2DViewModel = myGrid2DViewModelList[aNewSelectedCAIndex];
                            }
                            else
                            {
                                SelectedCAGrid2DViewModel = null;
                            }

                            myGrid2DViewModelList.RemoveAt(i);

                            break;
                        }
                    }
                }
            }
        }

        //setting state colors for specific CA
        public void SetCAColorsInSelectedCA(object messageIn)
        {
            if (mySelectedCAGrid2DViewModel != null)
            {
                try
                {
                    mySelectedCAGrid2DViewModel.StopTimer();

                    //instantiating of SetCAColors viewmodel
                    var aSetCAColorsVM = new SetCAColorsVM();

                    bool? aDialogResult = null;

                    //copying data from CAGrid viewmodel to SetCAColors viewmodel.
                    CopyFromCAGrid2DVM_To_SetCAColorsVM(mySelectedCAGrid2DViewModel, ref aSetCAColorsVM);

                    //showing of SetCAColors dialog
                    aDialogResult = DialogMediator.ShowModalDialog("Set CA colors", aSetCAColorsVM, this);

                    if (aDialogResult == true)
                    {
                        //copying data from SetCAColors viewmodel to CAGrid2D viewmodel
                        CopyFromSetCAColorsVM_To_CAGrid2DVM(aSetCAColorsVM, ref mySelectedCAGrid2DViewModel);

                        //redrawing CA after CA colors modification
                        mySelectedCAGrid2DViewModel.RedrawGrid();
                    }
                }
                catch (Exception ex)
                {
                    DialogMediator.ShowMessageBox(null, "Exception during showing Set State Colors dialog", "Following exception occured : \n" + ex.Message, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    throw;
                }
            }
        }

        //showing application settings dialog - default CA colors are defined here.
        public void ShowAppSettings(object messageIn)
        {
            try
            {
                if (mySelectedCAGrid2DViewModel != null)
                {
                    mySelectedCAGrid2DViewModel.StopTimer();
                }

                //instantiating of ApplicationSettings viewmodel
                var aAppSettingsVM = new AppSettingsVM();

                bool? aDialogResult = null;

                //copying data - colors from this viewmodel (CAMainWindow) to Application settings viewmodel
                aAppSettingsVM.SelectedDefaultBackgroundColor = this.SelectedDefaultBackgroundColor;
                aAppSettingsVM.SelectedDefaultEndInterpColor = this.SelectedDefaultEndInterpColor;
                aAppSettingsVM.SelectedDefaultGridColor = this.SelectedDefaultGridColor;
                aAppSettingsVM.SelectedDefaultMarkingColor = this.SelectedDefaultMarkingColor;
                aAppSettingsVM.SelectedDefaultMouseOverColor = this.SelectedDefaultMouseOverColor;
                aAppSettingsVM.SelectedDefaultSelectionFrameColor = this.SelectedDefaultSelectionFrameColor;
                aAppSettingsVM.SelectedDefaultStartInterpColor = this.SelectedDefaultStartInterpColor;

                aAppSettingsVM.DefaultStateColorsCollection.Clear();
                foreach (StateAndColor aStateAndColorItem in this.DefaultStateColorsCollection)
                {
                    aAppSettingsVM.DefaultStateColorsCollection.Add(new StateAndColor(aStateAndColorItem));
                }

                //showing Application settings viewmodel.
                aDialogResult = DialogMediator.ShowModalDialog("Application Settings", aAppSettingsVM, this);

                if (aDialogResult == true)
                {
                    //copying data - colors from Application settings viewmodel to this (CAMainWindow) viewmodel.
                    this.SelectedDefaultBackgroundColor = aAppSettingsVM.SelectedDefaultBackgroundColor;
                    this.SelectedDefaultEndInterpColor = aAppSettingsVM.SelectedDefaultEndInterpColor;
                    this.SelectedDefaultGridColor = aAppSettingsVM.SelectedDefaultGridColor;
                    this.SelectedDefaultMarkingColor = aAppSettingsVM.SelectedDefaultMarkingColor;
                    this.SelectedDefaultMouseOverColor = aAppSettingsVM.SelectedDefaultMouseOverColor;
                    this.SelectedDefaultSelectionFrameColor = aAppSettingsVM.SelectedDefaultSelectionFrameColor;
                    this.SelectedDefaultStartInterpColor = aAppSettingsVM.SelectedDefaultStartInterpColor;

                    this.DefaultStateColorsCollection.Clear();
                    foreach (StateAndColor aStateAndColorItem in aAppSettingsVM.DefaultStateColorsCollection)
                    {
                        this.DefaultStateColorsCollection.Add(new StateAndColor(aStateAndColorItem));
                    }
                }
            }
            catch (Exception ex)
            {
                DialogMediator.ShowMessageBox(null, "Exception during showing Application setting dialog", "Following exception occured : \n" + ex.Message, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                throw;
            }
        }

        //showing modeless Help dialog
        public void ShowViewHelp(object messageIn)
        {
            if (mySelectedCAGrid2DViewModel != null)
            {
                mySelectedCAGrid2DViewModel.StopTimer();
            }

            var aViewHelpVM = new ViewHelpVM();

            DialogMediator.ShowModelessDialog("Help", aViewHelpVM, this);
        }

        //showing About dialog
        public void ShowAboutCAExplorer(object messageIn)
        {
            if (mySelectedCAGrid2DViewModel != null)
            {
                mySelectedCAGrid2DViewModel.StopTimer();
            }

            var aAboutCAExplorerVM = new AboutCAExplorerVM();

            DialogMediator.ShowModalDialog("About CA Explorer", aAboutCAExplorerVM, this);
        }

        //loading specific CA from file
        public void LoadCAFromFile(object messageIn)
        {
            bool? aDialogResult = null;

            string aDefaultExtension = ".xml";
            string aFileExtensions = "Xml files (.xml)|*.xml";

            string aFileName = null;
            aDialogResult = DialogMediator.ShowOpenFileDialog(this, aFileExtensions, aDefaultExtension, out aFileName);

            if (aDialogResult == true)
            {
                var aCAGrid2DVM = new CAGrid2DVM();

                try
                {
                    //reading CA from specified file and adding it to list of CAs
                    CAGrid2DRW.ReadCA(aFileName, this, ref aCAGrid2DVM);
                    myGrid2DViewModelList.Add(aCAGrid2DVM);
                }
                catch (Exception ex)
                {
                    if (ex is System.IO.FileNotFoundException || ex is System.Security.SecurityException || ex is UriFormatException || ex is System.Xml.XmlException
                        || ex is FormatException || ex is OverflowException || ex is ArgumentOutOfRangeException || ex is NullReferenceException || ex is CAExplorerException)
                    {
                        DialogMediator.ShowMessageBox(null, "Exception during reading CA", "Problem with reading following CA file : \n" + aFileName
                        + "\n" + "Following exception occured : \n" + ex.Message, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        //saving specific CA To file
        public void SaveCAToFile(object messageIn)
        {
            if (mySelectedCAGrid2DViewModel != null)
            {
                bool? aDialogResult = null;

                string aDefaultExtension = ".xml";
                string aFileExtensions = "Xml files (.xml)|*.xml";

                string aFileName = null;
                aDialogResult = DialogMediator.ShowSaveFileDialog(this, aFileExtensions, aDefaultExtension, out aFileName);

                if (aDialogResult == true)
                {
                    try
                    {
                        //writing specific CA to file.
                        CAGrid2DRW.WriteCA(aFileName, mySelectedCAGrid2DViewModel);
                    }
                    catch (Exception ex)
                    {
                        if (ex is InvalidOperationException || ex is EncoderFallbackException || ex is ArgumentException || ex is UnauthorizedAccessException
                             || ex is System.IO.DirectoryNotFoundException || ex is System.IO.IOException || ex is System.Security.SecurityException || ex is System.IO.PathTooLongException)
                        {
                            DialogMediator.ShowMessageBox(null, "Exception during writing of CA", "Problem with writing following CA file : \n" + aFileName
                                               + "\n" + "Following exception occured : \n" + ex.Message, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
        }

        //loading background image from file.
        public void LoadBackgroundImageFromFile(object messageIn)
        {
            if (mySelectedCAGrid2DViewModel != null)
            {
                bool? aDialogResult = null;

                string aDefaultExtension = ".jpg";
                string aFileExtensions = "Images jpeg files (.jpg)|*.jpg|png files (.png)|*.png|gif files (.gif)|*.gif|bmp files (.bmp)|*.bmp";

                string aFileName = null;
                aDialogResult = DialogMediator.ShowOpenFileDialog(this, aFileExtensions, aDefaultExtension, out aFileName);

                if (aDialogResult == true)
                {
                    try
                    {
                        ImageBrush aCABackgroundImageBrush = new ImageBrush();

                        aCABackgroundImageBrush.ImageSource = new BitmapImage(new Uri(@aFileName, UriKind.Relative));
                        mySelectedCAGrid2DViewModel.CABackgroundImage = aCABackgroundImageBrush;
                    }
                    catch (Exception ex)
                    {
                        if (ex is OutOfMemoryException || ex is System.IO.FileNotFoundException || ex is ArgumentException || ex is UriFormatException)
                        {
                            DialogMediator.ShowMessageBox(null, "Exception during reading of background image", "Problem with reading of background image : \n" + aFileName
                                               + "\n" + "Following exception occured : \n" + ex.Message, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
        }

        //loading application settings - default colors are defined there
        public void LoadApplicationSettings()
        {
            string aPath = @".\CAExplorerApplicationSettings.xml";
            CAMainWindowRW.ReadCAMainWindowData(aPath, this);
        }

        //loading of all CAs
        public void LoadAllAvailableCAs()
        {
            CAMainWindowRW.ReadAllAvailableCAs(this);
        }

        //saving of all CAs
        public void SaveApplicationSettings()
        {
            string aPath = @".\CAExplorerApplicationSettings.xml";
            CAMainWindowRW.WriteCAMainWindowData(aPath, this);
            CAMainWindowRW.WriteAllAvailableCAs(this);
        }

        //copying data from CAGrid viewmodel to CAProperties viewmodel
        private void CopyFromCAGrid2DVM_To_CAPropertiesVM(CAGrid2DVM caGrid2DVMIn, ref CAPropertiesDialogVM caPropertiesVM)
        {
            caPropertiesVM.CAName = caGrid2DVMIn.CAName;
            caPropertiesVM.CACellSizeX = (int)caGrid2DVMIn.CellObjectWidth;
            caPropertiesVM.CACellSizeY = (int)caGrid2DVMIn.CellObjectHeight;
            caPropertiesVM.CARows = caGrid2DVMIn.Rows;
            caPropertiesVM.CAColumns = caGrid2DVMIn.Columns;

            foreach (ComboBoxItem aGridThickness in ConstantLists.CAGridThicknessItems)
            {
                if (caGrid2DVMIn.GridThickness == (LineThickness)aGridThickness.ComboBoxId)
                {
                    caPropertiesVM.CAGridThicknessSelectedItem = aGridThickness.ComboBoxString;
                    break;
                }
            }

            foreach (ComboBoxItem aGridThickness in ConstantLists.CAGridThicknessItems)
            {
                if (caGrid2DVMIn.GridThickness == (LineThickness)aGridThickness.ComboBoxId)
                {
                    caPropertiesVM.CAGridThicknessSelectedItem = aGridThickness.ComboBoxString;
                    break;
                }
            }

            foreach (ComboBoxItem aSelFrameThickness in ConstantLists.CASelFrameThicknessItems)
            {
                if (caGrid2DVMIn.SelFrameThickness == (LineThickness)aSelFrameThickness.ComboBoxId)
                {
                    caPropertiesVM.CASelFrameThicknessSelectedItem = aSelFrameThickness.ComboBoxString;
                    break;
                }
            }

            foreach (ComboBoxItem aCAGridInitializationMethodItem in ConstantLists.CAInitializationMethodItems)
            {
                if (caGrid2DVMIn.CAGridInitializationMethodType == (CAGridInitializationMethodTypes)aCAGridInitializationMethodItem.ComboBoxId)
                {
                    caPropertiesVM.CAGridCellInitializationSelectedItem = aCAGridInitializationMethodItem.ComboBoxString;
                    break;
                }
            }

            ICARuleFamily aCARuleFamily = caGrid2DVMIn.CAGrid2DModel.CARule;
            caPropertiesVM.CARuleFamilySelectedItem = aCARuleFamily.CARuleFamilyString;
            caPropertiesVM.CARuleSelectedItem = aCARuleFamily.CARuleData.CARuleName;

            caPropertiesVM.OriginalCAName = caGrid2DVMIn.CAName;

            caPropertiesVM.ReadOnlyCAProperty = true;
        }

        //copying data from CAProperties viewmodel to CAGrid viewmodel.
        private void CopyFromCAPropertiesVM_To_CAGrid2DVM(CAPropertiesDialogVM caPropertiesVM, bool shallCreateIn, ref CAGrid2DVM caGrid2DVMIn)
        {
            CARuleFamilies? aCARuleFamilyEnum = null;

            if (shallCreateIn == true)
            {
                foreach (ComboBoxItem aCARuleFamilyEnumItem in ConstantLists.CARuleFamilyItems)
                {
                    if (aCARuleFamilyEnumItem.ComboBoxString == caPropertiesVM.CARuleFamilySelectedItem)
                    {
                        aCARuleFamilyEnum = (CARuleFamilies)aCARuleFamilyEnumItem.ComboBoxId;
                        break;
                    }
                }

                ICARuleData aCARuleData = null;

                foreach (ICARuleData aCARuleDataItem in CAMainWindowModel.ListOfCARules)
                {
                    if (aCARuleDataItem.CARuleFamily == aCARuleFamilyEnum && aCARuleDataItem.CARuleName == caPropertiesVM.CARuleSelectedItem)
                    {
                        aCARuleData = aCARuleDataItem;
                        break;
                    }
                }

                ICARuleFamily aCARuleFamily = CARuleFactory.CreateCARuleFamily((CARuleFamilies)aCARuleFamilyEnum, aCARuleData);

                CAGridInitializationMethodTypes? aCAGridInitializationMethodEnum = null;
                foreach (ComboBoxItem aCAGridInitializationMethodItem in ConstantLists.CAInitializationMethodItems)
                {
                    if (aCAGridInitializationMethodItem.ComboBoxString == caPropertiesVM.CAGridCellInitializationSelectedItem)
                    {
                        aCAGridInitializationMethodEnum = (CAGridInitializationMethodTypes)aCAGridInitializationMethodItem.ComboBoxId;
                        break;
                    }
                }

                ICAGridCellInitialization aCAGridCellInitialization = CAGridCellInitializationFactory.CreateCAGridCellInitialization((CAGridInitializationMethodTypes)aCAGridInitializationMethodEnum);

                caGrid2DVMIn.CreateCells(caPropertiesVM.CAColumns, caPropertiesVM.CARows, aCARuleFamily, aCAGridCellInitialization);

                caGrid2DVMIn.SelectedBackgroundColor = this.SelectedDefaultBackgroundColor;
                caGrid2DVMIn.SelectedGridColor = this.SelectedDefaultGridColor;
                caGrid2DVMIn.SelectedMarkingColor = this.SelectedDefaultMarkingColor;
                caGrid2DVMIn.SelectedMouseOverColor = this.SelectedDefaultMouseOverColor;
                caGrid2DVMIn.SelectedSelectionFrameColor = this.SelectedDefaultSelectionFrameColor;
                caGrid2DVMIn.SelectedStartInterpColor = this.SelectedDefaultStartInterpColor;
                caGrid2DVMIn.SelectedEndInterpColor = this.SelectedDefaultEndInterpColor;

                caGrid2DVMIn.StateToColorCollection.Clear();
                foreach (StateAndColor aStateAndColorItem in this.DefaultStateColorsCollection)
                {
                    caGrid2DVMIn.StateToColorCollection.Add(new StateAndColor(aStateAndColorItem));
                }

                caGrid2DVMIn.StateColorAssigning = caGrid2DVMIn.CAGrid2DModel.NumberOfStates < Constants.MaxColorCountForDirectColors
                    ? StateColorAssigningType.Direct : StateColorAssigningType.Interpolated;
            }

            caGrid2DVMIn.CellObjectWidth = caPropertiesVM.CACellSizeX;
            caGrid2DVMIn.CellObjectHeight = caPropertiesVM.CACellSizeY;

            caGrid2DVMIn.CAName = caPropertiesVM.CAName;

            foreach (ComboBoxItem aGridThickness in ConstantLists.CAGridThicknessItems)
            {
                if (caPropertiesVM.CAGridThicknessSelectedItem == aGridThickness.ComboBoxString)
                {
                    caGrid2DVMIn.GridThickness = (LineThickness)aGridThickness.ComboBoxId;
                    break;
                }
            }

            foreach (ComboBoxItem aSelFrameThickness in ConstantLists.CASelFrameThicknessItems)
            {
                if (caPropertiesVM.CASelFrameThicknessSelectedItem == aSelFrameThickness.ComboBoxString)
                {
                    caGrid2DVMIn.SelFrameThickness = (LineThickness)aSelFrameThickness.ComboBoxId;
                    break;
                }
            }
        }

        //copying data from CAGrid viewmodel to SetCAColors viewmodel.
        private void CopyFromCAGrid2DVM_To_SetCAColorsVM(CAGrid2DVM caGrid2DVMIn, ref SetCAColorsVM setCAColorsVM)
        {
            setCAColorsVM.SelectedGridColor = caGrid2DVMIn.SelectedGridColor;
            setCAColorsVM.SelectedMarkingColor = caGrid2DVMIn.SelectedMarkingColor;
            setCAColorsVM.SelectedMouseOverColor = caGrid2DVMIn.SelectedMouseOverColor;
            setCAColorsVM.SelectedSelectionFrameColor = caGrid2DVMIn.SelectedSelectionFrameColor;
            setCAColorsVM.SelectedBackgroundColor = caGrid2DVMIn.SelectedBackgroundColor;
            setCAColorsVM.SelectedStartInterpColor = caGrid2DVMIn.SelectedStartInterpColor;
            setCAColorsVM.SelectedEndInterpColor = caGrid2DVMIn.SelectedEndInterpColor;

            setCAColorsVM.SetStateColorsDirectlyRBEnabled = caGrid2DVMIn.CAGrid2DModel.NumberOfStates < Constants.MaxColorCountForDirectColors ? true : false;

            if (caGrid2DVMIn.StateColorAssigning == StateColorAssigningType.Direct)
            {
                setCAColorsVM.SetStateColorsDirectlyChecked = true;
                setCAColorsVM.SetStateColorsInterpChecked = false;
            }
            else if (caGrid2DVMIn.StateColorAssigning == StateColorAssigningType.Interpolated)
            {
                setCAColorsVM.SetStateColorsInterpChecked = true;
                setCAColorsVM.SetStateColorsDirectlyChecked = false;
            }

            if (setCAColorsVM.SetStateColorsDirectlyRBEnabled == false)
            {
                setCAColorsVM.SetStateColorsDirectlyChecked = false;
                setCAColorsVM.SetStateColorsInterpChecked = true;
            }

            setCAColorsVM.StateCount = (int)caGrid2DVMIn.CAGrid2DModel.NumberOfStates;

            setCAColorsVM.StateColorsCollection.Clear(); 
            if (setCAColorsVM.SetStateColorsDirectlyRBEnabled == true)
            {

                for (int i = 0; i < caGrid2DVMIn.CAGrid2DModel.NumberOfStates; i++)
                {
                    setCAColorsVM.StateColorsCollection.Add(new StateAndColor(caGrid2DVMIn.StateToColorCollection[i]));
                }
            }
        }
        
        //copying data from SetCAColors viewmodel to CAGrid viewmodel
        private void CopyFromSetCAColorsVM_To_CAGrid2DVM(SetCAColorsVM setCAColorsVM, ref CAGrid2DVM caGrid2DVMIn)
        {
            caGrid2DVMIn.SelectedGridColor = setCAColorsVM.SelectedGridColor;
            caGrid2DVMIn.SelectedMarkingColor = setCAColorsVM.SelectedMarkingColor;
            caGrid2DVMIn.SelectedMouseOverColor = setCAColorsVM.SelectedMouseOverColor;
            caGrid2DVMIn.SelectedSelectionFrameColor = setCAColorsVM.SelectedSelectionFrameColor;
            caGrid2DVMIn.SelectedBackgroundColor = setCAColorsVM.SelectedBackgroundColor;
            caGrid2DVMIn.SelectedStartInterpColor = setCAColorsVM.SelectedStartInterpColor;
            caGrid2DVMIn.SelectedEndInterpColor = setCAColorsVM.SelectedEndInterpColor;

            if (setCAColorsVM.SetStateColorsDirectlyChecked)
            {
                caGrid2DVMIn.StateColorAssigning = StateColorAssigningType.Direct;
            }
            else if (setCAColorsVM.SetStateColorsInterpChecked)
            {
                caGrid2DVMIn.StateColorAssigning = StateColorAssigningType.Interpolated;
            }
            else
            {
                throw new CAExplorerException("Unknown state color assigning was used!");
            }

            caGrid2DVMIn.StateToColorCollection.Clear();
            foreach (StateAndColor aStateAndColor in setCAColorsVM.StateColorsCollection)
            {
                caGrid2DVMIn.StateToColorCollection.Add(new StateAndColor(aStateAndColor));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposingIn)
        {
            if (disposingIn)
            {
                foreach (CAGrid2DVM aCAGrid2DVM in myGrid2DViewModelList)
                {
                    aCAGrid2DVM.Dispose();
                }
            }
        }

        #endregion

        #region Properties

        public ICommand CreateNewCACommand
        {
            get
            {
                return myCreateNewCACommand;
            }
            set
            {
                myCreateNewCACommand = value;
            }
        }

        public ICommand ModifySelectedCACommand
        {
            get
            {
                return myModifySelectedCACommand;
            }
            set
            {
                myModifySelectedCACommand = value;
            }
        }

        public ICommand MoveSelectedCAUpCommand
        {
            get
            {
                return myMoveSelectedCAUpCommand;
            }
            set
            {
                myMoveSelectedCAUpCommand = value;
            }
        }

        public ICommand MoveSelectedCADownCommand
        {
            get
            {
                return myMoveSelectedCADownCommand;
            }
            set
            {
                myMoveSelectedCADownCommand = value;
            }
        }

        public ICommand DeleteSelectedCACommand
        {
            get
            {
                return myDeleteSelectedCACommand;
            }
            set
            {
                myDeleteSelectedCACommand = value;
            }
        }

        public ICommand SetCAColorsInSelectedCACommand
        {
            get
            {
                return mySetCAColorsInSelectedCACommand;
            }
            set
            {
                mySetCAColorsInSelectedCACommand = value;
            }
        }

        public ICommand ApplicationSettingsCommand
        {
            get
            {
                return myApplicationSettingsCommand;
            }
            set
            {
                myApplicationSettingsCommand = value;
            }
        }

        public ICommand ShowViewHelpCommand
        {
            get
            {
                return myShowViewHelpCommand;
            }
            set
            {
                myShowViewHelpCommand = value;
            }
        }

        public ICommand ShowAboutCAExplorerCommand
        {
            get
            {
                return myShowAboutCAExplorerCommand;
            }
            set
            {
                myShowAboutCAExplorerCommand = value;
            }
        }

        public ICommand LoadBackgroundFromFileCommand
        {
            get
            {
                return myLoadBackgroundFromFileCommand;
            }
            set
            {
                myLoadBackgroundFromFileCommand = value;
            }
        }

        public ICommand LoadCAFromFileCommand
        {
            get
            {
                return myLoadCAFromFileCommand;
            }
            set
            {
                myLoadCAFromFileCommand = value;
            }
        }

        public ICommand SaveCAToFileCommand
        {
            get
            {
                return mySaveCAToFileCommand;
            }
            set
            {
                mySaveCAToFileCommand = value;
            }
        }

        //currently selected CA
        public CAGrid2DVM SelectedCAGrid2DViewModel
        {
            get
            {
                return mySelectedCAGrid2DViewModel;
            }
            set
            {
                mySelectedCAGrid2DViewModel = value;

                if (mySelectedCAGrid2DViewModel != null)
                {
                    mySelectedCAGrid2DViewModel.StopTimer();
                }

                OnPropertyChanged("SelectedCAGrid2DViewModel");
                OnPropertyChanged("IsCAGrid2DViewModelSelected");
                OnPropertyChanged("IsGenerationChangeEnabled");
                OnPropertyChanged("StatusBarDescriptionString");
            }
        }

        //list of all available CAs
        public ObservableCollection<CAGrid2DVM> Grid2DViewModelList
        {
            get
            {
                return myGrid2DViewModelList;
            }
        }

        //is currently some CA selected
        public bool IsCAGrid2DViewModelSelected
        {
            get
            {
                return SelectedCAGrid2DViewModel == null ? false : true;
            }
        }

        public bool IsGenerationChangeEnabled
        {
            get
            {
                return SelectedCAGrid2DViewModel == null ? false : true;
            }
        }

        //model to this viewmodel
        public CAMainWindowM CAMainWindowModel
        {
            get
            {
                return myCAMainWindowModel;
            }
        }

        //default color - will be used in newly created CA
        public Color SelectedDefaultGridColor
        {
            get
            {
                return mySelectedDefaultGridColor;
            }
            set
            {
                mySelectedDefaultGridColor = value;
                OnPropertyChanged("SelectedDefaultGridColor");
            }
        }

        //default color - will be used in newly created CA
        public Color SelectedDefaultSelectionFrameColor
        {
            get
            {
                return mySelectedDefaultSelectionFrameColor;
            }
            set
            {
                mySelectedDefaultSelectionFrameColor = value;
                OnPropertyChanged("SelectedDefaultSelectionFrameColor");
            }
        }

        //default color - will be used in newly created CA
        public Color SelectedDefaultMarkingColor
        {
            get
            {
                return mySelectedDefaultMarkingColor;
            }
            set
            {
                mySelectedDefaultMarkingColor = value;
                OnPropertyChanged("SelectedDefaultMarkingColor");
            }
        }

        //default color - will be used in newly created CA
        public Color SelectedDefaultMouseOverColor
        {
            get
            {
                return mySelectedDefaultMouseOverColor;
            }
            set
            {
                mySelectedDefaultMouseOverColor = value;
                OnPropertyChanged("SelectedDefaultMouseOverColor");
            }
        }

        //default color - will be used in newly created CA
        public Color SelectedDefaultBackgroundColor
        {
            get
            {
                return mySelectedDefaultBackgroundColor;
            }
            set
            {
                mySelectedDefaultBackgroundColor = value;
                OnPropertyChanged("SelectedDefaultBackgroundColor");
            }
        }

        //default color - will be used in newly created CA
        public Color SelectedDefaultStartInterpColor
        {
            get
            {
                return mySelectedDefaultStartInterpColor;
            }
            set
            {
                mySelectedDefaultStartInterpColor = value;
                OnPropertyChanged("SelectedDefaultStartInterpColor");
            }
        }

        //default color - will be used in newly created CA
        public Color SelectedDefaultEndInterpColor
        {
            get
            {
                return mySelectedDefaultEndInterpColor;
            }
            set
            {
                mySelectedDefaultEndInterpColor = value;
                OnPropertyChanged("SelectedDefaultEndInterpColor");
            }
        }

        //default color - will be used in newly created CA
        public IList<StateAndColor> DefaultStateColorsCollection
        {
            get
            {
                return myDefaultStateColorsCollection;
            }
        }

        //text written main window status bar
        public string StatusBarDescriptionString
        {
            get
            {
                if (SelectedCAGrid2DViewModel != null)
                {
                    ICARuleFamily aCARuleFamily = SelectedCAGrid2DViewModel.CAGrid2DModel.CARule;
                    if (aCARuleFamily != null)
                    {
                        string aCARuleFamilyString = aCARuleFamily.CARuleFamilyString;

                        if (aCARuleFamily.CARuleData == null)
                        {
                            throw new CAExplorerException("There is a CA selected, but there is no CA rule data available!");
                        }

                        string aCARuleString = aCARuleFamily.CARuleData.CARuleName;

                        return "Name: " + SelectedCAGrid2DViewModel.CAName + "   Rule Family: " + aCARuleFamilyString + "   Rule Name: " + aCARuleString;
                    }
                }
                
                return "No CA is selected";
            }
        }

        #endregion

        #region Members

        private ICommand myCreateNewCACommand;
        private ICommand myModifySelectedCACommand;
        private ICommand myMoveSelectedCAUpCommand;
        private ICommand myMoveSelectedCADownCommand;
        private ICommand myDeleteSelectedCACommand;
        private ICommand mySetCAColorsInSelectedCACommand;

        private ICommand myApplicationSettingsCommand;
        private ICommand myShowViewHelpCommand;
        private ICommand myShowAboutCAExplorerCommand;

        private ICommand myLoadBackgroundFromFileCommand;
        private ICommand myLoadCAFromFileCommand;
        private ICommand mySaveCAToFileCommand;

        private ObservableCollection<CAGrid2DVM> myGrid2DViewModelList = null;
        private CAMainWindowM myCAMainWindowModel = null;
        private CAGrid2DVM mySelectedCAGrid2DViewModel = null;

        private Color mySelectedDefaultGridColor;
        private Color mySelectedDefaultSelectionFrameColor;
        private Color mySelectedDefaultMarkingColor;
        private Color mySelectedDefaultMouseOverColor;
        private Color mySelectedDefaultBackgroundColor;
        private Color mySelectedDefaultStartInterpColor;
        private Color mySelectedDefaultEndInterpColor;

        private IList<StateAndColor> myDefaultStateColorsCollection = null;

        #endregion
    }
}
