namespace GPG
{
    using System;

    public class OptionsRootAttribute : Attribute
    {
        private string mDescription;

        public OptionsRootAttribute(string description)
        {
            this.mDescription = Loc.Get(description);
        }

        public string Description
        {
            get
            {
                return this.mDescription;
            }
        }
    }
}

