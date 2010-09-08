namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using DevExpress.LookAndFeel;
    using DevExpress.Utils;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Card;
    using GPG;
    using GPG.Multiplayer.Client;
    using GPG.Multiplayer.Client.Controls;
    using GPG.Multiplayer.Client.Games;
    using GPG.Multiplayer.Client.Games.SupremeCommander.icons;
    using GPG.Multiplayer.Client.Properties;
    using GPG.Multiplayer.Quazal;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class DlgSupcomMapOptions : DlgBase
    {
        private CardView cardView1;
        private GridColumn colData;
        private GridColumn colDescription;
        private GridColumn colMap;
        private GridColumn colMapTitle;
        private GridColumn colStatus;
        private GridColumn colTime;
        private GridColumn colType;
        private IContainer components;
        private GPGLabel gpgLabel1;
        public GPGChatGrid gpgMapSelectGrid;
        private ImageList ilThumbs;
        private GPGLabel lMapPrefs;
        private bool mAeonEnabled;
        private string mKind;
        private BindingList<SupcomMapInfo> mMaps;
        public RadioButton rbAeon;
        public RadioButton rbCybran;
        public RadioButton rbRandom;
        public RadioButton rbSeraphim;
        public RadioButton rbUEF;
        private RepositoryItemTextEdit repositoryItemTextEdit1;
        private RepositoryItemTimeEdit repositoryItemTimeEdit1;
        private RepositoryItemButtonEdit riButtonStatus;
        private RepositoryItemPictureEdit riMap;
        private RepositoryItemMemoEdit rimMemoEdit3;
        private RepositoryItemPictureEdit rimPictureEdit3;
        private RepositoryItemTextEdit rimTextEdit;
        private RepositoryItemPictureEdit riPictureStatus;
        private RepositoryItemPopupContainerEdit riPopup;
        private RepositoryItemImageComboBox riPriority;
        private RepositoryItemTextEdit riTextPriority;
        private SkinButton skinButtonCancel;
        private SkinButton skinButtonSearch;

        internal DlgSupcomMapOptions()
        {
            this.mMaps = new BindingList<SupcomMapInfo>();
            this.mKind = "1v1";
            this.components = null;
            this.InitializeComponent();
            this.gpgMapSelectGrid.DataSource = this.mMaps;
            bool hasOriginal = true;
            bool hasExpansion = true;
            SupcomPrefs.TestFactions(out hasOriginal, out hasExpansion);
            if (!hasOriginal)
            {
                this.rbCybran.Checked = false;
                this.rbCybran.Enabled = false;
                this.rbAeon.Checked = false;
                this.rbAeon.Enabled = false;
                this.rbUEF.Checked = false;
                this.rbUEF.Enabled = false;
                this.rbRandom.Checked = false;
                this.rbRandom.Enabled = false;
            }
            if (!hasExpansion)
            {
                this.rbSeraphim.Checked = false;
                this.rbSeraphim.Enabled = false;
            }
            this.lMapPrefs.Text = string.Format(Loc.Get("<LOC>Step 1: Select your map preferences -- you may have only {0} thumbs up and {0} thumbs down."), this.GetMaxCount);
        }

        public DlgSupcomMapOptions(string kind) : this(kind, false)
        {
        }

        public DlgSupcomMapOptions(string kind, bool selectOnly)
        {
            this.mMaps = new BindingList<SupcomMapInfo>();
            this.mKind = "1v1";
            this.components = null;
            this.InitializeComponent();
            Loc.LocObject(this);
            this.mKind = kind;
            if (selectOnly)
            {
                this.skinButtonSearch.Text = "<LOC>OK";
                this.Text = "<LOC>Ranked Game Preferences";
            }
            this.gpgMapSelectGrid.DataSource = this.mMaps;
            if (this.mKind == "1v1")
            {
                this.AddMap("Burial Mounds", SupcomMaps.SCMP_001, "SCMP_001", false);
                this.AddMap("Concord Lake", SupcomMaps.SCMP_002, "SCMP_002", false);
                this.AddMap("Drakes Ravine", SupcomMaps.SCMP_003, "SCMP_003", false);
                this.AddMap("Emerald Crater", SupcomMaps.SCMP_004, "SCMP_004", false);
                this.AddMap("Gentlemans Reef", SupcomMaps.SCMP_005, "SCMP_005", false);
                this.AddMap("Ians Cross", SupcomMaps.SCMP_006, "SCMP_006", false);
                this.AddMap("Open Palms", SupcomMaps.SCMP_007, "SCMP_007", true);
                this.AddMap("Seraphim Glaciers", SupcomMaps.SCMP_008, "SCMP_008", false);
                this.AddMap("Setons Clutch", SupcomMaps.SCMP_009, "SCMP_009", false);
                this.AddMap("Sung Island", SupcomMaps.SCMP_010, "SCMP_010", false);
                this.AddMap("The Great Void", SupcomMaps.SCMP_011, "SCMP_011", false);
                this.AddMap("Theta Passage", SupcomMaps.SCMP_012, "SCMP_012", false);
                this.AddMap("Winters Duel", SupcomMaps.SCMP_013, "SCMP_013", false);
                this.AddMap("The Bermuda Locket", SupcomMaps.SCMP_014, "SCMP_014", false);
                this.AddMap("Fields of Isis", SupcomMaps.SCMP_015, "SCMP_015", false);
                this.AddMap("Canis River", SupcomMaps.SCMP_016, "SCMP_016", false);
                this.AddMap("Syrtis Major", SupcomMaps.SCMP_017, "SCMP_017", false);
                this.AddMap("Sentry Point", SupcomMaps.SCMP_018, "SCMP_018", true);
                this.AddMap("Finns Revenge", SupcomMaps.SCMP_019, "SCMP_019", true);
                this.AddMap("Roanoke Abyss", SupcomMaps.SCMP_020, "SCMP_020", false);
                this.AddMap("Alpha 7 Quarentine", SupcomMaps.SCMP_021, "SCMP_021", false);
                this.AddMap("Arctic Refuge", SupcomMaps.SCMP_022, "SCMP_022", false);
                this.AddMap("Varga Pass", SupcomMaps.SCMP_023, "SCMP_023", false);
                this.AddMap("Crossfire Cannal", SupcomMaps.SCMP_024, "SCMP_024", false);
                this.AddMap("Saltrock Colony", SupcomMaps.SCMP_025, "SCMP_025", false);
                this.AddMap("Vya-3 Protectorate", SupcomMaps.SCMP_026, "SCMP_026", false);
                this.AddMap("The Scar", SupcomMaps.SCMP_027, "SCMP_027", false);
                this.AddMap("Hanna Oasis", SupcomMaps.SCMP_028, "SCMP_028", false);
                this.AddMap("Betrayal Ocean", SupcomMaps.SCMP_029, "SCMP_029", false);
                this.AddMap("Frostmill Ruins", SupcomMaps.SCMP_030, "SCMP_030", false);
                this.AddMap("Four-Leaf Clover", SupcomMaps.SCMP_030, "SCMP_031", false);
                this.AddMap("The Wilderness", SupcomMaps.SCMP_030, "SCMP_032", false);
                this.AddMap("White Fire", SupcomMaps.SCMP_030, "SCMP_033", false);
                this.AddMap("High Noon", SupcomMaps.SCMP_030, "SCMP_034", false);
                this.AddMap("Paradise", SupcomMaps.SCMP_030, "SCMP_035", false);
                this.AddMap("Blasted Rock", SupcomMaps.SCMP_030, "SCMP_036", false);
                this.AddMap("Sludge", SupcomMaps.SCMP_030, "SCMP_037", false);
                this.AddMap("Ambush Pass", SupcomMaps.SCMP_030, "SCMP_038", false);
                this.AddMap("Four-Corners", SupcomMaps.SCMP_030, "SCMP_039", false);
                this.AddMap("The Ditch", SupcomMaps.SCMP_030, "SCMP_040", false);
                if (GPGnetSelectedGame.SelectedGame.GameID == 0x11)
                {
                    this.AddMap("Crag Dunes", SupcomMaps.SCMP_030, "X1MP_001", true);
                    this.AddMap("Williamson's Bridge", SupcomMaps.SCMP_030, "X1MP_002", true);
                    this.AddMap("Snoey Triangle", SupcomMaps.SCMP_030, "X1MP_003", true);
                    this.AddMap("Haven Reef", SupcomMaps.SCMP_030, "X1MP_004", true);
                    this.AddMap("The Dark Heart", SupcomMaps.SCMP_030, "X1MP_005", true);
                    this.AddMap("Daroza's Sanctuary", SupcomMaps.SCMP_030, "X1MP_006", true);
                    this.AddMap("Strip Mine", SupcomMaps.SCMP_030, "X1MP_007", false);
                    this.AddMap("Thawing Glacier", SupcomMaps.SCMP_030, "X1MP_008", true);
                    this.AddMap("Liberiam Battles", SupcomMaps.SCMP_030, "X1MP_009", true);
                    this.AddMap("Shards", SupcomMaps.SCMP_030, "X1MP_010", true);
                    this.AddMap("Shuriken Island", SupcomMaps.SCMP_030, "X1MP_011", true);
                    this.AddMap("Debris", SupcomMaps.SCMP_030, "X1MP_012", false);
                    this.AddMap("X1MP_013", SupcomMaps.SCMP_030, "X1MP_013", false);
                    this.AddMap("Flooded Strip Mine", SupcomMaps.SCMP_030, "X1MP_014", false);
                    this.AddMap("X1MP_015", SupcomMaps.SCMP_030, "X1MP_015", false);
                    this.AddMap("X1MP_016", SupcomMaps.SCMP_030, "X1MP_016", false);
                    this.AddMap("Eye of the Storm", SupcomMaps.SCMP_030, "X1MP_017", true);
                    this.AddMap("X1MP_018", SupcomMaps.SCMP_030, "X1MP_018", false);
                    this.AddMap("X1MP_019", SupcomMaps.SCMP_030, "X1MP_019", false);
                    this.AddMap("X1MP_020", SupcomMaps.SCMP_030, "X1MP_020", false);
                }
            }
            else if (this.mKind == "2v2")
            {
                this.AddMap("Burial Mounds", SupcomMaps.SCMP_001, "SCMP_001", true);
                this.AddMap("Concord Lake", SupcomMaps.SCMP_002, "SCMP_002", false);
                this.AddMap("Drakes Ravine", SupcomMaps.SCMP_003, "SCMP_003", false);
                this.AddMap("Emerald Crater", SupcomMaps.SCMP_004, "SCMP_004", false);
                this.AddMap("Gentlemans Reef", SupcomMaps.SCMP_005, "SCMP_005", false);
                this.AddMap("Ians Cross", SupcomMaps.SCMP_006, "SCMP_006", false);
                this.AddMap("Open Palms", SupcomMaps.SCMP_007, "SCMP_007", true);
                this.AddMap("Seraphim Glaciers", SupcomMaps.SCMP_008, "SCMP_008", false);
                this.AddMap("Setons Clutch", SupcomMaps.SCMP_009, "SCMP_009", false);
                this.AddMap("Sung Island", SupcomMaps.SCMP_010, "SCMP_010", false);
                this.AddMap("The Great Void", SupcomMaps.SCMP_011, "SCMP_011", false);
                this.AddMap("Theta Passage", SupcomMaps.SCMP_012, "SCMP_012", false);
                this.AddMap("Winters Duel", SupcomMaps.SCMP_013, "SCMP_013", false);
                this.AddMap("The Bermuda Locket", SupcomMaps.SCMP_014, "SCMP_014", false);
                this.AddMap("Fields of Isis", SupcomMaps.SCMP_015, "SCMP_015", false);
                this.AddMap("Canis River", SupcomMaps.SCMP_016, "SCMP_016", false);
                this.AddMap("Syrtis Major", SupcomMaps.SCMP_017, "SCMP_017", false);
                this.AddMap("Sentry Point", SupcomMaps.SCMP_018, "SCMP_018", false);
                this.AddMap("Finns Revenge", SupcomMaps.SCMP_019, "SCMP_019", false);
                this.AddMap("Roanoke Abyss", SupcomMaps.SCMP_020, "SCMP_020", false);
                this.AddMap("Alpha 7 Quarentine", SupcomMaps.SCMP_021, "SCMP_021", false);
                this.AddMap("Arctic Refuge", SupcomMaps.SCMP_022, "SCMP_022", false);
                this.AddMap("Varga Pass", SupcomMaps.SCMP_023, "SCMP_023", false);
                this.AddMap("Crossfire Cannal", SupcomMaps.SCMP_024, "SCMP_024", false);
                this.AddMap("Saltrock Colony", SupcomMaps.SCMP_025, "SCMP_025", false);
                this.AddMap("Vya-3 Protectorate", SupcomMaps.SCMP_026, "SCMP_026", false);
                this.AddMap("The Scar", SupcomMaps.SCMP_027, "SCMP_027", false);
                this.AddMap("Hanna Oasis", SupcomMaps.SCMP_028, "SCMP_028", false);
                this.AddMap("Betrayal Ocean", SupcomMaps.SCMP_029, "SCMP_029", false);
                this.AddMap("Frostmill Ruins", SupcomMaps.SCMP_030, "SCMP_030", false);
                this.AddMap("Four-Leaf Clover", SupcomMaps.SCMP_030, "SCMP_031", false);
                this.AddMap("The Wilderness", SupcomMaps.SCMP_030, "SCMP_032", false);
                this.AddMap("White Fire", SupcomMaps.SCMP_030, "SCMP_033", false);
                this.AddMap("High Noon", SupcomMaps.SCMP_030, "SCMP_034", false);
                this.AddMap("Paradise", SupcomMaps.SCMP_030, "SCMP_035", false);
                this.AddMap("Blasted Rock", SupcomMaps.SCMP_030, "SCMP_036", false);
                this.AddMap("Sludge", SupcomMaps.SCMP_030, "SCMP_037", false);
                this.AddMap("Ambush Pass", SupcomMaps.SCMP_030, "SCMP_038", false);
                this.AddMap("Four-Corners", SupcomMaps.SCMP_030, "SCMP_039", false);
                this.AddMap("The Ditch", SupcomMaps.SCMP_030, "SCMP_040", false);
                if (GPGnetSelectedGame.SelectedGame.GameID == 0x11)
                {
                    this.AddMap("Crag Dunes", SupcomMaps.SCMP_030, "X1MP_001", false);
                    this.AddMap("Williamson's Bridge", SupcomMaps.SCMP_030, "X1MP_002", false);
                    this.AddMap("Snoey Triangle3", SupcomMaps.SCMP_030, "X1MP_003", false);
                    this.AddMap("Haven Reef", SupcomMaps.SCMP_030, "X1MP_004", false);
                    this.AddMap("The Dark Heart", SupcomMaps.SCMP_030, "X1MP_005", false);
                    this.AddMap("Daroza's Sanctuary", SupcomMaps.SCMP_030, "X1MP_006", false);
                    this.AddMap("Strip Mine", SupcomMaps.SCMP_030, "X1MP_007", false);
                    this.AddMap("Thawing Glacier", SupcomMaps.SCMP_030, "X1MP_008", false);
                    this.AddMap("Liberiam Battles", SupcomMaps.SCMP_030, "X1MP_009", false);
                    this.AddMap("Shards", SupcomMaps.SCMP_030, "X1MP_010", false);
                    this.AddMap("Shuriken Island", SupcomMaps.SCMP_030, "X1MP_011", false);
                    this.AddMap("Debris", SupcomMaps.SCMP_030, "X1MP_012", false);
                    this.AddMap("X1MP_013", SupcomMaps.SCMP_030, "X1MP_013", false);
                    this.AddMap("Flooded Strip Mine", SupcomMaps.SCMP_030, "X1MP_014", false);
                    this.AddMap("X1MP_015", SupcomMaps.SCMP_030, "X1MP_015", false);
                    this.AddMap("X1MP_016", SupcomMaps.SCMP_030, "X1MP_016", false);
                    this.AddMap("Eye of the Storm", SupcomMaps.SCMP_030, "X1MP_017", false);
                    this.AddMap("X1MP_018", SupcomMaps.SCMP_030, "X1MP_018", false);
                    this.AddMap("X1MP_019", SupcomMaps.SCMP_030, "X1MP_019", false);
                    this.AddMap("X1MP_020", SupcomMaps.SCMP_030, "X1MP_020", false);
                }
            }
            else if (this.mKind == "3v3")
            {
                this.AddMap("Burial Mounds", SupcomMaps.SCMP_001, "SCMP_001", true);
                this.AddMap("Concord Lake", SupcomMaps.SCMP_002, "SCMP_002", false);
                this.AddMap("Drakes Ravine", SupcomMaps.SCMP_003, "SCMP_003", false);
                this.AddMap("Emerald Crater", SupcomMaps.SCMP_004, "SCMP_004", false);
                this.AddMap("Gentlemans Reef", SupcomMaps.SCMP_005, "SCMP_005", false);
                this.AddMap("Ians Cross", SupcomMaps.SCMP_006, "SCMP_006", false);
                this.AddMap("Open Palms", SupcomMaps.SCMP_007, "SCMP_007", false);
                this.AddMap("Seraphim Glaciers", SupcomMaps.SCMP_008, "SCMP_008", false);
                this.AddMap("Setons Clutch", SupcomMaps.SCMP_009, "SCMP_009", false);
                this.AddMap("Sung Island", SupcomMaps.SCMP_010, "SCMP_010", false);
                this.AddMap("The Great Void", SupcomMaps.SCMP_011, "SCMP_011", false);
                this.AddMap("Theta Passage", SupcomMaps.SCMP_012, "SCMP_012", false);
                this.AddMap("Winters Duel", SupcomMaps.SCMP_013, "SCMP_013", false);
                this.AddMap("The Bermuda Locket", SupcomMaps.SCMP_014, "SCMP_014", false);
                this.AddMap("Fields of Isis", SupcomMaps.SCMP_015, "SCMP_015", false);
                this.AddMap("Canis River", SupcomMaps.SCMP_016, "SCMP_016", false);
                this.AddMap("Syrtis Major", SupcomMaps.SCMP_017, "SCMP_017", false);
                this.AddMap("Sentry Point", SupcomMaps.SCMP_018, "SCMP_018", false);
                this.AddMap("Finns Revenge", SupcomMaps.SCMP_019, "SCMP_019", false);
                this.AddMap("Roanoke Abyss", SupcomMaps.SCMP_020, "SCMP_020", true);
                this.AddMap("Alpha 7 Quarentine", SupcomMaps.SCMP_021, "SCMP_021", false);
                this.AddMap("Arctic Refuge", SupcomMaps.SCMP_022, "SCMP_022", false);
                this.AddMap("Varga Pass", SupcomMaps.SCMP_023, "SCMP_023", false);
                this.AddMap("Crossfire Cannal", SupcomMaps.SCMP_024, "SCMP_024", false);
                this.AddMap("Saltrock Colony", SupcomMaps.SCMP_025, "SCMP_025", false);
                this.AddMap("Vya-3 Protectorate", SupcomMaps.SCMP_026, "SCMP_026", false);
                this.AddMap("The Scar", SupcomMaps.SCMP_027, "SCMP_027", false);
                this.AddMap("Hanna Oasis", SupcomMaps.SCMP_028, "SCMP_028", false);
                this.AddMap("Betrayal Ocean", SupcomMaps.SCMP_029, "SCMP_029", false);
                this.AddMap("Frostmill Ruins", SupcomMaps.SCMP_030, "SCMP_030", false);
                this.AddMap("Four-Leaf Clover", SupcomMaps.SCMP_030, "SCMP_031", false);
                this.AddMap("The Wilderness", SupcomMaps.SCMP_030, "SCMP_032", false);
                this.AddMap("White Fire", SupcomMaps.SCMP_030, "SCMP_033", false);
                this.AddMap("High Noon", SupcomMaps.SCMP_030, "SCMP_034", false);
                this.AddMap("Paradise", SupcomMaps.SCMP_030, "SCMP_035", false);
                this.AddMap("Blasted Rock", SupcomMaps.SCMP_030, "SCMP_036", false);
                this.AddMap("Sludge", SupcomMaps.SCMP_030, "SCMP_037", false);
                this.AddMap("Ambush Pass", SupcomMaps.SCMP_030, "SCMP_038", false);
                this.AddMap("Four-Corners", SupcomMaps.SCMP_030, "SCMP_039", false);
                this.AddMap("The Ditch", SupcomMaps.SCMP_030, "SCMP_040", false);
            }
            else
            {
                this.AddMap("Burial Mounds", SupcomMaps.SCMP_001, "SCMP_001", true);
                this.AddMap("Concord Lake", SupcomMaps.SCMP_002, "SCMP_002", false);
                this.AddMap("Drakes Ravine", SupcomMaps.SCMP_003, "SCMP_003", false);
                this.AddMap("Emerald Crater", SupcomMaps.SCMP_004, "SCMP_004", false);
                this.AddMap("Gentlemans Reef", SupcomMaps.SCMP_005, "SCMP_005", false);
                this.AddMap("Ians Cross", SupcomMaps.SCMP_006, "SCMP_006", false);
                this.AddMap("Open Palms", SupcomMaps.SCMP_007, "SCMP_007", false);
                this.AddMap("Seraphim Glaciers", SupcomMaps.SCMP_008, "SCMP_008", false);
                this.AddMap("Setons Clutch", SupcomMaps.SCMP_009, "SCMP_009", false);
                this.AddMap("Sung Island", SupcomMaps.SCMP_010, "SCMP_010", false);
                this.AddMap("The Great Void", SupcomMaps.SCMP_011, "SCMP_011", false);
                this.AddMap("Theta Passage", SupcomMaps.SCMP_012, "SCMP_012", false);
                this.AddMap("Winters Duel", SupcomMaps.SCMP_013, "SCMP_013", false);
                this.AddMap("The Bermuda Locket", SupcomMaps.SCMP_014, "SCMP_014", false);
                this.AddMap("Fields of Isis", SupcomMaps.SCMP_015, "SCMP_015", false);
                this.AddMap("Canis River", SupcomMaps.SCMP_016, "SCMP_016", false);
                this.AddMap("Syrtis Major", SupcomMaps.SCMP_017, "SCMP_017", false);
                this.AddMap("Sentry Point", SupcomMaps.SCMP_018, "SCMP_018", false);
                this.AddMap("Finns Revenge", SupcomMaps.SCMP_019, "SCMP_019", false);
                this.AddMap("Roanoke Abyss", SupcomMaps.SCMP_020, "SCMP_020", false);
                this.AddMap("Alpha 7 Quarentine", SupcomMaps.SCMP_021, "SCMP_021", false);
                this.AddMap("Arctic Refuge", SupcomMaps.SCMP_022, "SCMP_022", false);
                this.AddMap("Varga Pass", SupcomMaps.SCMP_023, "SCMP_023", false);
                this.AddMap("Crossfire Cannal", SupcomMaps.SCMP_024, "SCMP_024", false);
                this.AddMap("Saltrock Colony", SupcomMaps.SCMP_025, "SCMP_025", false);
                this.AddMap("Vya-3 Protectorate", SupcomMaps.SCMP_026, "SCMP_026", false);
                this.AddMap("The Scar", SupcomMaps.SCMP_027, "SCMP_027", false);
                this.AddMap("Hanna Oasis", SupcomMaps.SCMP_028, "SCMP_028", false);
                this.AddMap("Betrayal Ocean", SupcomMaps.SCMP_029, "SCMP_029", false);
                this.AddMap("Frostmill Ruins", SupcomMaps.SCMP_030, "SCMP_030", false);
                this.AddMap("Four-Leaf Clover", SupcomMaps.SCMP_030, "SCMP_031", false);
                this.AddMap("The Wilderness", SupcomMaps.SCMP_030, "SCMP_032", false);
                this.AddMap("White Fire", SupcomMaps.SCMP_030, "SCMP_033", false);
                this.AddMap("High Noon", SupcomMaps.SCMP_030, "SCMP_034", false);
                this.AddMap("Paradise", SupcomMaps.SCMP_030, "SCMP_035", false);
                this.AddMap("Blasted Rock", SupcomMaps.SCMP_030, "SCMP_036", false);
                this.AddMap("Sludge", SupcomMaps.SCMP_030, "SCMP_037", false);
                this.AddMap("Ambush Pass", SupcomMaps.SCMP_030, "SCMP_038", false);
                this.AddMap("Four-Corners", SupcomMaps.SCMP_030, "SCMP_039", false);
                this.AddMap("The Ditch", SupcomMaps.SCMP_030, "SCMP_040", false);
                if (GPGnetSelectedGame.SelectedGame.GameID == 0x11)
                {
                    this.AddMap("Crag Dunes", SupcomMaps.SCMP_030, "X1MP_001", false);
                    this.AddMap("Williamson's Bridge", SupcomMaps.SCMP_030, "X1MP_002", false);
                    this.AddMap("Snoey Triangle3", SupcomMaps.SCMP_030, "X1MP_003", false);
                    this.AddMap("Haven Reef", SupcomMaps.SCMP_030, "X1MP_004", false);
                    this.AddMap("The Dark Heart", SupcomMaps.SCMP_030, "X1MP_005", false);
                    this.AddMap("Daroza's Sanctuary", SupcomMaps.SCMP_030, "X1MP_006", false);
                    this.AddMap("Strip Mine", SupcomMaps.SCMP_030, "X1MP_007", false);
                    this.AddMap("Thawing Glacier", SupcomMaps.SCMP_030, "X1MP_008", false);
                    this.AddMap("Liberiam Battles", SupcomMaps.SCMP_030, "X1MP_009", false);
                    this.AddMap("Shards", SupcomMaps.SCMP_030, "X1MP_010", false);
                    this.AddMap("Shuriken Island", SupcomMaps.SCMP_030, "X1MP_011", false);
                    this.AddMap("Debris", SupcomMaps.SCMP_030, "X1MP_012", false);
                    this.AddMap("X1MP_013", SupcomMaps.SCMP_030, "X1MP_013", false);
                    this.AddMap("Flooded Strip Mine", SupcomMaps.SCMP_030, "X1MP_014", false);
                    this.AddMap("X1MP_015", SupcomMaps.SCMP_030, "X1MP_015", false);
                    this.AddMap("X1MP_016", SupcomMaps.SCMP_030, "X1MP_016", false);
                    this.AddMap("Eye of the Storm", SupcomMaps.SCMP_030, "X1MP_017", false);
                    this.AddMap("X1MP_018", SupcomMaps.SCMP_030, "X1MP_018", false);
                    this.AddMap("X1MP_019", SupcomMaps.SCMP_030, "X1MP_019", false);
                    this.AddMap("X1MP_020", SupcomMaps.SCMP_030, "X1MP_020", false);
                }
            }
            string faction = Program.Settings.SupcomPrefs.RankedGames.Faction;
            if (faction != null)
            {
                if (!(faction == "/aeon"))
                {
                    if (faction == "/cybran")
                    {
                        this.rbCybran.Checked = true;
                        goto Label_1588;
                    }
                    if (faction == "/uef")
                    {
                        this.rbUEF.Checked = true;
                        goto Label_1588;
                    }
                    if (faction == "/seraphim")
                    {
                        this.rbSeraphim.Checked = true;
                        goto Label_1588;
                    }
                }
                else
                {
                    this.rbAeon.Checked = true;
                    goto Label_1588;
                }
            }
            this.rbRandom.Checked = true;
        Label_1588:
            if (!ConfigSettings.GetBool("NO_CYBRAN", true))
            {
                this.rbCybran.Checked = false;
                this.rbCybran.Enabled = false;
            }
            if (!ConfigSettings.GetBool("NO_AEON", true))
            {
                this.rbAeon.Checked = false;
                this.rbAeon.Enabled = false;
            }
            if (!ConfigSettings.GetBool("NO_UEF", true))
            {
                this.rbUEF.Checked = false;
                this.rbUEF.Enabled = false;
            }
            if (!ConfigSettings.GetBool("NO_SERAPHIM", true))
            {
                this.rbSeraphim.Checked = false;
                this.rbSeraphim.Enabled = false;
            }
            if (!ConfigSettings.GetBool("NO_RANDOM", true))
            {
                this.rbRandom.Checked = false;
                this.rbRandom.Enabled = false;
            }
            bool hasOriginal = true;
            bool hasExpansion = true;
            SupcomPrefs.TestFactions(out hasOriginal, out hasExpansion);
            if (!hasOriginal)
            {
                this.rbCybran.Checked = false;
                this.rbCybran.Enabled = false;
                this.rbAeon.Checked = false;
                this.rbAeon.Enabled = false;
                this.rbUEF.Checked = false;
                this.rbUEF.Enabled = false;
                this.rbRandom.Checked = false;
                this.rbRandom.Enabled = false;
                this.rbSeraphim.Checked = true;
            }
            if (!hasExpansion)
            {
                this.rbSeraphim.Checked = false;
                this.rbSeraphim.Enabled = false;
            }
            this.lMapPrefs.Text = string.Format(Loc.Get("<LOC>Step 1: Select your map preferences -- you may have only {0} thumbs up and {1} thumbs down."), this.GetMaxCount, this.GetMaxCount);
        }

        public void AddDefaultMaps(string category)
        {
            if (category == "1v1")
            {
                this.AddMap("Burial Mounds", SupcomMaps.SCMP_001, "SCMP_001", false);
                this.AddMap("Concord Lake", SupcomMaps.SCMP_002, "SCMP_002", false);
                this.AddMap("Drakes Ravine", SupcomMaps.SCMP_003, "SCMP_003", false);
                this.AddMap("Emerald Crater", SupcomMaps.SCMP_004, "SCMP_004", false);
                this.AddMap("Gentlemans Reef", SupcomMaps.SCMP_005, "SCMP_005", false);
                this.AddMap("Ians Cross", SupcomMaps.SCMP_006, "SCMP_006", false);
                this.AddMap("Open Palms", SupcomMaps.SCMP_007, "SCMP_007", true);
                this.AddMap("Seraphim Glaciers", SupcomMaps.SCMP_008, "SCMP_008", false);
                this.AddMap("Setons Clutch", SupcomMaps.SCMP_009, "SCMP_009", false);
                this.AddMap("Sung Island", SupcomMaps.SCMP_010, "SCMP_010", false);
                this.AddMap("The Great Void", SupcomMaps.SCMP_011, "SCMP_011", false);
                this.AddMap("Theta Passage", SupcomMaps.SCMP_012, "SCMP_012", false);
                this.AddMap("Winters Duel", SupcomMaps.SCMP_013, "SCMP_013", false);
                this.AddMap("The Bermuda Locket", SupcomMaps.SCMP_014, "SCMP_014", false);
                this.AddMap("Fields of Isis", SupcomMaps.SCMP_015, "SCMP_015", false);
                this.AddMap("Canis River", SupcomMaps.SCMP_016, "SCMP_016", false);
                this.AddMap("Syrtis Major", SupcomMaps.SCMP_017, "SCMP_017", false);
                this.AddMap("Sentry Point", SupcomMaps.SCMP_018, "SCMP_018", true);
                this.AddMap("Finns Revenge", SupcomMaps.SCMP_019, "SCMP_019", true);
                this.AddMap("Roanoke Abyss", SupcomMaps.SCMP_020, "SCMP_020", false);
                this.AddMap("Alpha 7 Quarentine", SupcomMaps.SCMP_021, "SCMP_021", false);
                this.AddMap("Arctic Refuge", SupcomMaps.SCMP_022, "SCMP_022", false);
                this.AddMap("Varga Pass", SupcomMaps.SCMP_023, "SCMP_023", false);
                this.AddMap("Crossfire Cannal", SupcomMaps.SCMP_024, "SCMP_024", false);
                this.AddMap("Saltrock Colony", SupcomMaps.SCMP_025, "SCMP_025", false);
                this.AddMap("Vya-3 Protectorate", SupcomMaps.SCMP_026, "SCMP_026", false);
                this.AddMap("The Scar", SupcomMaps.SCMP_027, "SCMP_027", false);
                this.AddMap("Hanna Oasis", SupcomMaps.SCMP_028, "SCMP_028", false);
                this.AddMap("Betrayal Ocean", SupcomMaps.SCMP_029, "SCMP_029", false);
                this.AddMap("Frostmill Ruins", SupcomMaps.SCMP_030, "SCMP_030", false);
                this.AddMap("Four-Leaf Clover", SupcomMaps.SCMP_030, "SCMP_031", false);
                this.AddMap("The Wilderness", SupcomMaps.SCMP_030, "SCMP_032", false);
                this.AddMap("White Fire", SupcomMaps.SCMP_030, "SCMP_033", false);
                this.AddMap("High Noon", SupcomMaps.SCMP_030, "SCMP_034", false);
                this.AddMap("Paradise", SupcomMaps.SCMP_030, "SCMP_035", false);
                this.AddMap("Blasted Rock", SupcomMaps.SCMP_030, "SCMP_036", false);
                this.AddMap("Sludge", SupcomMaps.SCMP_030, "SCMP_037", false);
                this.AddMap("Ambush Pass", SupcomMaps.SCMP_030, "SCMP_038", false);
                this.AddMap("Four-Corners", SupcomMaps.SCMP_030, "SCMP_039", false);
                this.AddMap("The Ditch", SupcomMaps.SCMP_030, "SCMP_040", false);
            }
            else if (category == "2v2")
            {
                this.AddMap("Burial Mounds", SupcomMaps.SCMP_001, "SCMP_001", true);
                this.AddMap("Concord Lake", SupcomMaps.SCMP_002, "SCMP_002", false);
                this.AddMap("Drakes Ravine", SupcomMaps.SCMP_003, "SCMP_003", false);
                this.AddMap("Emerald Crater", SupcomMaps.SCMP_004, "SCMP_004", false);
                this.AddMap("Gentlemans Reef", SupcomMaps.SCMP_005, "SCMP_005", false);
                this.AddMap("Ians Cross", SupcomMaps.SCMP_006, "SCMP_006", false);
                this.AddMap("Open Palms", SupcomMaps.SCMP_007, "SCMP_007", true);
                this.AddMap("Seraphim Glaciers", SupcomMaps.SCMP_008, "SCMP_008", false);
                this.AddMap("Setons Clutch", SupcomMaps.SCMP_009, "SCMP_009", false);
                this.AddMap("Sung Island", SupcomMaps.SCMP_010, "SCMP_010", false);
                this.AddMap("The Great Void", SupcomMaps.SCMP_011, "SCMP_011", false);
                this.AddMap("Theta Passage", SupcomMaps.SCMP_012, "SCMP_012", false);
                this.AddMap("Winters Duel", SupcomMaps.SCMP_013, "SCMP_013", false);
                this.AddMap("The Bermuda Locket", SupcomMaps.SCMP_014, "SCMP_014", false);
                this.AddMap("Fields of Isis", SupcomMaps.SCMP_015, "SCMP_015", false);
                this.AddMap("Canis River", SupcomMaps.SCMP_016, "SCMP_016", false);
                this.AddMap("Syrtis Major", SupcomMaps.SCMP_017, "SCMP_017", false);
                this.AddMap("Sentry Point", SupcomMaps.SCMP_018, "SCMP_018", false);
                this.AddMap("Finns Revenge", SupcomMaps.SCMP_019, "SCMP_019", false);
                this.AddMap("Roanoke Abyss", SupcomMaps.SCMP_020, "SCMP_020", false);
                this.AddMap("Alpha 7 Quarentine", SupcomMaps.SCMP_021, "SCMP_021", false);
                this.AddMap("Arctic Refuge", SupcomMaps.SCMP_022, "SCMP_022", false);
                this.AddMap("Varga Pass", SupcomMaps.SCMP_023, "SCMP_023", false);
                this.AddMap("Crossfire Cannal", SupcomMaps.SCMP_024, "SCMP_024", false);
                this.AddMap("Saltrock Colony", SupcomMaps.SCMP_025, "SCMP_025", false);
                this.AddMap("Vya-3 Protectorate", SupcomMaps.SCMP_026, "SCMP_026", false);
                this.AddMap("The Scar", SupcomMaps.SCMP_027, "SCMP_027", false);
                this.AddMap("Hanna Oasis", SupcomMaps.SCMP_028, "SCMP_028", false);
                this.AddMap("Betrayal Ocean", SupcomMaps.SCMP_029, "SCMP_029", false);
                this.AddMap("Frostmill Ruins", SupcomMaps.SCMP_030, "SCMP_030", false);
                this.AddMap("Four-Leaf Clover", SupcomMaps.SCMP_030, "SCMP_031", false);
                this.AddMap("The Wilderness", SupcomMaps.SCMP_030, "SCMP_032", false);
                this.AddMap("White Fire", SupcomMaps.SCMP_030, "SCMP_033", false);
                this.AddMap("High Noon", SupcomMaps.SCMP_030, "SCMP_034", false);
                this.AddMap("Paradise", SupcomMaps.SCMP_030, "SCMP_035", false);
                this.AddMap("Blasted Rock", SupcomMaps.SCMP_030, "SCMP_036", false);
                this.AddMap("Sludge", SupcomMaps.SCMP_030, "SCMP_037", false);
                this.AddMap("Ambush Pass", SupcomMaps.SCMP_030, "SCMP_038", false);
                this.AddMap("Four-Corners", SupcomMaps.SCMP_030, "SCMP_039", false);
                this.AddMap("The Ditch", SupcomMaps.SCMP_030, "SCMP_040", false);
            }
            else if (category == "3v3")
            {
                this.AddMap("Burial Mounds", SupcomMaps.SCMP_001, "SCMP_001", true);
                this.AddMap("Concord Lake", SupcomMaps.SCMP_002, "SCMP_002", false);
                this.AddMap("Drakes Ravine", SupcomMaps.SCMP_003, "SCMP_003", false);
                this.AddMap("Emerald Crater", SupcomMaps.SCMP_004, "SCMP_004", false);
                this.AddMap("Gentlemans Reef", SupcomMaps.SCMP_005, "SCMP_005", false);
                this.AddMap("Ians Cross", SupcomMaps.SCMP_006, "SCMP_006", false);
                this.AddMap("Open Palms", SupcomMaps.SCMP_007, "SCMP_007", false);
                this.AddMap("Seraphim Glaciers", SupcomMaps.SCMP_008, "SCMP_008", false);
                this.AddMap("Setons Clutch", SupcomMaps.SCMP_009, "SCMP_009", false);
                this.AddMap("Sung Island", SupcomMaps.SCMP_010, "SCMP_010", false);
                this.AddMap("The Great Void", SupcomMaps.SCMP_011, "SCMP_011", false);
                this.AddMap("Theta Passage", SupcomMaps.SCMP_012, "SCMP_012", false);
                this.AddMap("Winters Duel", SupcomMaps.SCMP_013, "SCMP_013", false);
                this.AddMap("The Bermuda Locket", SupcomMaps.SCMP_014, "SCMP_014", false);
                this.AddMap("Fields of Isis", SupcomMaps.SCMP_015, "SCMP_015", false);
                this.AddMap("Canis River", SupcomMaps.SCMP_016, "SCMP_016", false);
                this.AddMap("Syrtis Major", SupcomMaps.SCMP_017, "SCMP_017", false);
                this.AddMap("Sentry Point", SupcomMaps.SCMP_018, "SCMP_018", false);
                this.AddMap("Finns Revenge", SupcomMaps.SCMP_019, "SCMP_019", false);
                this.AddMap("Roanoke Abyss", SupcomMaps.SCMP_020, "SCMP_020", true);
                this.AddMap("Alpha 7 Quarentine", SupcomMaps.SCMP_021, "SCMP_021", false);
                this.AddMap("Arctic Refuge", SupcomMaps.SCMP_022, "SCMP_022", false);
                this.AddMap("Varga Pass", SupcomMaps.SCMP_023, "SCMP_023", false);
                this.AddMap("Crossfire Cannal", SupcomMaps.SCMP_024, "SCMP_024", false);
                this.AddMap("Saltrock Colony", SupcomMaps.SCMP_025, "SCMP_025", false);
                this.AddMap("Vya-3 Protectorate", SupcomMaps.SCMP_026, "SCMP_026", false);
                this.AddMap("The Scar", SupcomMaps.SCMP_027, "SCMP_027", false);
                this.AddMap("Hanna Oasis", SupcomMaps.SCMP_028, "SCMP_028", false);
                this.AddMap("Betrayal Ocean", SupcomMaps.SCMP_029, "SCMP_029", false);
                this.AddMap("Frostmill Ruins", SupcomMaps.SCMP_030, "SCMP_030", false);
                this.AddMap("Four-Leaf Clover", SupcomMaps.SCMP_030, "SCMP_031", false);
                this.AddMap("The Wilderness", SupcomMaps.SCMP_030, "SCMP_032", false);
                this.AddMap("White Fire", SupcomMaps.SCMP_030, "SCMP_033", false);
                this.AddMap("High Noon", SupcomMaps.SCMP_030, "SCMP_034", false);
                this.AddMap("Paradise", SupcomMaps.SCMP_030, "SCMP_035", false);
                this.AddMap("Blasted Rock", SupcomMaps.SCMP_030, "SCMP_036", false);
                this.AddMap("Sludge", SupcomMaps.SCMP_030, "SCMP_037", false);
                this.AddMap("Ambush Pass", SupcomMaps.SCMP_030, "SCMP_038", false);
                this.AddMap("Four-Corners", SupcomMaps.SCMP_030, "SCMP_039", false);
                this.AddMap("The Ditch", SupcomMaps.SCMP_030, "SCMP_040", false);
            }
            else
            {
                this.AddMap("Burial Mounds", SupcomMaps.SCMP_001, "SCMP_001", true);
                this.AddMap("Concord Lake", SupcomMaps.SCMP_002, "SCMP_002", false);
                this.AddMap("Drakes Ravine", SupcomMaps.SCMP_003, "SCMP_003", false);
                this.AddMap("Emerald Crater", SupcomMaps.SCMP_004, "SCMP_004", false);
                this.AddMap("Gentlemans Reef", SupcomMaps.SCMP_005, "SCMP_005", false);
                this.AddMap("Ians Cross", SupcomMaps.SCMP_006, "SCMP_006", false);
                this.AddMap("Open Palms", SupcomMaps.SCMP_007, "SCMP_007", false);
                this.AddMap("Seraphim Glaciers", SupcomMaps.SCMP_008, "SCMP_008", false);
                this.AddMap("Setons Clutch", SupcomMaps.SCMP_009, "SCMP_009", false);
                this.AddMap("Sung Island", SupcomMaps.SCMP_010, "SCMP_010", false);
                this.AddMap("The Great Void", SupcomMaps.SCMP_011, "SCMP_011", false);
                this.AddMap("Theta Passage", SupcomMaps.SCMP_012, "SCMP_012", false);
                this.AddMap("Winters Duel", SupcomMaps.SCMP_013, "SCMP_013", false);
                this.AddMap("The Bermuda Locket", SupcomMaps.SCMP_014, "SCMP_014", false);
                this.AddMap("Fields of Isis", SupcomMaps.SCMP_015, "SCMP_015", false);
                this.AddMap("Canis River", SupcomMaps.SCMP_016, "SCMP_016", false);
                this.AddMap("Syrtis Major", SupcomMaps.SCMP_017, "SCMP_017", false);
                this.AddMap("Sentry Point", SupcomMaps.SCMP_018, "SCMP_018", false);
                this.AddMap("Finns Revenge", SupcomMaps.SCMP_019, "SCMP_019", false);
                this.AddMap("Roanoke Abyss", SupcomMaps.SCMP_020, "SCMP_020", false);
                this.AddMap("Alpha 7 Quarentine", SupcomMaps.SCMP_021, "SCMP_021", false);
                this.AddMap("Arctic Refuge", SupcomMaps.SCMP_022, "SCMP_022", false);
                this.AddMap("Varga Pass", SupcomMaps.SCMP_023, "SCMP_023", false);
                this.AddMap("Crossfire Cannal", SupcomMaps.SCMP_024, "SCMP_024", false);
                this.AddMap("Saltrock Colony", SupcomMaps.SCMP_025, "SCMP_025", false);
                this.AddMap("Vya-3 Protectorate", SupcomMaps.SCMP_026, "SCMP_026", false);
                this.AddMap("The Scar", SupcomMaps.SCMP_027, "SCMP_027", false);
                this.AddMap("Hanna Oasis", SupcomMaps.SCMP_028, "SCMP_028", false);
                this.AddMap("Betrayal Ocean", SupcomMaps.SCMP_029, "SCMP_029", false);
                this.AddMap("Frostmill Ruins", SupcomMaps.SCMP_030, "SCMP_030", false);
                this.AddMap("Four-Leaf Clover", SupcomMaps.SCMP_030, "SCMP_031", false);
                this.AddMap("The Wilderness", SupcomMaps.SCMP_030, "SCMP_032", false);
                this.AddMap("White Fire", SupcomMaps.SCMP_030, "SCMP_033", false);
                this.AddMap("High Noon", SupcomMaps.SCMP_030, "SCMP_034", false);
                this.AddMap("Paradise", SupcomMaps.SCMP_030, "SCMP_035", false);
                this.AddMap("Blasted Rock", SupcomMaps.SCMP_030, "SCMP_036", false);
                this.AddMap("Sludge", SupcomMaps.SCMP_030, "SCMP_037", false);
                this.AddMap("Ambush Pass", SupcomMaps.SCMP_030, "SCMP_038", false);
                this.AddMap("Four-Corners", SupcomMaps.SCMP_030, "SCMP_039", false);
                this.AddMap("The Ditch", SupcomMaps.SCMP_030, "SCMP_040", false);
            }
        }

        public bool AddMap(string mapid, bool selectable)
        {
            SupcomMapInfo item = null;
            foreach (SupcomMap map in SupcomMapList.Maps)
            {
                if (map.Path.ToUpper().IndexOf(mapid.ToUpper()) >= 0)
                {
                    item = new SupcomMapInfo();
                    item.Selectable = selectable;
                    item.Thumbnail = map.Image;
                    item.Description = map.MapName;
                    item.MapID = map.MapID;
                    item.Priority = true;
                    this.mMaps.Add(item);
                    return true;
                }
            }
            return false;
        }

        private void AddMap(string name, Image image, string mapid, bool defaultaction)
        {
            string str = "";
            if (GameInformation.SelectedGame.GameDescription != "Supreme Commander")
            {
                str = GameInformation.SelectedGame.GameID.ToString();
            }
            if (ConfigSettings.GetBool(str + "AUTO_" + this.mKind + "_" + mapid, defaultaction))
            {
                SupcomMapInfo item = new SupcomMapInfo();
                item.Thumbnail = image;
                foreach (SupcomMap map in SupcomMapList.Maps)
                {
                    if (map.Path.ToUpper().IndexOf(mapid.ToUpper()) >= 0)
                    {
                        item.Thumbnail = map.Image;
                        break;
                    }
                }
                item.Description = name;
                item.MapID = mapid;
                if (Program.Settings.SupcomPrefs.RankedGames.Maps != null)
                {
                    for (int i = 0; i < Program.Settings.SupcomPrefs.RankedGames.Maps.Length; i++)
                    {
                        if (Program.Settings.SupcomPrefs.RankedGames.Maps[i].MapID == item.MapID)
                        {
                            item.Priority = Program.Settings.SupcomPrefs.RankedGames.Maps[i].Priority;
                        }
                    }
                }
                this.mMaps.Add(item);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
        }

        private void cardView1_Click(object sender, EventArgs e)
        {
            try
            {
                int[] selectedRows = this.cardView1.GetSelectedRows();
                if ((selectedRows == null) || (selectedRows.Length < 1))
                {
                    return;
                }
                SupcomMapInfo row = this.cardView1.GetRow(selectedRows[0]) as SupcomMapInfo;
                if (!row.Selectable)
                {
                    return;
                }
            }
            catch
            {
            }
            int num = 0;
            int num2 = 0;
            foreach (SupcomMapInfo info in this.mMaps)
            {
                if (info.Priority == true)
                {
                    num++;
                }
                else if (info.Priority == false)
                {
                    num2++;
                }
            }
            ContextMenuStrip strip = new ContextMenuStrip();
            ToolStripButton button = new ToolStripButton(Loc.Get("<LOC>Neutral"));
            button.Click += new EventHandler(this.NeutralClick);
            button.Image = SupremeCommanderIcons.ResourceManager.GetObject("neutral") as Bitmap;
            button.Height = 40;
            strip.Items.Add(button);
            button = new ToolStripButton(Loc.Get("<LOC>Thumbs Up"));
            button.Click += new EventHandler(this.ThumbsUpClick);
            button.Image = SupremeCommanderIcons.ResourceManager.GetObject("thumbsup") as Bitmap;
            button.Enabled = num < this.GetMaxCount;
            button.Height = 40;
            strip.Items.Add(button);
            button = new ToolStripButton(Loc.Get("<LOC>Thumbs Down"));
            button.Click += new EventHandler(this.ThumbsDownClick);
            button.Image = SupremeCommanderIcons.ResourceManager.GetObject("thumbsdown") as Bitmap;
            button.Enabled = num2 < this.GetMaxCount;
            button.Height = 40;
            strip.Items.Add(button);
            strip.Show(this, new Point(Control.MousePosition.X - base.Left, Control.MousePosition.Y - base.Top));
        }

        private void cardView1_CustomDrawCardField(object sender, RowCellCustomDrawEventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public string FetchMap()
        {
            Random random = new Random();
            string mapID = "SCMP_019";
            int num = 0;
            foreach (SupcomMapInfo info in this.mMaps)
            {
                int num2 = 0;
                if (info.Priority == true)
                {
                    num2 = random.Next(1, 0x3e8);
                }
                else if (!info.Priority.HasValue)
                {
                    num2 = random.Next(1, 500);
                }
                else
                {
                    num2 = random.Next(1, 100);
                }
                if (num2 > num)
                {
                    mapID = info.MapID;
                    num = num2;
                }
            }
            return mapID;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(DlgSupcomMapOptions));
            this.gpgMapSelectGrid = new GPGChatGrid();
            this.cardView1 = new CardView();
            this.colMapTitle = new GridColumn();
            this.colStatus = new GridColumn();
            this.riPriority = new RepositoryItemImageComboBox();
            this.ilThumbs = new ImageList(this.components);
            this.colMap = new GridColumn();
            this.riMap = new RepositoryItemPictureEdit();
            this.rimPictureEdit3 = new RepositoryItemPictureEdit();
            this.rimTextEdit = new RepositoryItemTextEdit();
            this.rimMemoEdit3 = new RepositoryItemMemoEdit();
            this.riPopup = new RepositoryItemPopupContainerEdit();
            this.repositoryItemTimeEdit1 = new RepositoryItemTimeEdit();
            this.repositoryItemTextEdit1 = new RepositoryItemTextEdit();
            this.riPictureStatus = new RepositoryItemPictureEdit();
            this.riButtonStatus = new RepositoryItemButtonEdit();
            this.riTextPriority = new RepositoryItemTextEdit();
            this.colTime = new GridColumn();
            this.colType = new GridColumn();
            this.colDescription = new GridColumn();
            this.colData = new GridColumn();
            this.rbAeon = new RadioButton();
            this.rbCybran = new RadioButton();
            this.rbUEF = new RadioButton();
            this.rbRandom = new RadioButton();
            this.lMapPrefs = new GPGLabel();
            this.gpgLabel1 = new GPGLabel();
            this.skinButtonCancel = new SkinButton();
            this.skinButtonSearch = new SkinButton();
            this.rbSeraphim = new RadioButton();
            ((ISupportInitialize) base.pbBottom).BeginInit();
            this.gpgMapSelectGrid.BeginInit();
            this.cardView1.BeginInit();
            this.riPriority.BeginInit();
            this.riMap.BeginInit();
            this.rimPictureEdit3.BeginInit();
            this.rimTextEdit.BeginInit();
            this.rimMemoEdit3.BeginInit();
            this.riPopup.BeginInit();
            this.repositoryItemTimeEdit1.BeginInit();
            this.repositoryItemTextEdit1.BeginInit();
            this.riPictureStatus.BeginInit();
            this.riButtonStatus.BeginInit();
            this.riTextPriority.BeginInit();
            base.SuspendLayout();
            base.pbBottom.Size = new Size(0x3c5, 0x39);
            base.ttDefault.SetSuperTip(base.pbBottom, null);
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            base.ttDefault.DefaultController.ToolTipLocation = ToolTipLocation.RightTop;
            this.gpgMapSelectGrid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgMapSelectGrid.CustomizeStyle = false;
            this.gpgMapSelectGrid.EmbeddedNavigator.Name = "";
            this.gpgMapSelectGrid.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgMapSelectGrid.IgnoreMouseWheel = false;
            this.gpgMapSelectGrid.Location = new Point(12, 0x55);
            this.gpgMapSelectGrid.LookAndFeel.SkinName = "Money Twins";
            this.gpgMapSelectGrid.LookAndFeel.Style = LookAndFeelStyle.Office2003;
            this.gpgMapSelectGrid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgMapSelectGrid.MainView = this.cardView1;
            this.gpgMapSelectGrid.Name = "gpgMapSelectGrid";
            this.gpgMapSelectGrid.RepositoryItems.AddRange(new RepositoryItem[] { this.rimPictureEdit3, this.rimTextEdit, this.rimMemoEdit3, this.riPopup, this.repositoryItemTimeEdit1, this.riPriority, this.riMap, this.repositoryItemTextEdit1, this.riPictureStatus, this.riButtonStatus, this.riTextPriority });
            this.gpgMapSelectGrid.ShowOnlyPredefinedDetails = true;
            this.gpgMapSelectGrid.Size = new Size(0x3e8, 0x12b);
            this.gpgMapSelectGrid.TabIndex = 0x1d;
            this.gpgMapSelectGrid.ViewCollection.AddRange(new BaseView[] { this.cardView1 });
            this.cardView1.Appearance.Card.BackColor = Color.Black;
            this.cardView1.Appearance.Card.ForeColor = Color.White;
            this.cardView1.Appearance.Card.Options.UseBackColor = true;
            this.cardView1.Appearance.Card.Options.UseForeColor = true;
            this.cardView1.Appearance.EmptySpace.BackColor = Color.Black;
            this.cardView1.Appearance.EmptySpace.Options.UseBackColor = true;
            this.cardView1.Appearance.FieldCaption.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cardView1.Appearance.FieldCaption.ForeColor = Color.FromArgb(0x8f, 0xbd, 0xd1);
            this.cardView1.Appearance.FieldCaption.Options.UseFont = true;
            this.cardView1.Appearance.FieldCaption.Options.UseForeColor = true;
            this.cardView1.Appearance.FieldValue.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cardView1.Appearance.FieldValue.ForeColor = Color.White;
            this.cardView1.Appearance.FieldValue.Options.UseFont = true;
            this.cardView1.Appearance.FieldValue.Options.UseForeColor = true;
            this.cardView1.Appearance.FilterCloseButton.BackColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.cardView1.Appearance.FilterCloseButton.BackColor2 = Color.FromArgb(90, 90, 90);
            this.cardView1.Appearance.FilterCloseButton.BorderColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.cardView1.Appearance.FilterCloseButton.ForeColor = Color.Black;
            this.cardView1.Appearance.FilterCloseButton.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.cardView1.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.cardView1.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.cardView1.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.cardView1.Appearance.FilterPanel.BackColor = Color.Black;
            this.cardView1.Appearance.FilterPanel.BackColor2 = Color.FromArgb(0xd4, 0xd0, 200);
            this.cardView1.Appearance.FilterPanel.ForeColor = Color.White;
            this.cardView1.Appearance.FilterPanel.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.cardView1.Appearance.FilterPanel.Options.UseBackColor = true;
            this.cardView1.Appearance.FilterPanel.Options.UseForeColor = true;
            this.cardView1.Appearance.SeparatorLine.BackColor = Color.Black;
            this.cardView1.Appearance.SeparatorLine.Options.UseBackColor = true;
            this.cardView1.BorderStyle = BorderStyles.NoBorder;
            this.cardView1.Columns.AddRange(new GridColumn[] { this.colMapTitle, this.colStatus, this.colMap });
            this.cardView1.DetailHeight = 400;
            this.cardView1.FocusedCardTopFieldIndex = 0;
            this.cardView1.GridControl = this.gpgMapSelectGrid;
            this.cardView1.Name = "cardView1";
            this.cardView1.OptionsBehavior.AllowExpandCollapse = false;
            this.cardView1.OptionsBehavior.AutoPopulateColumns = false;
            this.cardView1.OptionsBehavior.FieldAutoHeight = true;
            this.cardView1.OptionsBehavior.Sizeable = false;
            this.cardView1.OptionsSelection.MultiSelect = true;
            this.cardView1.OptionsView.ShowCardCaption = false;
            this.cardView1.OptionsView.ShowFieldCaptions = false;
            this.cardView1.OptionsView.ShowQuickCustomizeButton = false;
            this.cardView1.VertScrollVisibility = ScrollVisibility.Auto;
            this.cardView1.CustomDrawCardField += new RowCellCustomDrawEventHandler(this.cardView1_CustomDrawCardField);
            this.cardView1.Click += new EventHandler(this.cardView1_Click);
            this.colMapTitle.AppearanceCell.BackColor = Color.FromArgb(0x1b, 0x2e, 0x4a);
            this.colMapTitle.AppearanceCell.BackColor2 = Color.FromArgb(12, 0x17, 0x29);
            this.colMapTitle.AppearanceCell.Font = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.colMapTitle.AppearanceCell.ForeColor = Color.FromArgb(0x8f, 0xbd, 0xd1);
            this.colMapTitle.AppearanceCell.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.colMapTitle.AppearanceCell.Options.UseBackColor = true;
            this.colMapTitle.AppearanceCell.Options.UseFont = true;
            this.colMapTitle.AppearanceCell.Options.UseForeColor = true;
            this.colMapTitle.AppearanceCell.Options.UseTextOptions = true;
            this.colMapTitle.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
            this.colMapTitle.AppearanceCell.TextOptions.Trimming = Trimming.None;
            this.colMapTitle.AppearanceCell.TextOptions.VAlignment = VertAlignment.Top;
            this.colMapTitle.AppearanceHeader.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.colMapTitle.AppearanceHeader.Options.UseFont = true;
            this.colMapTitle.Caption = "Map Name";
            this.colMapTitle.FieldName = "Description";
            this.colMapTitle.Name = "colMapTitle";
            this.colMapTitle.OptionsColumn.AllowEdit = false;
            this.colMapTitle.Visible = true;
            this.colMapTitle.VisibleIndex = 0;
            this.colStatus.AppearanceCell.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.colStatus.AppearanceCell.Options.UseFont = true;
            this.colStatus.AppearanceCell.Options.UseTextOptions = true;
            this.colStatus.AppearanceCell.TextOptions.Trimming = Trimming.None;
            this.colStatus.AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
            this.colStatus.Caption = "Priority";
            this.colStatus.ColumnEdit = this.riPriority;
            this.colStatus.FieldName = "Priority";
            this.colStatus.Name = "colStatus";
            this.colStatus.OptionsColumn.AllowEdit = false;
            this.colStatus.OptionsColumn.AllowFocus = false;
            this.colStatus.OptionsColumn.ReadOnly = true;
            this.colStatus.ShowButtonMode = ShowButtonModeEnum.ShowOnlyInEditor;
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 1;
            this.riPriority.AllowNullInput = DefaultBoolean.True;
            this.riPriority.Appearance.BackColor = Color.Black;
            this.riPriority.Appearance.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.riPriority.Appearance.ForeColor = Color.White;
            this.riPriority.Appearance.Options.UseBackColor = true;
            this.riPriority.Appearance.Options.UseFont = true;
            this.riPriority.Appearance.Options.UseForeColor = true;
            this.riPriority.AppearanceDropDown.BackColor = Color.Black;
            this.riPriority.AppearanceDropDown.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.riPriority.AppearanceDropDown.ForeColor = Color.White;
            this.riPriority.AppearanceDropDown.Options.UseBackColor = true;
            this.riPriority.AppearanceDropDown.Options.UseFont = true;
            this.riPriority.AppearanceDropDown.Options.UseForeColor = true;
            this.riPriority.AutoHeight = false;
            this.riPriority.DropDownRows = 3;
            this.riPriority.HideSelection = false;
            this.riPriority.Items.AddRange(new ImageComboBoxItem[] { new ImageComboBoxItem("<LOC>Neutral", null, 0), new ImageComboBoxItem("<LOC>Thumbs Up", true, 1), new ImageComboBoxItem("<LOC>Thumbs Down", false, 2) });
            this.riPriority.LargeImages = this.ilThumbs;
            this.riPriority.Name = "riPriority";
            this.riPriority.Validating += new CancelEventHandler(this.riPriority_Validating);
            this.ilThumbs.ImageStream = (ImageListStreamer) manager.GetObject("ilThumbs.ImageStream");
            this.ilThumbs.TransparentColor = Color.Transparent;
            this.ilThumbs.Images.SetKeyName(0, "neutral.png");
            this.ilThumbs.Images.SetKeyName(1, "thumbsup.png");
            this.ilThumbs.Images.SetKeyName(2, "thumbsdown.png");
            this.colMap.Caption = "Image";
            this.colMap.ColumnEdit = this.riMap;
            this.colMap.FieldName = "Thumbnail";
            this.colMap.Name = "colMap";
            this.colMap.OptionsColumn.AllowEdit = false;
            this.colMap.Visible = true;
            this.colMap.VisibleIndex = 2;
            this.riMap.CustomHeight = 200;
            this.riMap.Name = "riMap";
            this.riMap.SizeMode = PictureSizeMode.Stretch;
            this.rimPictureEdit3.Name = "rimPictureEdit3";
            this.rimPictureEdit3.PictureAlignment = ContentAlignment.TopCenter;
            this.rimTextEdit.AutoHeight = false;
            this.rimTextEdit.Name = "rimTextEdit";
            this.rimMemoEdit3.MaxLength = 500;
            this.rimMemoEdit3.Name = "rimMemoEdit3";
            this.riPopup.AutoHeight = false;
            this.riPopup.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.riPopup.Name = "riPopup";
            this.repositoryItemTimeEdit1.AutoHeight = false;
            this.repositoryItemTimeEdit1.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.repositoryItemTimeEdit1.Name = "repositoryItemTimeEdit1";
            this.repositoryItemTextEdit1.Appearance.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.repositoryItemTextEdit1.Appearance.Options.UseFont = true;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            this.riPictureStatus.Name = "riPictureStatus";
            this.riPictureStatus.PictureAlignment = ContentAlignment.MiddleLeft;
            this.riPictureStatus.ReadOnly = true;
            this.riPictureStatus.Click += new EventHandler(this.riTextPriority_Click);
            this.riButtonStatus.AllowFocused = false;
            this.riButtonStatus.AllowNullInput = DefaultBoolean.True;
            this.riButtonStatus.AutoHeight = false;
            this.riButtonStatus.Name = "riButtonStatus";
            this.riButtonStatus.ReadOnly = true;
            this.riButtonStatus.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.riButtonStatus.Click += new EventHandler(this.riTextPriority_Click);
            this.riButtonStatus.ButtonClick += new ButtonPressedEventHandler(this.riButtonStatus_ButtonClick);
            this.riTextPriority.AutoHeight = false;
            this.riTextPriority.Name = "riTextPriority";
            this.riTextPriority.ReadOnly = true;
            this.riTextPriority.Click += new EventHandler(this.riTextPriority_Click);
            this.colTime.Caption = "Time";
            this.colTime.ColumnEdit = this.repositoryItemTimeEdit1;
            this.colTime.FieldName = "DateTime";
            this.colTime.Name = "colTime";
            this.colTime.OptionsColumn.AllowEdit = false;
            this.colTime.Visible = true;
            this.colTime.VisibleIndex = 0;
            this.colType.Caption = "Type";
            this.colType.FieldName = "LogType";
            this.colType.Name = "colType";
            this.colType.OptionsColumn.AllowEdit = false;
            this.colType.Visible = true;
            this.colType.VisibleIndex = 1;
            this.colDescription.Caption = "Description";
            this.colDescription.FieldName = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.OptionsColumn.AllowEdit = false;
            this.colDescription.Visible = true;
            this.colDescription.VisibleIndex = 2;
            this.colData.Caption = "Data";
            this.colData.ColumnEdit = this.riPopup;
            this.colData.FieldName = "Data";
            this.colData.Name = "colData";
            this.colData.Visible = true;
            this.colData.VisibleIndex = 3;
            this.rbAeon.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.rbAeon.CheckAlign = ContentAlignment.BottomCenter;
            this.rbAeon.FlatStyle = FlatStyle.Flat;
            this.rbAeon.Image = Resources.aeon1;
            this.rbAeon.ImageAlign = ContentAlignment.TopCenter;
            this.rbAeon.Location = new Point(12, 0x1a7);
            this.rbAeon.Name = "rbAeon";
            this.rbAeon.Size = new Size(0xc6, 0xf5);
            base.ttDefault.SetSuperTip(this.rbAeon, null);
            this.rbAeon.TabIndex = 0x25;
            this.rbAeon.Text = "Aeon";
            this.rbAeon.TextAlign = ContentAlignment.BottomCenter;
            this.rbAeon.TextImageRelation = TextImageRelation.TextAboveImage;
            this.rbAeon.UseVisualStyleBackColor = true;
            this.rbAeon.Paint += new PaintEventHandler(this.rbRandom_Paint);
            this.rbAeon.CheckedChanged += new EventHandler(this.rbAeon_CheckedChanged);
            this.rbCybran.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.rbCybran.CheckAlign = ContentAlignment.BottomCenter;
            this.rbCybran.FlatStyle = FlatStyle.Flat;
            this.rbCybran.Image = Resources.cybran1;
            this.rbCybran.ImageAlign = ContentAlignment.TopCenter;
            this.rbCybran.Location = new Point(0xd0, 0x1a7);
            this.rbCybran.Name = "rbCybran";
            this.rbCybran.Size = new Size(0xc6, 0xf5);
            base.ttDefault.SetSuperTip(this.rbCybran, null);
            this.rbCybran.TabIndex = 0x26;
            this.rbCybran.Text = "Cybran";
            this.rbCybran.TextAlign = ContentAlignment.BottomCenter;
            this.rbCybran.TextImageRelation = TextImageRelation.TextAboveImage;
            this.rbCybran.UseVisualStyleBackColor = true;
            this.rbCybran.Paint += new PaintEventHandler(this.rbRandom_Paint);
            this.rbCybran.CheckedChanged += new EventHandler(this.rbAeon_CheckedChanged);
            this.rbUEF.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.rbUEF.CheckAlign = ContentAlignment.BottomCenter;
            this.rbUEF.FlatStyle = FlatStyle.Flat;
            this.rbUEF.Image = Resources.uef1;
            this.rbUEF.ImageAlign = ContentAlignment.TopCenter;
            this.rbUEF.Location = new Point(410, 0x1a7);
            this.rbUEF.Name = "rbUEF";
            this.rbUEF.Size = new Size(0xc6, 0xf5);
            base.ttDefault.SetSuperTip(this.rbUEF, null);
            this.rbUEF.TabIndex = 0x27;
            this.rbUEF.Text = "UEF";
            this.rbUEF.TextAlign = ContentAlignment.BottomCenter;
            this.rbUEF.TextImageRelation = TextImageRelation.TextAboveImage;
            this.rbUEF.UseVisualStyleBackColor = true;
            this.rbUEF.Paint += new PaintEventHandler(this.rbRandom_Paint);
            this.rbUEF.CheckedChanged += new EventHandler(this.rbAeon_CheckedChanged);
            this.rbRandom.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.rbRandom.BackColor = Color.Black;
            this.rbRandom.CheckAlign = ContentAlignment.BottomCenter;
            this.rbRandom.Checked = true;
            this.rbRandom.FlatStyle = FlatStyle.Flat;
            this.rbRandom.Image = Resources.random1;
            this.rbRandom.ImageAlign = ContentAlignment.TopCenter;
            this.rbRandom.Location = new Point(0x329, 0x1a7);
            this.rbRandom.Name = "rbRandom";
            this.rbRandom.Size = new Size(0xc6, 0xf5);
            base.ttDefault.SetSuperTip(this.rbRandom, null);
            this.rbRandom.TabIndex = 40;
            this.rbRandom.TabStop = true;
            this.rbRandom.Text = "<LOC>Random";
            this.rbRandom.TextAlign = ContentAlignment.BottomCenter;
            this.rbRandom.TextImageRelation = TextImageRelation.TextAboveImage;
            this.rbRandom.UseVisualStyleBackColor = false;
            this.rbRandom.Paint += new PaintEventHandler(this.rbRandom_Paint);
            this.rbRandom.CheckedChanged += new EventHandler(this.rbAeon_CheckedChanged);
            this.lMapPrefs.AutoGrowDirection = GrowDirections.None;
            this.lMapPrefs.AutoSize = true;
            this.lMapPrefs.AutoStyle = true;
            this.lMapPrefs.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lMapPrefs.ForeColor = Color.White;
            this.lMapPrefs.IgnoreMouseWheel = false;
            this.lMapPrefs.IsStyled = false;
            this.lMapPrefs.Location = new Point(0x15, 0x3e);
            this.lMapPrefs.Name = "lMapPrefs";
            this.lMapPrefs.Size = new Size(0x263, 0x10);
            base.ttDefault.SetSuperTip(this.lMapPrefs, null);
            this.lMapPrefs.TabIndex = 0x29;
            this.lMapPrefs.Text = "<LOC>Step 1: Select your map preferences -- you may have only one thumbs up and one thumbs down.";
            this.lMapPrefs.TextStyle = TextStyles.Default;
            this.gpgLabel1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.gpgLabel1.AutoGrowDirection = GrowDirections.None;
            this.gpgLabel1.AutoSize = true;
            this.gpgLabel1.AutoStyle = true;
            this.gpgLabel1.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgLabel1.ForeColor = Color.White;
            this.gpgLabel1.IgnoreMouseWheel = false;
            this.gpgLabel1.IsStyled = false;
            this.gpgLabel1.Location = new Point(12, 0x189);
            this.gpgLabel1.Name = "gpgLabel1";
            this.gpgLabel1.Size = new Size(0xcc, 0x10);
            base.ttDefault.SetSuperTip(this.gpgLabel1, null);
            this.gpgLabel1.TabIndex = 0x2a;
            this.gpgLabel1.Text = "<LOC>Step 2: Select your faction";
            this.gpgLabel1.TextStyle = TextStyles.Default;
            this.skinButtonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonCancel.AutoStyle = true;
            this.skinButtonCancel.BackColor = Color.Black;
            this.skinButtonCancel.ButtonState = 0;
            this.skinButtonCancel.DialogResult = DialogResult.OK;
            this.skinButtonCancel.DisabledForecolor = Color.Gray;
            this.skinButtonCancel.DrawColor = Color.White;
            this.skinButtonCancel.DrawEdges = true;
            this.skinButtonCancel.FocusColor = Color.Yellow;
            this.skinButtonCancel.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonCancel.ForeColor = Color.White;
            this.skinButtonCancel.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonCancel.IsStyled = true;
            this.skinButtonCancel.Location = new Point(870, 0x2a2);
            this.skinButtonCancel.Name = "skinButtonCancel";
            this.skinButtonCancel.Size = new Size(0x61, 0x17);
            this.skinButtonCancel.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonCancel, null);
            this.skinButtonCancel.TabIndex = 0x2d;
            this.skinButtonCancel.TabStop = true;
            this.skinButtonCancel.Text = "<LOC>Cancel";
            this.skinButtonCancel.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonCancel.TextPadding = new Padding(0);
            this.skinButtonCancel.Click += new EventHandler(this.skinButtonCancel_Click);
            this.skinButtonSearch.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.skinButtonSearch.AutoStyle = true;
            this.skinButtonSearch.BackColor = Color.Black;
            this.skinButtonSearch.ButtonState = 0;
            this.skinButtonSearch.DialogResult = DialogResult.OK;
            this.skinButtonSearch.DisabledForecolor = Color.Gray;
            this.skinButtonSearch.DrawColor = Color.White;
            this.skinButtonSearch.DrawEdges = true;
            this.skinButtonSearch.FocusColor = Color.Yellow;
            this.skinButtonSearch.Font = new Font("Verdana", 8f, FontStyle.Bold);
            this.skinButtonSearch.ForeColor = Color.White;
            this.skinButtonSearch.HorizontalScalingMode = ScalingModes.Tile;
            this.skinButtonSearch.IsStyled = true;
            this.skinButtonSearch.Location = new Point(0x2ff, 0x2a2);
            this.skinButtonSearch.Name = "skinButtonSearch";
            this.skinButtonSearch.Size = new Size(0x61, 0x17);
            this.skinButtonSearch.SkinBasePath = @"Controls\Button\Round Edge";
            base.ttDefault.SetSuperTip(this.skinButtonSearch, null);
            this.skinButtonSearch.TabIndex = 0x2e;
            this.skinButtonSearch.TabStop = true;
            this.skinButtonSearch.Text = "<LOC>Start Search";
            this.skinButtonSearch.TextAlign = ContentAlignment.MiddleCenter;
            this.skinButtonSearch.TextPadding = new Padding(0);
            this.skinButtonSearch.Click += new EventHandler(this.skinButton1_Click);
            this.rbSeraphim.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.rbSeraphim.BackColor = Color.Black;
            this.rbSeraphim.CheckAlign = ContentAlignment.BottomCenter;
            this.rbSeraphim.FlatStyle = FlatStyle.Flat;
            this.rbSeraphim.Image = Resources.seraphim;
            this.rbSeraphim.ImageAlign = ContentAlignment.TopCenter;
            this.rbSeraphim.Location = new Point(610, 0x1a7);
            this.rbSeraphim.Name = "rbSeraphim";
            this.rbSeraphim.Size = new Size(0xc6, 0xf5);
            base.ttDefault.SetSuperTip(this.rbSeraphim, null);
            this.rbSeraphim.TabIndex = 0x2f;
            this.rbSeraphim.Text = "Seraphim";
            this.rbSeraphim.TextAlign = ContentAlignment.BottomCenter;
            this.rbSeraphim.TextImageRelation = TextImageRelation.TextAboveImage;
            this.rbSeraphim.UseVisualStyleBackColor = false;
            this.rbSeraphim.Paint += new PaintEventHandler(this.rbRandom_Paint);
            this.rbSeraphim.CheckedChanged += new EventHandler(this.rbSeraphim_CheckedChanged);
            base.AcceptButton = this.skinButtonSearch;
            base.AutoScaleMode = AutoScaleMode.None;
            base.CancelButton = this.skinButtonCancel;
            base.ClientSize = new Size(0x400, 740);
            base.Controls.Add(this.rbSeraphim);
            base.Controls.Add(this.skinButtonSearch);
            base.Controls.Add(this.skinButtonCancel);
            base.Controls.Add(this.gpgLabel1);
            base.Controls.Add(this.lMapPrefs);
            base.Controls.Add(this.rbRandom);
            base.Controls.Add(this.rbUEF);
            base.Controls.Add(this.rbCybran);
            base.Controls.Add(this.rbAeon);
            base.Controls.Add(this.gpgMapSelectGrid);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            this.MinimumSize = new Size(0x400, 740);
            base.Name = "DlgSupcomMapOptions";
            base.ttDefault.SetSuperTip(this, null);
            this.Text = "<LOC>Ranked Match Preferences";
            base.Controls.SetChildIndex(this.gpgMapSelectGrid, 0);
            base.Controls.SetChildIndex(this.rbAeon, 0);
            base.Controls.SetChildIndex(this.rbCybran, 0);
            base.Controls.SetChildIndex(this.rbUEF, 0);
            base.Controls.SetChildIndex(this.rbRandom, 0);
            base.Controls.SetChildIndex(this.lMapPrefs, 0);
            base.Controls.SetChildIndex(this.gpgLabel1, 0);
            base.Controls.SetChildIndex(this.skinButtonCancel, 0);
            base.Controls.SetChildIndex(this.skinButtonSearch, 0);
            base.Controls.SetChildIndex(this.rbSeraphim, 0);
            ((ISupportInitialize) base.pbBottom).EndInit();
            this.gpgMapSelectGrid.EndInit();
            this.cardView1.EndInit();
            this.riPriority.EndInit();
            this.riMap.EndInit();
            this.rimPictureEdit3.EndInit();
            this.rimTextEdit.EndInit();
            this.rimMemoEdit3.EndInit();
            this.riPopup.EndInit();
            this.repositoryItemTimeEdit1.EndInit();
            this.repositoryItemTextEdit1.EndInit();
            this.riPictureStatus.EndInit();
            this.riButtonStatus.EndInit();
            this.riTextPriority.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private bool IsRegularRankedGame()
        {
            return ((ConfigSettings.GetString("Automatch GameIDs", "27") + " ").IndexOf(GameInformation.SelectedGame.GameID.ToString() + " ") >= 0);
        }

        protected override void Localize()
        {
            base.Localize();
            foreach (ImageComboBoxItem item in this.riPriority.Items)
            {
                item.Description = Loc.Get(item.Description);
            }
        }

        private void NeutralClick(object sender, EventArgs e)
        {
            if (this.cardView1.SelectedRowsCount == 1)
            {
                SupcomMapInfo row = this.cardView1.GetRow(this.cardView1.GetSelectedRows()[0]) as SupcomMapInfo;
                if (row != null)
                {
                    row.Priority = null;
                }
            }
            this.cardView1.RefreshData();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (this.IsRegularRankedGame())
            {
                base.DialogResult = DialogResult.OK;
            }
        }

        private void rbAeon_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void rbRandom_Paint(object sender, PaintEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (!button.Checked)
            {
                Brush brush = new SolidBrush(Color.FromArgb(170, 0, 0, 0));
                e.Graphics.FillRectangle(brush, button.ClientRectangle);
                brush.Dispose();
            }
        }

        private void rbSeraphim_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void riButtonStatus_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
        }

        private void riButtonStatus_Click(object sender, EventArgs e)
        {
        }

        private void riPriority_Validating(object sender, CancelEventArgs e)
        {
            ImageComboBoxEdit edit = sender as ImageComboBoxEdit;
            int num = 0;
            int num2 = 0;
            foreach (SupcomMapInfo info in this.mMaps)
            {
                if (info.Priority == true)
                {
                    num++;
                }
                else if (info.Priority == false)
                {
                    num2++;
                }
            }
            if ((((bool?) edit.EditValue) == true) && (num >= this.GetMaxCount))
            {
                DlgMessage.ShowDialog(Loc.Get("<LOC>You may have only 1 'thumbs up'.", new object[] { this.GetMaxCount }));
                edit.EditValue = null;
                e.Cancel = true;
            }
            else if ((((bool?) edit.EditValue) == false) && (num2 >= this.GetMaxCount))
            {
                DlgMessage.ShowDialog(Loc.Get("<LOC>You may have only 1 'thumbs down'.", new object[] { this.GetMaxCount }));
                edit.EditValue = null;
                e.Cancel = true;
            }
        }

        private void riTextPriority_Click(object sender, EventArgs e)
        {
        }

        internal void SaveMapPrefs()
        {
            SupcomMapInfo[] array = new SupcomMapInfo[this.mMaps.Count];
            this.mMaps.CopyTo(array, 0);
            Program.Settings.SupcomPrefs.RankedGames.Maps = array;
            if (this.Faction == Loc.Get("<LOC>random"))
            {
                Program.Settings.SupcomPrefs.RankedGames.Faction = "random";
            }
            else
            {
                Program.Settings.SupcomPrefs.RankedGames.Faction = this.Faction;
            }
        }

        public void SelectFaction(string faction)
        {
            string str = faction;
            if (str != null)
            {
                if (!(str == "aeon"))
                {
                    if (str == "cybran")
                    {
                        if (this.CybranEnabled)
                        {
                            this.rbCybran.Checked = true;
                        }
                    }
                    else if (str == "uef")
                    {
                        if (this.UEFEnabled)
                        {
                            this.rbUEF.Checked = true;
                        }
                    }
                    else if (str == "seraphim")
                    {
                        if (this.SeraphimEnabled)
                        {
                            this.rbSeraphim.Checked = true;
                        }
                    }
                    else if ((str == "random") && this.SeraphimEnabled)
                    {
                        this.rbSeraphim.Checked = true;
                    }
                }
                else if (this.AeonEnabled)
                {
                    this.rbAeon.Checked = true;
                }
            }
        }

        public void SelectPreferredFaction()
        {
            switch (Program.Settings.SupcomPrefs.RankedGames.Faction)
            {
                case "/aeon":
                    if (this.AeonEnabled)
                    {
                        this.rbAeon.Checked = true;
                    }
                    return;

                case "/cybran":
                    if (this.CybranEnabled)
                    {
                        this.rbCybran.Checked = true;
                    }
                    return;

                case "/uef":
                    if (this.UEFEnabled)
                    {
                        this.rbUEF.Checked = true;
                    }
                    return;

                case "/seraphim":
                    if (this.SeraphimEnabled)
                    {
                        this.rbSeraphim.Checked = true;
                    }
                    return;
            }
            if (this.RandomEnabled)
            {
                this.rbRandom.Checked = true;
            }
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            this.SaveMapPrefs();
            base.DialogResult = DialogResult.OK;
            base.Close();
        }

        private void skinButtonCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void ThumbsDownClick(object sender, EventArgs e)
        {
            if (this.cardView1.SelectedRowsCount == 1)
            {
                SupcomMapInfo row = this.cardView1.GetRow(this.cardView1.GetSelectedRows()[0]) as SupcomMapInfo;
                if (row != null)
                {
                    row.Priority = false;
                }
            }
            this.cardView1.RefreshData();
        }

        private void ThumbsUpClick(object sender, EventArgs e)
        {
            if (this.cardView1.SelectedRowsCount == 1)
            {
                SupcomMapInfo row = this.cardView1.GetRow(this.cardView1.GetSelectedRows()[0]) as SupcomMapInfo;
                if (row != null)
                {
                    row.Priority = true;
                }
            }
            this.cardView1.RefreshData();
        }

        public bool AeonEnabled
        {
            get
            {
                return this.rbAeon.Enabled;
            }
            set
            {
                if (!value)
                {
                    this.rbAeon.Checked = false;
                }
                this.rbAeon.Enabled = value;
            }
        }

        public bool CybranEnabled
        {
            get
            {
                return this.rbCybran.Enabled;
            }
            set
            {
                if (!value)
                {
                    this.rbCybran.Checked = false;
                }
                this.rbCybran.Enabled = value;
            }
        }

        public string Faction
        {
            get
            {
                if (this.rbAeon.Checked)
                {
                    return "/aeon";
                }
                if (this.rbCybran.Checked)
                {
                    return "/cybran";
                }
                if (this.rbUEF.Checked)
                {
                    return "/uef";
                }
                if (this.rbSeraphim.Checked)
                {
                    return "/seraphim";
                }
                return Loc.Get("<LOC>random");
            }
        }

        private int GetMaxCount
        {
            get
            {
                return ConfigSettings.GetInt("AUTO_MAP_THUMBS", 1);
            }
        }

        internal BindingList<SupcomMapInfo> Maps
        {
            get
            {
                return this.mMaps;
            }
        }

        public bool RandomEnabled
        {
            get
            {
                return this.rbRandom.Enabled;
            }
            set
            {
                if (!value)
                {
                    this.rbRandom.Checked = false;
                }
                this.rbRandom.Enabled = value;
            }
        }

        public bool SeraphimEnabled
        {
            get
            {
                return this.rbSeraphim.Enabled;
            }
            set
            {
                if (!value)
                {
                    this.rbSeraphim.Checked = false;
                }
                this.rbSeraphim.Enabled = value;
            }
        }

        public bool UEFEnabled
        {
            get
            {
                return this.rbUEF.Enabled;
            }
            set
            {
                if (!value)
                {
                    this.rbUEF.Checked = false;
                }
                this.rbUEF.Enabled = value;
            }
        }
    }
}

