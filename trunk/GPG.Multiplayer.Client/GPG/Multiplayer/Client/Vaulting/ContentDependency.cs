namespace GPG.Multiplayer.Client.Vaulting
{
    using System;

    internal class ContentDependency
    {
        public string Description;
        public int ID;

        public ContentDependency(IAdditionalContent content)
        {
            this.Description = content.Name;
            this.ID = content.ID;
        }

        public override string ToString()
        {
            return this.Description;
        }
    }
}

