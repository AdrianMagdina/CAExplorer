// programmed by Adrian Magdina in 2013
// in this file is the implementation CAGrid - a control which draws the Cellular Automaton Grid

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CAExplorerNamespace
{
    //this is a class representing the CA Grid control
    public class CAGrid : FrameworkElement, IDisposable
    {
        #region Constructors

        public CAGrid()
        {
            myVisualChildren = new VisualCollection(this);
            DataContextChanged += DataContextChangedEventHandler;

            CreateCAGridCells();

            MouseMove += OnMouseMove;
            MouseDown += OnMouseDown;
        }

        #endregion

        #region Methods

        //during Refresh (if some parameter changed) it is needed to redraw the CA Grid to show the new data
        public void Refresh(object sender, EventArgs e)
        {
            ModifyCAGridCells();
        }

        //if cell state of some cell changed, it is needed to redraw the changed cell.
        public void OnCellStateChanged(Object sender, EventArgs e)
        {
            for (int i = 0; i < myVisualChildren.Count; i++)
            {
                DrawingVisualWithDataContext aDrawingVisualWithDataContext = myVisualChildren[i] as DrawingVisualWithDataContext;

                if (aDrawingVisualWithDataContext != null)
                {
                    CellVM aCellVM = aDrawingVisualWithDataContext.DataContext as CellVM;

                    if (aCellVM != null)
                    {
                        if (aCellVM.IsSelected == true)
                        {
                            CreateCAGridCellGeometry(aCellVM, ref aDrawingVisualWithDataContext, false);
                        }
                    }
                }
            }
        }

        //if mouse moves the cell over is the mouse pointer currently positioned is drawn here
        public void OnMouseMove(Object sender, MouseEventArgs e)
        {
            HitTestResult aHTR = VisualTreeHelper.HitTest(this, e.GetPosition(sender as IInputElement));
            DrawingVisualWithDataContext aDrawingVisualWithDataContext = aHTR.VisualHit as DrawingVisualWithDataContext;

            if (aDrawingVisualWithDataContext != null)
            {
                if (aDrawingVisualWithDataContext.DataContext != null)
                {
                    //here is the returning to former appereance of cell where mouse pointer was positioned before
                    if (myLastPointedDrawingVisual != null && myLastPointedDrawingVisual.DataContext != null)
                    {
                        CellVM aCellVM_LP = myLastPointedDrawingVisual.DataContext as CellVM;

                        if (aCellVM_LP != null)
                        {
                            CreateCAGridCellGeometry(aCellVM_LP, ref myLastPointedDrawingVisual, false);
                        }
                    }

                    CellVM aCellVM = aDrawingVisualWithDataContext.DataContext as CellVM;

                    if (aCellVM != null)
                    {
                        CreateCAGridCellGeometry(aCellVM, ref aDrawingVisualWithDataContext, true);

                        myLastPointedDrawingVisual = aDrawingVisualWithDataContext;
                    }
                }
            }
        }

        //if mouse button is pressed then current cell will be selected
        public void OnMouseDown(Object sender, MouseEventArgs e)
        {
            HitTestResult aHTR = VisualTreeHelper.HitTest(this, e.GetPosition(sender as IInputElement));
            DrawingVisualWithDataContext aDrawingVisualWithDataContext = aHTR.VisualHit as DrawingVisualWithDataContext;

            if (aDrawingVisualWithDataContext != null)
            {
                if (aDrawingVisualWithDataContext.DataContext != null)
                {
                    //here is the returning of before selected cell to normal state
                    if (myLastSelectedDrawingVisual != null && myLastSelectedDrawingVisual.DataContext != null)
                    {
                        CellVM aCellVM_LS = myLastSelectedDrawingVisual.DataContext as CellVM;

                        if (aCellVM_LS != null)
                        {
                            aCellVM_LS.IsSelected = false;
                            CreateCAGridCellGeometry(aCellVM_LS, ref myLastSelectedDrawingVisual, false);
                        }
                    }

                    CellVM aCellVM = aDrawingVisualWithDataContext.DataContext as CellVM;

                    if (aCellVM != null)
                    {
                        CAGrid2DVM aCAGrid2DVM = this.DataContext as CAGrid2DVM;

                        if (aCAGrid2DVM != null)
                        {
                            aCellVM.IsSelected = true;
                            CreateCAGridCellGeometry(aCellVM, ref aDrawingVisualWithDataContext, false);

                            aCAGrid2DVM.SelectedCellViewModel = aCellVM;
                            myLastSelectedDrawingVisual = aDrawingVisualWithDataContext;
                        }
                    }
                }
            }
        }

        //this method creates the cells
        private void CreateCAGridCells()
        {
            if (myVisualChildren != null)
            {
                myVisualChildren.Clear();
            }

            CAGrid2DVM aCAGrid2DVM = DataContext as CAGrid2DVM;
            if (aCAGrid2DVM != null)
            {
                //for every Cell view model one cell is created
                foreach (CellVM aCellVM in aCAGrid2DVM.Cells)
                {
                    DrawingVisualWithDataContext aDrawingVisualWithDataContext = CreateCell(aCellVM);

                    if (aDrawingVisualWithDataContext != null)
                    {
                        myVisualChildren.Add(aDrawingVisualWithDataContext);
                    }
                }
            }
        }

        //in this method the cells are modified if someting changed
        private void ModifyCAGridCells()
        {
            if ( myVisualChildren != null )
            {
                CAGrid2DVM aCAGrid2DVM = DataContext as CAGrid2DVM;
                if (aCAGrid2DVM != null)
                {
                    for (int i=0; i < myVisualChildren.Count; i++)
                    {
                        DrawingVisualWithDataContext aDVWithDC = myVisualChildren[i] as DrawingVisualWithDataContext;
                        if (aDVWithDC != null)
                        {
                            CellVM aCellVM = aDVWithDC.DataContext as CellVM;
                            if (aCellVM != null)
                            {
                                CreateCAGridCellGeometry(aCellVM, ref aDVWithDC, false);
                            }
                        }
                    }
                }
            }
        }

        //event handler on dependency property, if Cells in CAGrid2D viewmodel changed, then CAGrid will be recreated.
        public static void OnCellsChanged(DependencyObject doIn, DependencyPropertyChangedEventArgs argsIn)
        {
            CAGrid aCAGrid = doIn as CAGrid;
            if (aCAGrid != null)
            {
                aCAGrid.CreateCAGridCells();
            }
        }

        public void DataContextChangedEventHandler(Object sender, DependencyPropertyChangedEventArgs args)
        {
            CAGrid2DVM aOldCAGrid2DVM = args.OldValue as CAGrid2DVM;
            CAGrid2DVM aNewCAGrid2DVM = args.NewValue as CAGrid2DVM;

            if (aOldCAGrid2DVM != null)
            {
                aOldCAGrid2DVM.CAGridPropertyChangedEvent -= Refresh;
                foreach (CellVM aCellVM in aOldCAGrid2DVM.Cells)
                {
                    aCellVM.CellStateChangedEvent -= OnCellStateChanged;
                }
            }

            if (aNewCAGrid2DVM != null)
            {
                aNewCAGrid2DVM.CAGridPropertyChangedEvent += Refresh;
            }

            myLastPointedDrawingVisual = null;
            myLastSelectedDrawingVisual = null;

            CreateCAGridCells();
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < myVisualChildren.Count)
            {
                return myVisualChildren[index];
            }
            else
            {
                return null;
            }
        }

        //this method creates a specific cell
        private DrawingVisualWithDataContext CreateCell(CellVM cellVMIn)
        {
            var aDrawingVisualWithDataContext = new DrawingVisualWithDataContext();

            aDrawingVisualWithDataContext.DataContext = cellVMIn;

            cellVMIn.CellStateChangedEvent += OnCellStateChanged;

            CreateCAGridCellGeometry(cellVMIn, ref aDrawingVisualWithDataContext, false);

            return aDrawingVisualWithDataContext;
        }

        //this method draws the specific cell
        private void CreateCAGridCellGeometry(CellVM cellVMIn, ref DrawingVisualWithDataContext drawingVisualWithDataContextIn, bool isMouseOverIn)
        {
            CAGrid2DVM aCAGrid2DVM = this.DataContext as CAGrid2DVM;

            if (aCAGrid2DVM != null)
            {
                DrawingContext dc = drawingVisualWithDataContextIn.RenderOpen();

                int aGridPenWidth = 0;
                ConstantLists.LineThicknessValue.TryGetValue(aCAGrid2DVM.GridThickness, out aGridPenWidth); // the currently selected grid thickness

                int aFramePenWidth = 0;
                ConstantLists.LineThicknessValue.TryGetValue(aCAGrid2DVM.SelFrameThickness, out aFramePenWidth); // the currently selected frame thickness

                //three brushes are drawn one after one, and that will create different areas of cell - grid, frame and content
                //Brush for grid
                Brush aGridBrush = new SolidColorBrush(aCAGrid2DVM.SelectedGridColor);
                Rect aGridRect = new Rect(cellVMIn.X, cellVMIn.Y, aCAGrid2DVM.CellGridWidth, aCAGrid2DVM.CellGridHeight);
                dc.DrawRectangle(aGridBrush, null, aGridRect);

                //Brush for frame
                Brush aFrameBrush = new SolidColorBrush((Color)GetFrameColor(cellVMIn, isMouseOverIn));
                Rect aFrameRect = new Rect(cellVMIn.X + aGridPenWidth, cellVMIn.Y + aGridPenWidth, aCAGrid2DVM.CellGridWidth - 2 * aGridPenWidth, aCAGrid2DVM.CellGridHeight - 2 * aGridPenWidth);
                dc.DrawRectangle(aFrameBrush, null, aFrameRect);

                Color? aStateColor = aCAGrid2DVM.GetFillColorForState(cellVMIn.State); // current color of this cell
                if (aStateColor == null)
                {
                    throw new CAExplorerException();
                }

                // brush for content
                Brush aContentBrush = new SolidColorBrush((Color)aStateColor);
                Rect aContentRect = new Rect(cellVMIn.X + aGridPenWidth + aFramePenWidth, cellVMIn.Y + aGridPenWidth + aFramePenWidth, aCAGrid2DVM.CellObjectWidth, aCAGrid2DVM.CellObjectHeight);
                dc.DrawRectangle(aContentBrush, null, aContentRect);

                aGridBrush.Freeze();
                aContentBrush.Freeze();
                aFrameBrush.Freeze();

                dc.Close();
            }
        }

        //this method gets the frame color of specific cell, it depends on if the cell is selected, marked, or if the mouse pointer is over cell.
        private Color? GetFrameColor(CellVM cellVMIn, bool isMouseOverIn)
        {
            CAGrid2DVM aCAGrid2DVM = this.DataContext as CAGrid2DVM;

            if (aCAGrid2DVM != null)
            {
                if (cellVMIn.IsSelected == true)
                {
                    return aCAGrid2DVM.SelectedSelectionFrameColor;
                }
                else if (cellVMIn.IsMarked)
                {
                    return aCAGrid2DVM.SelectedMarkingColor;
                }
                else if (isMouseOverIn == true)
                {
                    return aCAGrid2DVM.SelectedMouseOverColor;
                }
                else
                {
                    return aCAGrid2DVM.SelectedBackgroundColor;
                }
            }

            return null;
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
                DataContextChanged -= DataContextChangedEventHandler;

                MouseMove -= OnMouseMove;
                MouseDown -= OnMouseDown;
            }
        }

        #endregion

        #region Properties

        protected override int VisualChildrenCount
        {
            get
            {
                return myVisualChildren.Count;
            }
        }

        public ObservableCollection<CellVM> Cells
        {
            get
            {
                return (ObservableCollection<CellVM>)GetValue(CellsDependencyProperty);
            }
            set
            {
                SetValue(CellsDependencyProperty, value);
            }
        }

        #endregion

        #region Members

        //dependenency property signalising that cells were changed
        public static readonly DependencyProperty CellsDependencyProperty = DependencyProperty.Register("Cells", typeof(ObservableCollection<CellVM>),
                                                                                                        typeof(CAGrid), new FrameworkPropertyMetadata(OnCellsChanged));

        private VisualCollection myVisualChildren;

        private DrawingVisualWithDataContext myLastPointedDrawingVisual = null; //cell where the mouse pointer was last over
        private DrawingVisualWithDataContext myLastSelectedDrawingVisual = null; //cell where was the last selection

        #endregion
    }
}
