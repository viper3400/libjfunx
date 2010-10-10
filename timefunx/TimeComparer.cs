using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace libjfunx.timefunx
{
    public class TimeComparer
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="timeToCompare">Zu vergleichende Zeit</param>
        public TimeComparer(DateTime timeToCompare)
        {
            this.timeToCompare = timeToCompare;
            this.compareTimer.Enabled = true;
            this.compareTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnCompareTimerElapsed); 
        }

        //*************************************************************************
        // Private Variablen
        //*************************************************************************

        private DateTime timeToCompare;
        private System.Timers.Timer compareTimer = new System.Timers.Timer(1000);

        //*************************************************************************
        // Eventhandling
        //*************************************************************************

        private void OnCompareTimerElapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            if (TimeHasCome() == true) { RaiseOnTime(); }
        }
        //************************************************************************
        // Public Events
        //************************************************************************

        public event EventHandler OnTime;


        //*************************************************************************
        // private Methoden
        //*************************************************************************

        private bool TimeHasCome()
        {
            DateTime now = DateTime.Now;
            //TimeSpan maxSpan = new TimeSpan(0, 0, 0);
            //TimeSpan actualSpan = this.timeToCompare - now;

            if (now.ToLongTimeString() == this.timeToCompare.ToLongTimeString())
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Raises the OnTime Event
        /// </summary>
        private void RaiseOnTime()
        {
            if (this.OnTime != null)
                this.OnTime(this, new EventArgs());
        }
    }
}
