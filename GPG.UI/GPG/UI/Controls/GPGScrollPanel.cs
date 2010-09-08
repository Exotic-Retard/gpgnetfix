namespace GPG.UI.Controls
{
    using GPG;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class GPGScrollPanel : GPGPanel
    {
        private IWdnProcControl mChildControl;

        public GPGScrollPanel()
        {
            this.AutoScroll = true;
            this.BackColor = Color.Black;
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
        }

        public IWdnProcControl ChildControl
        {
            get
            {
                return this.mChildControl;
            }
            set
            {
                VGen1 gen = null;
                this.mChildControl = value;
                if (value != null)
                {
                    if (gen == null)
                    {
                        gen = delegate (object msg) {
                            if (msg is Message)
                            {
                                Message message2 = (Message) msg;
                                if (message2.Msg == 0x20a)
                                {
                                    Message m = (Message) msg;
                                    this.WndProc(ref m);
                                }
                            }
                        };
                    }
                    this.mChildControl.OnWndProc += gen;
                }
            }
        }
    }
}

