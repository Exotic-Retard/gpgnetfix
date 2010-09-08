namespace GPG.UI
{
    using System;

    public interface ITextVal<valType>
    {
        string Text { get; }

        valType Value { get; }
    }
}

