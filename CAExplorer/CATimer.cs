// programmed by Adrian Magdina in 2013
// in this file is the implementation of wrapper for DispatcherTimer.

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
using System.Windows.Threading;

namespace CAExplorerNamespace
{
    public delegate void CATimerTickDelegate(object sender, EventArgs e);

    public class CATimer
    {
        public CATimer()
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            myTimerSpeed = Constants.TimerSpeedInitializationValue;
        }

        public void StartTimer()
        {
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, myTimerSpeed);
            dispatcherTimer.Start();
        }

        public void StopTimer()
        {
            dispatcherTimer.Stop();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (CATimerTickEvent != null)
            {
                CATimerTickEvent(this, null);
            }
        }

        public int TimerSpeed
        {
            get
            {
                return myTimerSpeed;
            }
            set
            {
                myTimerSpeed = value;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, myTimerSpeed);
            }
        }

        public event CATimerTickDelegate CATimerTickEvent;
        private DispatcherTimer dispatcherTimer = null;
        private int myTimerSpeed = 0;
    }
}

