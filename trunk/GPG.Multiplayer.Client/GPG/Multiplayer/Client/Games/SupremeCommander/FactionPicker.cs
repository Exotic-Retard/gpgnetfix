namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using GPG.Multiplayer.Client.Controls;
    using System;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class FactionPicker : SkinDropDown
    {
        private bool mSearchMode = false;
        private SupcomLookups._Factions mSelectedFaction = SupcomLookups._Factions.Any;

        public event EventHandler SelectedFactionChanged;

        public FactionPicker()
        {
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            bool hasOriginal = true;
            bool hasExpansion = true;
            SupcomPrefs.TestFactions(out hasOriginal, out hasExpansion);
            SkinMenuItem item = new SkinMenuItem("<LOC>Random", SupcomFactions.random);
            item.Tag = SupcomLookups._Factions.Any;
            item.Name = item.Tag.ToString();
            item.Click += new EventHandler(this.SelectionChanged);
            base.Menu.MenuItems.Add(item);
            if (hasOriginal)
            {
                SkinMenuItem item2 = new SkinMenuItem("Aeon", SupcomFactions.aeon);
                item2.Tag = SupcomLookups._Factions.Aeon;
                item2.Name = item2.Tag.ToString();
                item2.Click += new EventHandler(this.SelectionChanged);
                SkinMenuItem item3 = new SkinMenuItem("Cybran", SupcomFactions.cybran);
                item3.Tag = SupcomLookups._Factions.Cybran;
                item3.Name = item3.Tag.ToString();
                item3.Click += new EventHandler(this.SelectionChanged);
                SkinMenuItem item4 = new SkinMenuItem("UEF", SupcomFactions.uef);
                item4.Tag = SupcomLookups._Factions.UEF;
                item4.Name = item4.Tag.ToString();
                item4.Click += new EventHandler(this.SelectionChanged);
                base.Menu.MenuItems.Add(item2);
                base.Menu.MenuItems.Add(item3);
                base.Menu.MenuItems.Add(item4);
            }
            if (hasExpansion)
            {
                SkinMenuItem item5 = new SkinMenuItem("Seraphim", SupcomFactions.seraphim);
                item5.Tag = SupcomLookups._Factions.Seraphim;
                item5.Name = item5.Tag.ToString();
                item5.Click += new EventHandler(this.SelectionChanged);
                base.Menu.MenuItems.Add(item5);
            }
            this.ForeColor = Color.WhiteSmoke;
            this.SelectItem(item);
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            this.SelectItem(sender as SkinMenuItem);
        }

        public void SelectItem(SkinMenuItem item)
        {
            this.mSelectedFaction = (SupcomLookups._Factions) item.Tag;
            this.Text = item.Text;
            base.Icon = item.Icon;
            this.Refresh();
            if (this.SelectedFactionChanged != null)
            {
                this.SelectedFactionChanged(this, EventArgs.Empty);
            }
        }

        public void SelectItem(SupcomLookups._Factions faction)
        {
            foreach (SkinMenuItem item in base.Menu.MenuItems)
            {
                if (((SupcomLookups._Factions) item.Tag) == faction)
                {
                    this.SelectItem(item);
                    break;
                }
            }
        }

        public override bool AutoStyle
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public bool SearchMode
        {
            get
            {
                return this.mSearchMode;
            }
            set
            {
                this.mSearchMode = value;
            }
        }

        public SupcomLookups._Factions SelectedFaction
        {
            get
            {
                return this.mSelectedFaction;
            }
        }
    }
}

