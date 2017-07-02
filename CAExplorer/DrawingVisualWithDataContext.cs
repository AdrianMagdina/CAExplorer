// programmed by Adrian Magdina in 2013
// in this file is the implementation of drawing visual extended with Data context.

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
    //implementation of DrawingVisualWithDataContext - enhances the DrawingVisual with DataContext member.
    public class DrawingVisualWithDataContext : DrawingVisual
    {
        public DrawingVisualWithDataContext()
            : base()
        {
        }

        public virtual ViewModelBase DataContext
        {
            get
            {
                return myDataContext;
            }
            set
            {
                myDataContext = value;
            }
        }

        private ViewModelBase myDataContext = null;
    }
}
