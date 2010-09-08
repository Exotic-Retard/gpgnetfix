namespace GPG.UI.Controls
{
    using DevExpress.LookAndFeel;
    using DevExpress.XtraEditors;
    using System;
    using System.Drawing;

    public class GPGComboBoxPanel : PopupContainerControl
    {
        public GPGComboBoxPanel()
        {
            this.BackColor = Color.Black;
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.Style = LookAndFeelStyle.Flat;
            this.ForeColor = Color.White;
        }
    }
}

