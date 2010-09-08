namespace GPG.UI.Controls
{
    using GPG;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class GPGBracketGrid : Control
    {
        private IContainer components;
        public BindingList<GPGMatchup> Matchups = new BindingList<GPGMatchup>();
        private int mCurrentRound = 1;
        private List<List<GPGMatchup>> mRoundMatches = new List<List<GPGMatchup>>();

        public GPGBracketGrid()
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            base.SetStyle(ControlStyles.ContainerControl, false);
            this.Matchups.ListChanged += new ListChangedEventHandler(this.Matchups_ListChanged);
            this.mCurrentRound = 1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private string FormatRecord(GPGDragItem player)
        {
            return (player.Wins.ToString() + "-" + player.Losses.ToString() + "-" + player.Draws.ToString());
        }

        private Image GetFactionIcon(string faction)
        {
            if (faction.ToUpper() == "AEON")
            {
                return GPGResources.tinyaeon;
            }
            if (faction.ToUpper() == "CYBRAN")
            {
                return GPGResources.tinycybran;
            }
            if (faction.ToUpper() == "UEF")
            {
                return GPGResources.tinyuef;
            }
            return GPGResources.tinyrandom;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        private void Matchups_ListChanged(object sender, ListChangedEventArgs e)
        {
            this.mRoundMatches.Clear();
            foreach (GPGMatchup matchup in this.Matchups)
            {
                while (matchup.Round >= this.mRoundMatches.Count)
                {
                    this.mRoundMatches.Add(new List<GPGMatchup>());
                }
                this.mRoundMatches[matchup.Round - 1].Add(matchup);
            }
            base.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Graphics graphics = pe.Graphics;
            Brush brush = new SolidBrush(Color.White);
            Brush brush2 = new SolidBrush(Color.FromArgb(0xc2, 0xc2, 0xf2));
            Brush brush3 = new SolidBrush(Color.Black);
            Brush brush4 = new LinearGradientBrush(new Point(0, 0), new Point(0x105, 0), Color.FromArgb(0x37, 0x37, 0x37), Color.Black);
            Brush brush5 = new SolidBrush(Color.FromArgb(0x66, 0xcc, 0x66));
            Brush brush6 = new SolidBrush(Color.FromArgb(0xff, 0, 0));
            Brush brush7 = new SolidBrush(Color.FromArgb(190, 190, 190));
            Brush brush8 = new SolidBrush(Color.FromArgb(100, 100, 100));
            Pen pen = new Pen(brush2, 2f);
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            StringFormat format2 = new StringFormat();
            format2.LineAlignment = StringAlignment.Center;
            format2.Alignment = StringAlignment.Far;
            Font font = new Font("Verdana", 7f, FontStyle.Bold);
            Font font2 = new Font("Verdana", 7f);
            Font font3 = new Font("Verdana", 16f, FontStyle.Bold);
            Rectangle rect = new Rectangle(0, 30, base.Width, base.Height - 30);
            graphics.FillRectangle(brush3, rect);
            int x = 2;
            int y = 30;
            int num3 = 0;
            if (((this.mRoundMatches != null) && ((this.CurrentRound - 1) >= 0)) && ((this.CurrentRound - 1) < this.mRoundMatches.Count))
            {
                try
                {
                    foreach (GPGMatchup matchup in this.mRoundMatches[this.CurrentRound - 1])
                    {
                        if ((matchup.Player1 == null) && (matchup.Player2 == null))
                        {
                            continue;
                        }
                        Rectangle rectangle2 = new Rectangle(x, y, 250, 0x24);
                        Rectangle layoutRectangle = new Rectangle(x + 0x35, y, 150, 0x12);
                        Rectangle rectangle4 = new Rectangle(x + 0x35, y + 0x12, 150, 0x12);
                        Rectangle rectangle5 = new Rectangle(x + 5, y, 0x23, 0x12);
                        Rectangle rectangle6 = new Rectangle(x + 5, y + 0x12, 0x23, 0x12);
                        Rectangle rectangle7 = new Rectangle(x + 0x22, y + 1, 0x10, 0x10);
                        Rectangle rectangle8 = new Rectangle(x + 0x22, y + 0x13, 0x10, 0x10);
                        Rectangle rectangle9 = new Rectangle(x + 0xc5, y, 50, 0x12);
                        Rectangle rectangle10 = new Rectangle(x + 0xc5, y + 0x12, 50, 0x12);
                        Rectangle rectangle11 = new Rectangle(x + 0xba, y + 10, 14, 15);
                        Rectangle rectangle12 = new Rectangle(x + 0xa6, y + 10, 14, 15);
                        Brush brush9 = brush3;
                        if ((num3 % 2) == 0)
                        {
                            brush9 = brush4;
                        }
                        Brush brush10 = brush;
                        Brush brush11 = brush;
                        Font font4 = font2;
                        Font font5 = font2;
                        if (matchup.Player1 != null)
                        {
                            if (matchup.Player1.Won == 1)
                            {
                                brush10 = brush5;
                                font4 = font;
                            }
                            else if (matchup.Player1.Won == 0)
                            {
                                brush10 = brush6;
                            }
                        }
                        if (matchup.Player2 != null)
                        {
                            if (matchup.Player2.Won == 1)
                            {
                                brush11 = brush5;
                                font5 = font;
                            }
                            else if (matchup.Player2.Won == 0)
                            {
                                brush11 = brush6;
                            }
                        }
                        if (((matchup.Player1.Wins + matchup.Player1.Losses) + matchup.Player1.Draws) != this.mCurrentRound)
                        {
                            brush10 = brush8;
                        }
                        if (((matchup.Player2.Wins + matchup.Player2.Losses) + matchup.Player2.Draws) != this.mCurrentRound)
                        {
                            brush11 = brush8;
                        }
                        graphics.FillRectangle(brush9, rectangle2);
                        if (matchup.Player1 != null)
                        {
                            graphics.DrawString(matchup.Player1.PlayerName, font4, brush10, rectangle4, format);
                            graphics.DrawString(matchup.Player1.Seed.ToString(), font2, brush, rectangle6, format);
                            graphics.DrawString(this.FormatRecord(matchup.Player1), font2, brush7, rectangle10, format2);
                            graphics.DrawImage(this.GetFactionIcon(matchup.Player1.Faction), rectangle8);
                        }
                        if (matchup.Player2 != null)
                        {
                            graphics.DrawString(matchup.Player2.PlayerName, font5, brush11, layoutRectangle, format);
                            graphics.DrawString(matchup.Player2.Seed.ToString(), font2, brush, rectangle5, format);
                            graphics.DrawString(this.FormatRecord(matchup.Player2), font2, brush7, rectangle9, format2);
                            graphics.DrawImage(this.GetFactionIcon(matchup.Player2.Faction), rectangle7);
                        }
                        graphics.DrawImage(GPGResources.film, rectangle11);
                        graphics.DrawImage(GPGResources.stats, rectangle12);
                        y += 0x24;
                        if ((y + 0x24) > (base.Height - 2))
                        {
                            num3 = 0;
                            x += 260;
                            y = 30;
                        }
                        else
                        {
                            num3++;
                        }
                    }
                }
                catch
                {
                }
            }
            graphics.DrawString(Loc.Get("<LOC>Round") + " " + this.CurrentRound.ToString(), font3, brush, (float) 3f, (float) 3f);
            graphics.DrawRectangle(pen, 1, 0x1f, base.Width - 2, base.Height - 0x20);
            brush8.Dispose();
            font3.Dispose();
            format2.Dispose();
            brush7.Dispose();
            brush5.Dispose();
            brush6.Dispose();
            font.Dispose();
            font2.Dispose();
            format.Dispose();
            brush4.Dispose();
            pen.Dispose();
            brush2.Dispose();
            brush3.Dispose();
            brush.Dispose();
        }

        public void SetupRounds()
        {
            this.mRoundMatches.Clear();
            foreach (GPGMatchup matchup in this.Matchups)
            {
                while (matchup.Round >= this.mRoundMatches.Count)
                {
                    this.mRoundMatches.Add(new List<GPGMatchup>());
                }
                this.mRoundMatches[matchup.Round - 1].Add(matchup);
            }
            base.Invalidate();
        }

        private void SetupTestData()
        {
            for (int i = 1; i < 6; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    GPGMatchup matchup = new GPGMatchup();
                    matchup.Round = i;
                    GPGDragItem item = new GPGDragItem();
                    item.Label = "Player " + ((j * 2)).ToString();
                    item.Seed = j + 1;
                    item.Faction = "Aeon";
                    item.Won = 1;
                    matchup.Player1 = item;
                    item = new GPGDragItem();
                    item.Label = "Player " + (((j * 2) + 1)).ToString();
                    item.Seed = j + 0x33;
                    item.Faction = "UEF";
                    item.Won = 0;
                    matchup.Player2 = item;
                    this.Matchups.Add(matchup);
                }
            }
        }

        public int CurrentRound
        {
            get
            {
                if (this.mCurrentRound < 1)
                {
                    return 1;
                }
                if (((this.mRoundMatches != null) && (this.mCurrentRound > this.mRoundMatches.Count)) && (this.mRoundMatches.Count > 0))
                {
                    return this.mRoundMatches.Count;
                }
                return this.mCurrentRound;
            }
            set
            {
                if (value < 1)
                {
                    this.mCurrentRound = 1;
                }
                else if ((this.mRoundMatches != null) && (value > this.mRoundMatches.Count))
                {
                    this.mCurrentRound = this.mRoundMatches.Count;
                }
                else
                {
                    this.mCurrentRound = value;
                }
                if (this.mCurrentRound == 0)
                {
                    this.mCurrentRound = 1;
                }
                base.Invalidate();
            }
        }
    }
}

