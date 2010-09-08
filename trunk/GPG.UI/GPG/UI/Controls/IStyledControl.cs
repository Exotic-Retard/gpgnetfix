namespace GPG.UI.Controls
{
    using System;

    public interface IStyledControl
    {
        bool AutoStyle { get; set; }

        bool IsStyled { get; set; }
    }
}

