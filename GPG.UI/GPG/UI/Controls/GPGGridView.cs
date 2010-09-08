namespace GPG.UI.Controls
{
    using DevExpress.XtraEditors.ViewInfo;
    using DevExpress.XtraGrid.Scrolling;
    using DevExpress.XtraGrid.Views.Grid;
    using GPG.Logging;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows.Forms;

    public class GPGGridView : GridView
    {
        private IContainer components;

        public GPGGridView()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public static DevExpress.XtraGrid.Scrolling.ScrollInfo GetScrollInfo(GridView gv)
        {
            try
            {
                return (gv.GetType().GetProperty("ScrollInfo", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gv, null) as DevExpress.XtraGrid.Scrolling.ScrollInfo);
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return null;
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        public static bool IsMaxScrolled(GridView gv)
        {
            DevExpress.XtraGrid.Scrolling.ScrollInfo scrollInfo = null;
            try
            {
                scrollInfo = GetScrollInfo(gv);
                if (scrollInfo != null)
                {
                    ScrollBarViewInfo info2 = scrollInfo.VScroll.GetType().GetProperty("ViewInfo", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(scrollInfo.VScroll, null) as ScrollBarViewInfo;
                    return (info2.VisibleValue == info2.VisibleMaximum);
                }
                return true;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
                return false;
            }
        }

        public static void RefreshView(GridView view)
        {
            try
            {
                if (((view.GridControl != null) && !view.GridControl.Disposing) && !view.GridControl.IsDisposed)
                {
                    List<int> list = new List<int>();
                    for (int i = 0; i < view.RowCount; i++)
                    {
                        int visibleRowHandle = view.GetVisibleRowHandle(i);
                        if ((visibleRowHandle >= 0) && !view.GetMasterRowExpanded(visibleRowHandle))
                        {
                            EventLog.WriteLine("Row {0} is collapsed", new object[] { view.GetRow(visibleRowHandle) });
                            list.Add(view.GetRow(visibleRowHandle).GetHashCode());
                        }
                    }
                    view.RefreshData();
                    bool flag = false;
                    for (int j = 0; j < list.Count; j++)
                    {
                        for (int k = 0; k < view.RowCount; k++)
                        {
                            int rowHandle = view.GetVisibleRowHandle(k);
                            if (view.GetRow(rowHandle).GetHashCode() == list[j])
                            {
                                EventLog.WriteLine("Re-collapsing row {0}", new object[] { view.GetRow(rowHandle) });
                                view.CollapseMasterRow(k);
                                flag = true;
                            }
                        }
                    }
                    if (!view.IsFirstRow && (view.DetailLevel != 0))
                    {
                        view.MoveFirst();
                        flag = true;
                    }
                    if (flag)
                    {
                        view.Invalidate();
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public static void ScrollBarClick(GridView gv, MouseEventHandler handler)
        {
            try
            {
                GetScrollInfo(gv).VScroll.MouseDown += handler;
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public static void ScrollBarFocus(GridView gv)
        {
            try
            {
                GetScrollInfo(gv).VScroll.Focus();
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public static void ScrollToBottom(GridView gv)
        {
            DevExpress.XtraGrid.Scrolling.ScrollInfo scrollInfo = null;
            try
            {
                scrollInfo = GetScrollInfo(gv);
                if (scrollInfo != null)
                {
                    scrollInfo.VScroll.Value = scrollInfo.VScroll.Maximum;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public static void ScrollToTop(GridView gv)
        {
            DevExpress.XtraGrid.Scrolling.ScrollInfo scrollInfo = null;
            try
            {
                scrollInfo = GetScrollInfo(gv);
                if (scrollInfo != null)
                {
                    scrollInfo.VScroll.Value = scrollInfo.VScroll.Minimum;
                }
            }
            catch (Exception exception)
            {
                ErrorLog.WriteLine(exception);
            }
        }

        public DevExpress.XtraGrid.Scrolling.ScrollInfo ScrollInfo
        {
            get
            {
                return base.ScrollInfo;
            }
        }
    }
}

