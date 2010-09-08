namespace GPG.UI
{
    using System;
    using System.Collections.Generic;

    public interface IFilterControl
    {
        Dictionary<string, bool> Filters { get; }

        bool IsFiltered { get; }
    }
}

