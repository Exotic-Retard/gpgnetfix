namespace GPG
{
    using System;
    using System.Timers;
    using System.Windows.Forms;

    public class StateTimer : System.Timers.Timer
    {
        private Form _ParentForm;
        private object _State;

        public StateTimer()
        {
        }

        public StateTimer(double msInterval) : base(msInterval)
        {
        }

        public Form ParentForm
        {
            get
            {
                return this._ParentForm;
            }
            set
            {
                this._ParentForm = value;
            }
        }

        public object State
        {
            get
            {
                return this._State;
            }
            set
            {
                this._State = value;
            }
        }
    }
}

