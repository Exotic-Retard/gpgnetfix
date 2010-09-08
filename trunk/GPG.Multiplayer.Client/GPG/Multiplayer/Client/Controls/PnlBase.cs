namespace GPG.Multiplayer.Client.Controls
{
    using GPG;
    using GPG.Multiplayer.Client;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Windows.Forms;

    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public class PnlBase : UserControl
    {
        private IContainer components = null;
        private ErrorProvider mErrors = new ErrorProvider();
        public ToolTip ttDefault;

        public PnlBase()
        {
            this.InitializeComponent();
        }

        private void _Localize(Control root)
        {
            if ((root.Text != null) && (root.Text.Length > 0))
            {
                root.Text = Loc.Get(root.Text);
            }
            if (root is ILocalizable)
            {
                (root as ILocalizable).Localize();
            }
            string toolTip = this.ttDefault.GetToolTip(root);
            if ((toolTip != null) && (toolTip.Length > 0))
            {
                this.ttDefault.SetToolTip(root, Loc.Get(toolTip));
            }
            foreach (Control control in root.Controls)
            {
                if (control != null)
                {
                    this._Localize(control);
                }
            }
        }

        protected internal void ClearErrors()
        {
            this.Errors.Clear();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected internal void Error(Control[] controls, string error, params object[] args)
        {
            for (int i = 0; i < controls.Length; i++)
            {
                this.Error(controls[i], error, args);
            }
        }

        protected internal void Error(Control control, string error, params object[] args)
        {
            VGen0 method = null;
            if ((base.InvokeRequired && !base.Disposing) && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate {
                        this.Errors.SetError(control, string.Format(Loc.Get(error), args));
                    };
                }
                base.BeginInvoke(method);
            }
            else if (!(base.Disposing || base.IsDisposed))
            {
                this.Errors.SetError(control, string.Format(Loc.Get(error), args));
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.ttDefault = new ToolTip(this.components);
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Name = "PnlBase";
            base.ResumeLayout(false);
        }

        protected virtual void Localize()
        {
            if (!base.DesignMode)
            {
                this._Localize(this);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Localize();
        }

        protected ErrorProvider Errors
        {
            get
            {
                return this.mErrors;
            }
        }

        protected FrmMain MainForm
        {
            get
            {
                return Program.MainForm;
            }
        }
    }
}

