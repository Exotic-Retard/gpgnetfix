namespace GPG.UI.Controls
{
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class GPGDragList : Control
    {
        private IContainer components;
        public List<GPGMatchup> DupeMatchups = new List<GPGMatchup>();
        public List<GPGDragItem> Items = new List<GPGDragItem>();
        private int matchheight = 0x26;
        public List<GPGMatchup> Matchups = new List<GPGMatchup>();
        private int matchwidth = 250;
        private Rectangle mDragRect = Rectangle.Empty;
        private string mHighlightPlayer;
        private int mItemHeight = 15;
        private int mItemWidth = 200;
        private bool mOverDrop;
        private int mOverItem;
        private int mOverX;
        private int mOverY;
        private GPGDragItem mSelectedItem;
        public GPGMatchup SelectedMatchup;
        public GPGDragState State = GPGDragState.NoMove;

        public GPGDragList()
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.SetStyle(ControlStyles.ContainerControl, false);
        }

        public void CheckMatchup()
        {
            int num = (this.Items.Count / 2) + (this.Items.Count % 2);
            while (this.Matchups.Count < num)
            {
                GPGMatchup item = new GPGMatchup();
                this.Matchups.Add(item);
            }
            while (this.Matchups.Count > num)
            {
                GPGMatchup matchup2 = this.Matchups[this.Matchups.Count - 1];
                if (matchup2.Player1 != null)
                {
                    matchup2.Player1.IsMatched = false;
                }
                if (matchup2.Player2 != null)
                {
                    matchup2.Player2.IsMatched = false;
                }
                this.Matchups.RemoveAt(this.Matchups.Count - 1);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if ((this.State != GPGDragState.NoMove) && (this.mSelectedItem == null))
            {
                if (((e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Right)) && (e.X < this.ItemWidth))
                {
                    int num = (int) Math.Truncate((double) (((float) e.Y) / ((float) this.ItemHeight)));
                    if (num < this.Items.Count)
                    {
                        int num2 = 0;
                        foreach (GPGDragItem item in this.Items)
                        {
                            if (!item.IsMatched)
                            {
                                if ((num2 == num) && !item.IsLocked)
                                {
                                    this.mSelectedItem = item;
                                }
                                num2++;
                            }
                        }
                        this.mDragRect = new Rectangle(e.X, e.Y, this.ItemWidth, this.ItemHeight);
                        base.Invalidate();
                    }
                }
                else if (e.Button == MouseButtons.Left)
                {
                    int num3 = 10;
                    int num4 = (this.Items.Count / 2) + (this.Items.Count % 2);
                    int num5 = (this.ItemWidth + 15) + num3;
                    int num6 = num3;
                    for (int i = 0; i < num4; i++)
                    {
                        if (((e.X > num5) && (e.X < (num5 + this.matchwidth))) && ((e.Y > num6) && (e.Y < (num6 + this.matchheight))))
                        {
                            GPGMatchup matchup = this.Matchups[i];
                            this.SelectedMatchup = matchup;
                            if (((matchup.Player1 != null) && (e.Y < ((num6 + 3) + this.ItemHeight))) && !matchup.Player1.IsLocked)
                            {
                                this.mSelectedItem = matchup.Player1;
                                matchup.Player1 = null;
                                this.mSelectedItem.IsMatched = false;
                                this.mDragRect = new Rectangle(e.X, e.Y, this.ItemWidth, this.ItemHeight);
                                this.mOverDrop = true;
                                this.mOverX = e.X;
                                this.mOverY = e.Y;
                                this.mOverItem = i;
                                base.Invalidate();
                            }
                            else if ((matchup.Player2 != null) && !matchup.Player2.IsLocked)
                            {
                                this.mSelectedItem = matchup.Player2;
                                matchup.Player2 = null;
                                this.mSelectedItem.IsMatched = false;
                                this.mDragRect = new Rectangle(e.X, e.Y, this.ItemWidth, this.ItemHeight);
                                this.mOverDrop = true;
                                this.mOverX = e.X;
                                this.mOverY = e.Y;
                                this.mOverItem = i;
                                base.Invalidate();
                            }
                        }
                        num5 = (num5 + this.matchwidth) + num3;
                        if (num5 > ((base.Width - this.matchwidth) - num3))
                        {
                            num5 = (this.ItemWidth + 15) + num3;
                            num6 = (num6 + this.matchheight) + num3;
                        }
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                if (this.mSelectedItem != null)
                {
                    this.mDragRect = new Rectangle(e.X, e.Y, this.ItemWidth, this.ItemHeight);
                    this.mOverDrop = false;
                    int num = 10;
                    int num2 = (this.Items.Count / 2) + (this.Items.Count % 2);
                    int num3 = (this.ItemWidth + 15) + num;
                    int num4 = num;
                    for (int i = 0; i < num2; i++)
                    {
                        if (((e.X > num3) && (e.X < (num3 + this.matchwidth))) && ((e.Y > num4) && (e.Y < (num4 + this.matchheight))))
                        {
                            this.mOverDrop = true;
                            this.mOverX = num3;
                            this.mOverY = num4;
                            this.mOverItem = i;
                        }
                        num3 = (num3 + this.matchwidth) + num;
                        if (num3 > ((base.Width - this.matchwidth) - num))
                        {
                            num3 = (this.ItemWidth + 15) + num;
                            num4 = (num4 + this.matchheight) + num;
                        }
                    }
                    base.Invalidate();
                }
            }
            else
            {
                this.mSelectedItem = null;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if ((this.mSelectedItem != null) && this.mOverDrop)
            {
                GPGMatchup matchup = this.Matchups[this.mOverItem];
                if (matchup.Player1 == null)
                {
                    matchup.Player1 = this.mSelectedItem;
                    this.mSelectedItem.IsMatched = true;
                }
                else if (matchup.Player2 == null)
                {
                    matchup.Player2 = this.mSelectedItem;
                    this.mSelectedItem.IsMatched = true;
                }
            }
            this.mDragRect = Rectangle.Empty;
            this.mSelectedItem = null;
            this.mOverDrop = false;
            base.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Graphics graphics = pe.Graphics;
            Brush brush = new SolidBrush(Color.Red);
            Brush brush2 = new SolidBrush(Color.Aqua);
            Brush brush3 = new SolidBrush(Color.White);
            Brush brush4 = new SolidBrush(Color.Lime);
            Brush brush5 = new SolidBrush(Color.Yellow);
            Brush brush6 = new SolidBrush(Color.FromArgb(0x7d, 0x7d, 0xff));
            Pen pen = new Pen(brush6);
            Pen pen2 = new Pen(brush6, 2f);
            Brush brush7 = new SolidBrush(Color.FromArgb(0x7d, 0, 0, 0));
            Font font = new Font("Verdana", 7f);
            int y = 0;
            foreach (GPGDragItem item in this.Items)
            {
                if (item.IsMatched)
                {
                    continue;
                }
                int num2 = 0;
                if (item.Icon != null)
                {
                    graphics.DrawImage(item.Icon, num2, y, this.ItemHeight, this.ItemHeight);
                    num2 += this.ItemHeight;
                }
                if (item == this.mSelectedItem)
                {
                    graphics.DrawString(item.Label, font, brush6, (float) num2, (float) y);
                }
                else if (item.LabelNoReport.ToUpper() == this.HighlightPlayer.ToUpper())
                {
                    graphics.DrawString(item.Label, font, brush4, (float) num2, (float) y);
                }
                else
                {
                    graphics.DrawString(item.Label, font, brush3, (float) num2, (float) y);
                }
                y += this.ItemHeight;
            }
            graphics.DrawLine(pen2, this.ItemWidth, 0, this.ItemWidth, base.Height);
            graphics.DrawLine(pen2, this.ItemWidth + 15, 0, this.ItemWidth + 15, base.Height);
            int num3 = 10;
            int num4 = (this.Items.Count / 2) + (this.Items.Count % 2);
            this.CheckMatchup();
            int x = (this.ItemWidth + 15) + num3;
            int num6 = num3;
            for (int i = 0; i < num4; i++)
            {
                GPGMatchup matchup = this.Matchups[i];
                Rectangle rect = new Rectangle(x, num6, this.matchwidth, this.matchheight);
                if ((this.mOverDrop && (this.mOverX == x)) && (this.mOverY == num6))
                {
                    if ((matchup.Player1 != null) && (matchup.Player2 != null))
                    {
                        graphics.DrawImage(GPGResources.matchupinvalid, rect);
                    }
                    else
                    {
                        graphics.DrawImage(GPGResources.matchupover, rect);
                    }
                }
                else if (matchup == this.SelectedMatchup)
                {
                    graphics.DrawImage(DrawUtil.AdjustColors(1f, 1f, 0f, GPGResources.matchup), rect);
                }
                else
                {
                    graphics.DrawImage(GPGResources.matchup, rect);
                }
                Brush brush8 = brush3;
                if ((matchup.Player1 != null) && (matchup.Player2 != null))
                {
                    if ((matchup.Player1.PlayerReport == "Disconnect") && (matchup.Player2.PlayerReport == "Disconnect"))
                    {
                        brush8 = brush;
                    }
                    if ((matchup.Player1.PlayerReport == "Failed Game") || (matchup.Player2.PlayerReport == "Failed Game"))
                    {
                        brush8 = brush;
                    }
                    if ((matchup.Player1.PlayerReport == "No Report") || (matchup.Player2.PlayerReport == "No Report"))
                    {
                        brush8 = brush2;
                    }
                    foreach (GPGMatchup matchup2 in this.DupeMatchups)
                    {
                        if (((matchup2 != matchup) && (matchup2.Player1 != null)) && (matchup2.Player1 != null))
                        {
                            if ((matchup2.Player1 == matchup.Player1) && (matchup2.Player2 == matchup.Player2))
                            {
                                brush8 = brush5;
                            }
                            if ((matchup2.Player2 == matchup.Player1) && (matchup2.Player1 == matchup.Player2))
                            {
                                brush8 = brush5;
                            }
                        }
                    }
                    if ((matchup.Player1.PlayerName.ToUpper() == this.HighlightPlayer.ToUpper()) || (matchup.Player2.PlayerName.ToUpper() == this.HighlightPlayer.ToUpper()))
                    {
                        brush8 = brush4;
                    }
                }
                if (matchup.Player1 != null)
                {
                    int num8 = x + 3;
                    if (matchup.Player1.Icon != null)
                    {
                        graphics.DrawImage(matchup.Player1.Icon, num8, num6 + 3, this.ItemHeight, this.ItemHeight);
                        num8 += this.ItemHeight;
                    }
                    graphics.DrawString(matchup.Player1.Label, font, brush8, (float) num8, (float) (num6 + 3));
                }
                if (matchup.Player2 != null)
                {
                    int num9 = x + 3;
                    if (matchup.Player2.Icon != null)
                    {
                        graphics.DrawImage(matchup.Player2.Icon, num9, (num6 + 3) + this.ItemHeight, this.ItemHeight, this.ItemHeight);
                        num9 += this.ItemHeight;
                    }
                    graphics.DrawString(matchup.Player2.Label, font, brush8, (float) num9, (float) ((num6 + 3) + this.ItemHeight));
                }
                x = (x + this.matchwidth) + num3;
                if (x > ((base.Width - this.matchwidth) - num3))
                {
                    x = (this.ItemWidth + 15) + num3;
                    num6 = (num6 + this.matchheight) + num3;
                }
            }
            if (this.mSelectedItem != null)
            {
                graphics.FillRectangle(brush7, this.mDragRect);
                graphics.DrawRectangle(pen, this.mDragRect);
                if (this.mSelectedItem.Icon != null)
                {
                    graphics.DrawImage(this.mSelectedItem.Icon, (int) (this.mDragRect.Left + 1), (int) (this.mDragRect.Top + 1), (int) (this.mDragRect.Height - 2), (int) (this.mDragRect.Height - 2));
                    graphics.DrawString(this.mSelectedItem.Label, font, brush6, (float) (this.mDragRect.Left + this.mDragRect.Height), (float) (this.mDragRect.Top + 1));
                }
                else
                {
                    graphics.DrawString(this.mSelectedItem.Label, font, brush6, this.mDragRect);
                }
            }
            brush5.Dispose();
            brush4.Dispose();
            brush2.Dispose();
            brush.Dispose();
            font.Dispose();
            brush7.Dispose();
            pen2.Dispose();
            pen.Dispose();
            brush6.Dispose();
            brush3.Dispose();
        }

        public void Resetmatchups()
        {
            this.Matchups.Clear();
            this.CheckMatchup();
            foreach (GPGDragItem item in this.Items)
            {
                item.IsMatched = false;
                item.InGame = false;
            }
        }

        public string HighlightPlayer
        {
            get
            {
                if (this.mHighlightPlayer == null)
                {
                    return "";
                }
                return this.mHighlightPlayer;
            }
            set
            {
                this.mHighlightPlayer = value;
                base.Invalidate();
            }
        }

        public int ItemHeight
        {
            get
            {
                return this.mItemHeight;
            }
            set
            {
                this.mItemHeight = value;
            }
        }

        public int ItemWidth
        {
            get
            {
                return this.mItemWidth;
            }
            set
            {
                this.mItemWidth = value;
            }
        }
    }
}

