namespace GPG.UI.Controls
{
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Localization;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using GPG;
    using GPG.Logging;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGDataGrid : GridControl, ILocalizable
    {
        public bool IsCheckingExpand = true;
        private bool mCustomizeStyle;
        private bool mFirstTime = true;
        private bool mIsLoaded;
        private static DevExLocalizer sLoc = new DevExLocalizer();

        [field: NonSerialized]
        public event PropertyChangedEventHandler CustomizeStyleChanged;

        public event EventHandler FinishedStyling;

        public static  event EventHandler OnStyleControl;

        public GPGDataGrid()
        {
            this.ShowOnlyPredefinedDetails = true;
        }

        private void CheckGridExpand(GridView gridView)
        {
            if (this.IsCheckingExpand)
            {
                try
                {
                    for (int i = 0; i < gridView.RowCount; i++)
                    {
                        if (gridView.GetRow(i) != null)
                        {
                            gridView.ExpandMasterRow(i);
                        }
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
            }
        }

        private void gridView_RowCountChanged(object sender, EventArgs e)
        {
            if (this.IsCheckingExpand)
            {
                GridView gridView = sender as GridView;
                this.CheckGridExpand(gridView);
            }
        }

        public void Localize()
        {
            foreach (BaseView view in base.ViewCollection)
            {
                if (view is GridView)
                {
                    foreach (GridColumn column in (view as GridView).Columns)
                    {
                        column.Caption = Loc.Get(column.Caption);
                    }
                    (view as GridView).GroupPanelText = Loc.Get((view as GridView).GroupPanelText);
                }
            }
        }

        protected override void OnLoaded()
        {
            if (!this.mIsLoaded && !base.DesignMode)
            {
                this.mIsLoaded = true;
                foreach (BaseView view in base.Views)
                {
                    if (OnStyleControl != null)
                    {
                        OnStyleControl(view, EventArgs.Empty);
                    }
                    if (this.FinishedStyling != null)
                    {
                        this.FinishedStyling(this, EventArgs.Empty);
                    }
                }
            }
            base.OnLoaded();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.mFirstTime)
            {
                this.mFirstTime = false;
                if ((OnStyleControl != null) && !base.DesignMode)
                {
                    if (OnStyleControl != null)
                    {
                        OnStyleControl(this, EventArgs.Empty);
                    }
                    if (this.FinishedStyling != null)
                    {
                        this.FinishedStyling(this, EventArgs.Empty);
                    }
                }
            }
            try
            {
                base.OnPaint(e);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        protected override void RegisterView(BaseView gv)
        {
            base.RegisterView(gv);
            if ((OnStyleControl != null) && !base.DesignMode)
            {
                if (OnStyleControl != null)
                {
                    OnStyleControl(gv, EventArgs.Empty);
                }
                if (this.FinishedStyling != null)
                {
                    this.FinishedStyling(this, EventArgs.Empty);
                }
                if (gv is GridView)
                {
                    GridView gridView = gv as GridView;
                    GridLocalizer.Active = sLoc;
                    gridView.RowCountChanged += new EventHandler(this.gridView_RowCountChanged);
                    this.CheckGridExpand(gridView);
                }
            }
        }

        public bool CustomizeStyle
        {
            get
            {
                return this.mCustomizeStyle;
            }
            set
            {
                this.mCustomizeStyle = value;
                if (this.mCustomizeStyleChanged != null)
                {
                    this.mCustomizeStyleChanged(this, new PropertyChangedEventArgs("CustomizeStyle"));
                }
            }
        }
    }
}

