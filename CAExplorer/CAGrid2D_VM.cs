// programmed by Adrian Magdina in 2013
// in this file is the implementation of viewmodel for CA Grid

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
using System.Collections.Specialized;

namespace CAExplorerNamespace
{
    //enum for defining how state color will be set - directly set or interpolated.
    public enum StateColorAssigningType
    {
        Direct,
        Interpolated
    };

    public delegate void NextIterationDelegate(object sender, EventArgs e);
    public delegate void CAGridPropertyChangedDelegate(object sender, EventArgs e);

    public class CAGrid2DVM : ViewModelBase, IDisposable
    {
        #region Constructors

        public CAGrid2DVM()
            : this(0, 0)
        {
            InitCA();
        }

        public CAGrid2DVM(int rowsIn, int columsIn)
        {
            //creating model for this viewmodel
            CreateCA2DModel(rowsIn, columsIn, null, null);

            InitCA();

            //adding all cells, the number is depending on column and row count
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Cells.Add(new CellVM(CAGrid2DModel.GetCellModel(x, y), x * CellGridWidth, y * CellGridHeight));
                }
            }
        }

        #endregion

        #region Methods

        //initialization of thic CA
        public void InitCA()
        {
            myCells = new ObservableCollection<CellVM>();

            myCATimer = new CATimer();

            PlaySelectedCACommand = new RelayCommand(new Action<object>(PlaySelectedCA));
            StopSelectedCACommand = new RelayCommand(new Action<object>(StopSelectedCA));
            StepBackwardSelectedCACommand = new RelayCommand(new Action<object>(StepBackwardSelectedCA));
            StepForwardSelectedCACommand = new RelayCommand(new Action<object>(StepForwardSelectedCA));

            myCATimer.CATimerTickEvent += CATimerTickEventHandler;

            myStateToColorCollection = new List<StateAndColor>();

            //creating the empty model - without knowing the values. 
            CreateCA2DModel(0, 0, null, null);
        }

        //creating all cells
        public void CreateCells(int rowsIn, int columnsIn, ICARuleFamily caRuleFamilyIn, ICAGridCellInitialization caGridCellInitializationIn)
        {
            //creating the empty model - with specific values.
            CreateCA2DModel(rowsIn, columnsIn, caRuleFamilyIn, caGridCellInitializationIn);

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Cells.Add(new CellVM(CAGrid2DModel.GetCellModel(x, y), x * CellGridWidth, y * CellGridHeight));
                }
            }

            InitCAColors();
        }

        //creating the model instance
        public void CreateCA2DModel(int rowsIn, int columnsIn, ICARuleFamily caRuleFamilyIn, ICAGridCellInitialization caGridCellInitializationIn)
        {
            myCAGrid2DModel = new CAGrid2DM(rowsIn, columnsIn, caRuleFamilyIn, caGridCellInitializationIn);

            //coupling the model with this viewmodel, viewmodel knows model, but model doesn't know viewmodel.
            myCAGrid2DModel.CurrentIterationChangedEvent += CurrentIterationChangedHandler;
            myCAGrid2DModel.MaximumIterationChangedEvent += MaximumIterationChangedHandler;
        }

        //resizing the whole grid.
        public void ResizeGrid()
        {
            if (Cells != null)
            {
                for (int x = 0; x < Columns; x++)
                {
                    for (int y = 0; y < Rows; y++)
                    {
                        Cells[y + x * Rows].X = x * CellGridWidth;
                        Cells[y + x * Rows].Y = y * CellGridHeight;
                    }
                }

                OnPropertyChanged("Width");
                OnPropertyChanged("Height");
            }
        }

        //redrawing the grid (if it was changed).
        public void RedrawGrid()
        {
            if (CAGridPropertyChangedEvent != null)
            {
                CAGridPropertyChangedEvent(this, null);
            }
        }

        //if timer ticked the iteration of this CA will be changed.
        public void CATimerTickEventHandler(object sender, EventArgs e)
        {
            //if this is existing iteration, then return the previously computed CA state
            if (CAGrid2DModel.MaximumIteration > CAGrid2DModel.CurrentIteration)
            {
                IterationNumber++;
            }
            else
            {
                //if this is completely new, not previously computed iteration, then add new iteration 
                myCAGrid2DModel.AddNextIteration();
            }
        }

        public void StartTimer()
        {
            myCATimer.StartTimer();
        }

        public void StopTimer()
        {
            myCATimer.StopTimer();
        }

        public void PlaySelectedCA(object messageIn)
        {
            myCATimer.StartTimer();
        }

        public void StopSelectedCA(object messageIn)
        {
            myCATimer.StopTimer();
        }

        public void StepBackwardSelectedCA(object messageIn)
        {
            myCATimer.StopTimer();

            if (CAGrid2DModel.CurrentIteration > 0)
            {
                IterationNumber--;
            }
        }

        //one step forward in this CA
        public void StepForwardSelectedCA(object messageIn)
        {
            myCATimer.StopTimer();
            if (CAGrid2DModel.MaximumIteration > CAGrid2DModel.CurrentIteration)
            {
                IterationNumber++;
            }
            else
            {
                myCAGrid2DModel.AddNextIteration();
            }
        }

        public void CurrentIterationChangedHandler(object sender, EventArgs e)
        {
            OnPropertyChanged("IterationNumber");

            RedrawGrid();
        }

        public void MaximumIterationChangedHandler(object sender, EventArgs e)
        {
            OnPropertyChanged("MaximumIteration");
        }

        //color for specific state
        public Color? GetFillColorForState(int stateIn)
        {
            Color? aColor = null;

            if (StateColorAssigning == StateColorAssigningType.Direct) //direct color assigning was chosen
            {
                foreach (StateAndColor aStateAndColor in myStateToColorCollection)
                {
                    if (stateIn == aStateAndColor.State)
                    {
                        aColor = aStateAndColor.StateColor;
                        break;
                    }
                }
            }
            else if (StateColorAssigning == StateColorAssigningType.Interpolated) //interpolation of state colors was chosen
            {
                aColor = ColorInterpolation.InterpolateColor(
                    SelectedStartInterpColor, SelectedEndInterpColor, stateIn, Constants.BaseCellStateForAllAvailableCAs, (int)CAGrid2DModel.NumberOfStates - 1
                    );
            }

            return aColor;
        }

        //initialization of colors
        private void InitCAColors()
        {
            if (myCAGrid2DModel.NumberOfStates != null)
            {
                myStateToColorCollection.Clear();

                for (int i = 0; i < myCAGrid2DModel.NumberOfStates; i++)
                {
                    myStateToColorCollection.Add( new StateAndColor(i, Colors.Olive));
                }
            }

            myStateItems = new List<int>();

            for (int i = 0; i < myCAGrid2DModel.NumberOfStates; i++)
            {
                myStateItems.Add(i);
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
                myCATimer.CATimerTickEvent -= CATimerTickEventHandler;
                myCAGrid2DModel.CurrentIterationChangedEvent -= CurrentIterationChangedHandler;
                myCAGrid2DModel.MaximumIterationChangedEvent -= MaximumIterationChangedHandler;

                foreach (CellVM aCellVM in Cells)
                {
                    aCellVM.Dispose();
                }
            }
        }

        #endregion

        #region Properties

        //current iteration number
        public int IterationNumber
        {
            get
            {
                return myCAGrid2DModel.CurrentIteration;
            }
            set
            {
                myCAGrid2DModel.CurrentIteration = value;
            }
        }

        //maximum iteration number
        public int MaximumIteration
        {
            get
            {
                return myCAGrid2DModel.MaximumIteration;
            }
        }

        public ICommand PlaySelectedCACommand
        {
            get
            {
                return myPlaySelectedCACommand;
            }
            set
            {
                myPlaySelectedCACommand = value;
            }
        }

        public ICommand StopSelectedCACommand
        {
            get
            {
                return myStopSelectedCACommand;
            }
            set
            {
                myStopSelectedCACommand = value;
            }
        }

        public ICommand StepBackwardSelectedCACommand
        {
            get
            {
                return myStepBackwardSelectedCACommand;
            }
            set
            {
                myStepBackwardSelectedCACommand = value;
            }
        }

        public ICommand StepForwardSelectedCACommand
        {
            get
            {
                return myStepForwardSelectedCACommand;
            }
            set
            {
                myStepForwardSelectedCACommand = value;
            }
        }

        //collection of all cell viewmodels for this CA
        public ObservableCollection<CellVM> Cells
        {
            get
            {
                return myCells;
            }
        }

        //thickness of grid
        public LineThickness GridThickness
        {
            get
            {
                return myGridThickness;
            }
            set
            {
                myGridThickness = value;

                ResizeGrid();

                OnPropertyChanged("GridThickness");

                RedrawGrid();
            }
        }

        //thickness of selection frame
        public LineThickness SelFrameThickness
        {
            get
            {
                return mySelFrameThickness;
            }
            set
            {
                mySelFrameThickness = value;

                ResizeGrid();

                OnPropertyChanged("SelFrameThickness");

                RedrawGrid();
            }
        }

        //method how the CA cells will be initialized.
        public CAGridInitializationMethodTypes CAGridInitializationMethodType
        {
            get
            {
                return myCAGrid2DModel.CAGridInitializationMethodType;
            }
            set
            {
                myCAGrid2DModel.CAGridInitializationMethodType = value;
                OnPropertyChanged("CAGridInitializationMethodType");
            }
        }

        //name of this CA
        public String CAName
        {
            get
            {
                return myCAName;
            }
            set
            {
                myCAName = value;
                OnPropertyChanged("CAName");
            }
        }

        //model to this viewmodel
        public CAGrid2DM CAGrid2DModel
        {
            get
            {
                return myCAGrid2DModel;
            }
        }

        public int Width
        {
            get
            {
                return Columns*CellGridWidth;
            }
        }

        public int Height
        {
            get
            {
                return Rows*CellGridHeight;
            }
        }

        public int Rows
        {
            get
            {
                return myCAGrid2DModel.Rows;
            }
        }

        public int Columns
        {
            get
            {
                return myCAGrid2DModel.Columns;
            }
        }

        //the cell which is currently selected
        public CellVM SelectedCellViewModel
        {
            get
            {
                return mySelectedCellViewModel;
            }
            set
            {
                mySelectedCellViewModel = value;
                OnPropertyChanged("SelectedCellViewModel");
                OnPropertyChanged("NewStateManyValuesEnabled");
                OnPropertyChanged("NewStateFewValuesEnabled");
                OnPropertyChanged("IsCellSelectedEnabled");
                OnPropertyChanged("CommitNewStateEnabled");
            }
        }

        public Color SelectedGridColor
        {
            get
            {
                return mySelectedGridColor;
            }
            set
            {
                mySelectedGridColor = value;
                OnPropertyChanged("SelectedGridColor");

                RedrawGrid();
            }
        }

        public Color SelectedSelectionFrameColor
        {
            get
            {
                return mySelectedSelectionFrameColor;
            }
            set
            {
                mySelectedSelectionFrameColor = value;
                OnPropertyChanged("SelectedSelectionFrameColor");

                RedrawGrid();
            }
        }

        public Color SelectedMarkingColor
        {
            get
            {
                return mySelectedMarkingColor;
            }
            set
            {
                mySelectedMarkingColor = value;
                OnPropertyChanged("SelectedMarkingColor");

                RedrawGrid();
            }
        }

        public Color SelectedMouseOverColor
        {
            get
            {
                return mySelectedMouseOverColor;
            }
            set
            {
                mySelectedMouseOverColor = value;
                OnPropertyChanged("SelectedMouseOverColor");

                RedrawGrid();
            }
        }

        public Color SelectedBackgroundColor
        {
            get
            {
                return mySelectedBackgroundColor;
            }
            set
            {
                mySelectedBackgroundColor = value;
                OnPropertyChanged("SelectedBackgroundColor");

                RedrawGrid();
            }
        }

        //here is the background image if some was chosen.
        public ImageBrush CABackgroundImage
        {
            get
            {
                return myCABackgroundImage;
            }
            set
            {
                myCABackgroundImage = value;
                OnPropertyChanged("CABackgroundImage");
            }
        }

        //collection of colors for each state.
        public IList<StateAndColor> StateToColorCollection
        {
            get
            {
                return myStateToColorCollection;
            }
        }

        //starting interpolation color
        public Color SelectedStartInterpColor
        {
            get
            {
                return mySelectedStartInterpColor;
            }
            set
            {
                mySelectedStartInterpColor = value;

                RedrawGrid();
            }
        }

        //ending interpolation color
        public Color SelectedEndInterpColor
        {
            get
            {
                return mySelectedEndInterpColor;
            }
            set
            {
                mySelectedEndInterpColor = value;

                RedrawGrid();
            }
        }

        //type of assigning the state colors - either directly assigning or interpolating
        public StateColorAssigningType StateColorAssigning
        {
            get
            {
                return myStateColorAssigning;
            }
            set
            {
                myStateColorAssigning = value;

                RedrawGrid();
            }
        }

        //the possible states, which will be shown in combobox (which is used for setting of new state value).
        public IList<int> StateItems
        {
            get
            {
                return myStateItems;
            }
        }

        //the textbox for setting the new text value is enabled.
        public bool NewStateManyValuesEnabled
        {
            get
            {
                if (SelectedCellViewModel == null)
                {
                    return false;
                }
                
                if (CAGrid2DModel.NumberOfStates < 8)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        //the combobox for setting the new text value is enabled.
        public bool NewStateFewValuesEnabled
        {
            get
            {
                if (SelectedCellViewModel == null)
                {
                    return false;
                }

                if (CAGrid2DModel.NumberOfStates < 8)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        
        //is the checkbox for marking cells enabled
        public bool IsCellSelectedEnabled
        {
            get
            {
                if (SelectedCellViewModel == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        //is it enabled to commit the new state to cell.
        public bool CommitNewStateEnabled
        {
            get
            {
                if (SelectedCellViewModel == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        //speed of CA iteration change.
        public int TimerSpeed
        {
            get
            {
                return myCATimer.TimerSpeed;
            }
            set
            {
                myCATimer.TimerSpeed = value;
                OnPropertyChanged("TimerSpeed");
                OnPropertyChanged("SlowChecked");
                OnPropertyChanged("MediumChecked");
                OnPropertyChanged("FastChecked");
            }
        }

        public bool SlowChecked
        {
            get
            {
                return TimerSpeed == Constants.TimerSpeedSlow;
            }
            set
            {
                if (value == true)
                {
                    TimerSpeed = Constants.TimerSpeedSlow;
                    OnPropertyChanged("SlowChecked");
                    OnPropertyChanged("MediumChecked");
                    OnPropertyChanged("FastChecked");
                }
            }
        }

        public bool MediumChecked
        {
            get
            {
                return TimerSpeed == Constants.TimerSpeedMedium;
            }
            set
            {
                if (value == true)
                {
                    TimerSpeed = Constants.TimerSpeedMedium;
                    OnPropertyChanged("SlowChecked");
                    OnPropertyChanged("MediumChecked");
                    OnPropertyChanged("FastChecked");
                }
            }
        }

        public bool FastChecked
        {
            get
            {
                return TimerSpeed == Constants.TimerSpeedFast;
            }
            set
            {
                if (value == true)
                {
                    TimerSpeed = Constants.TimerSpeedFast;
                    OnPropertyChanged("SlowChecked");
                    OnPropertyChanged("MediumChecked");
                    OnPropertyChanged("FastChecked");
                }
            }
        }

        //width of cell interior
        public int CellObjectWidth
        {
            get
            {
                return myCellObjectWidth;
            }
            set
            {
                myCellObjectWidth = value;

                ResizeGrid();

                OnPropertyChanged("CellObjectWidth");
                OnPropertyChanged("Width");

                RedrawGrid();
            }
        }

        //height of cell interior
        public int CellObjectHeight
        {
            get
            {
                return myCellObjectHeight;
            }
            set
            {
                myCellObjectHeight = value;

                ResizeGrid();

                OnPropertyChanged("CellObjectHeight");
                OnPropertyChanged("Height");

                RedrawGrid();
            }
        }

        //width of cell - interior + selection frame + width
        public int CellGridWidth
        {
            get
            {
                int aCellGridWidth = 0;

                int aSelFrameThicknessValue = 0;
                ConstantLists.LineThicknessValue.TryGetValue(SelFrameThickness, out aSelFrameThicknessValue);
                aCellGridWidth += 2 * aSelFrameThicknessValue;

                int aGridThicknessValue = 0;
                ConstantLists.LineThicknessValue.TryGetValue(GridThickness, out aGridThicknessValue);
                aCellGridWidth += 2 * aGridThicknessValue;

                aCellGridWidth += CellObjectWidth;
                return aCellGridWidth;
            }
        }

        //height of cell - interior + selection frame + width
        public int CellGridHeight
        {
            get
            {
                int aCellGridHeight = 0;

                int aSelFrameThicknessValue = 0;
                ConstantLists.LineThicknessValue.TryGetValue(SelFrameThickness, out aSelFrameThicknessValue);
                aCellGridHeight += 2 * aSelFrameThicknessValue;

                int aGridThicknessValue = 0;
                ConstantLists.LineThicknessValue.TryGetValue(GridThickness, out aGridThicknessValue);
                aCellGridHeight += 2 * aGridThicknessValue;

                aCellGridHeight += CellObjectHeight;
                return aCellGridHeight;
            }
        }

        #endregion

        #region Members

        public event CAGridPropertyChangedDelegate CAGridPropertyChangedEvent;

        private ObservableCollection<CellVM> myCells = null;

        private LineThickness myGridThickness;
        private LineThickness mySelFrameThickness;

        private String myCAName = null;

        private CAGrid2DM myCAGrid2DModel = null;

        private ICommand myPlaySelectedCACommand;
        private ICommand myStopSelectedCACommand;
        private ICommand myStepBackwardSelectedCACommand;
        private ICommand myStepForwardSelectedCACommand;

        private CellVM mySelectedCellViewModel = null;

        private Color mySelectedGridColor;
        private Color mySelectedSelectionFrameColor;
        private Color mySelectedMarkingColor;
        private Color mySelectedMouseOverColor;
        private Color mySelectedBackgroundColor;

        private ImageBrush myCABackgroundImage = null;

        private IList<StateAndColor> myStateToColorCollection = null;

        private Color mySelectedStartInterpColor;
        private Color mySelectedEndInterpColor;

        private StateColorAssigningType myStateColorAssigning;

        private IList<int> myStateItems = null;

        private int myCellObjectWidth = 0;
        private int myCellObjectHeight = 0;

        private CATimer myCATimer = null;

        #endregion
    }
}
