namespace GPG.Multiplayer.UI.Controls
{
    using DevExpress.UserSkins;
    using DevExpress.XtraEditors;
    using System;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGButton : SimpleButton
    {
        private bool mFirstTime = true;
        private bool mUseVisualStyleBackColor;

        public static  event EventHandler OnStyleControl;

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
        }

        public bool UseVisualStyleBackColor
        {
            get
            {
                return this.mUseVisualStyleBackColor;
            }
            set
            {
                this.mUseVisualStyleBackColor = value;
            }
        }
    }
}

