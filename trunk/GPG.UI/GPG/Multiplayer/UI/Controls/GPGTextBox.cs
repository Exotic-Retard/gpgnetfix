namespace GPG.Multiplayer.UI.Controls
{
    using DevExpress.UserSkins;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using GPG;
    using GPG.Logging;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGTextBox : TextEdit
    {
        private bool mFirstTime = true;
        private static DevExControlLocalizer sLocalizer = new DevExControlLocalizer();

        public static  event EventHandler OnStyleControl;

        public GPGTextBox()
        {
            base.Height = 20;
            base.Properties.Appearance.BackColor = Color.Black;
            base.Properties.Appearance.BorderColor = Color.FromArgb(0x52, 0x83, 190);
            base.Properties.Appearance.ForeColor = Color.White;
            base.Properties.Appearance.Options.UseBackColor = true;
            base.Properties.Appearance.Options.UseBorderColor = true;
            base.Properties.Appearance.Options.UseForeColor = true;
            base.Properties.AppearanceFocused.BackColor = Color.FromArgb(0x10, 0x21, 0x4f);
            base.Properties.AppearanceFocused.BackColor2 = Color.FromArgb(0, 0, 0);
            base.Properties.AppearanceFocused.BorderColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            base.Properties.AppearanceFocused.GradientMode = LinearGradientMode.ForwardDiagonal;
            base.Properties.AppearanceFocused.Options.UseBackColor = true;
            base.Properties.AppearanceFocused.Options.UseBorderColor = true;
            base.Properties.BorderStyle = BorderStyles.Simple;
            base.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            Localizer.Active = sLocalizer;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                if (!e.Control)
                {
                    base.OnKeyDown(e);
                }
                else if ((this.Text != null) && (base.SelectionLength < 1))
                {
                    try
                    {
                        int length = this.Text.Length;
                        int selectionStart = base.SelectionStart;
                        this.Text = TextUtil.DeleteWord(this.Text, base.SelectionStart);
                        base.SelectionStart = selectionStart - (length - this.Text.Length);
                        e.SuppressKeyPress = true;
                    }
                    catch (Exception exception)
                    {
                        ErrorLog.WriteLine(exception);
                        base.OnKeyDown(e);
                    }
                }
            }
            else
            {
                base.OnKeyDown(e);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                if (!e.Control)
                {
                    base.OnKeyUp(e);
                }
            }
            else
            {
                base.OnKeyUp(e);
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            base.ShowCaret();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.mFirstTime)
            {
                this.mFirstTime = false;
                if (OnStyleControl != null)
                {
                    OnStyleControl(this, EventArgs.Empty);
                }
                else
                {
                    this.LookAndFeel.UseDefaultLookAndFeel = false;
                    BonusSkins.Register();
                    this.LookAndFeel.SkinName = "London Liquid Sky";
                }
            }
            base.OnPaint(e);
            if (!base.Enabled)
            {
                Brush brush = new SolidBrush(Color.FromArgb(0x80, 0, 0, 0));
                e.Graphics.FillRectangle(brush, base.ClientRectangle);
                brush.Dispose();
            }
        }
    }
}

