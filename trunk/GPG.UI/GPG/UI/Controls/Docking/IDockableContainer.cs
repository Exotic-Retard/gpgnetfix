namespace GPG.UI.Controls.Docking
{
    using System;
    using System.Windows.Forms;

    public interface IDockableContainer
    {
        void DockTo(IDockableContainer target, DockStyles orientation);
        void Undock();

        Control ContainerControl { get; set; }

        DockContainerForm ContainerForm { get; set; }

        DockStyles DockStyle { get; set; }
    }
}

