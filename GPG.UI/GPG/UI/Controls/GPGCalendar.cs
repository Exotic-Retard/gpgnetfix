namespace GPG.UI.Controls
{
    using GPG;
    using GPG.UI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class GPGCalendar : Control
    {
        private IContainer components;
        public List<CalendarDate> Dates = new List<CalendarDate>();
        private Bitmap mBackground;
        private int mDay = DateTime.Now.Day;
        private List<string> mDays = new List<string>();
        private MouseEventArgs mLastArgs = new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0);
        private int mLeftMonthButton;
        [NonSerialized]
        private int mMonth = DateTime.Now.Month;
        private List<string> mMonths = new List<string>();
        public List<Image> MontlyBackgrounds = new List<Image>();
        private int mRefreshButton;
        private int mRightMonthButton;
        private int mRightMonthButtonLeft;
        private int mYear = DateTime.Now.Year;

        public event EventHandler OnRefreshClick;

        public GPGCalendar()
        {
            this.InitializeComponent();
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            base.SetStyle(ControlStyles.ContainerControl, false);
            this.mDays.Add(Loc.Get("<LOC>Sunday"));
            this.mDays.Add(Loc.Get("<LOC>Monday"));
            this.mDays.Add(Loc.Get("<LOC>Tuesday"));
            this.mDays.Add(Loc.Get("<LOC>Wednesday"));
            this.mDays.Add(Loc.Get("<LOC>Thursday"));
            this.mDays.Add(Loc.Get("<LOC>Friday"));
            this.mDays.Add(Loc.Get("<LOC>Saturday"));
            this.mMonths.Add(Loc.Get("<LOC>January"));
            this.mMonths.Add(Loc.Get("<LOC>February"));
            this.mMonths.Add(Loc.Get("<LOC>March"));
            this.mMonths.Add(Loc.Get("<LOC>April"));
            this.mMonths.Add(Loc.Get("<LOC>May"));
            this.mMonths.Add(Loc.Get("<LOC>June"));
            this.mMonths.Add(Loc.Get("<LOC>July"));
            this.mMonths.Add(Loc.Get("<LOC>August"));
            this.mMonths.Add(Loc.Get("<LOC>September"));
            this.mMonths.Add(Loc.Get("<LOC>October"));
            this.mMonths.Add(Loc.Get("<LOC>November"));
            this.mMonths.Add(Loc.Get("<LOC>December"));
        }

        private void CheckMonthButtons(MouseEventArgs e)
        {
            if (((e.X > 0) && (e.X < 30)) && ((e.Y > 5) && (e.Y < 0x23)))
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.LeftMonthButton = 2;
                    this.Month--;
                    this.RecalcBackground();
                    base.Invalidate();
                }
                else
                {
                    this.LeftMonthButton = 1;
                }
            }
            else
            {
                this.LeftMonthButton = 0;
            }
            if (((e.X > this.mRightMonthButtonLeft) && (e.X < (this.mRightMonthButtonLeft + 30))) && ((e.Y > 5) && (e.Y < 0x23)))
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.RightMonthButton = 2;
                    this.Month++;
                    this.RecalcBackground();
                    base.Invalidate();
                }
                else
                {
                    this.RightMonthButton = 1;
                }
            }
            else
            {
                this.RightMonthButton = 0;
            }
            if (((e.X > (base.Right - 40)) && (e.X < (base.Right - 10))) && ((e.Y > 5) && (e.Y < 0x23)))
            {
                if (e.Button != MouseButtons.Left)
                {
                    this.RefreshButton = 1;
                }
                else
                {
                    this.RefreshButton = 2;
                    base.Invalidate();
                    if (this.OnRefreshClick != null)
                    {
                        this.OnRefreshClick(this, EventArgs.Empty);
                    }
                }
            }
            else
            {
                this.RefreshButton = 0;
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

        private int FindDay(DateTime firstday)
        {
            if (firstday.DayOfWeek == DayOfWeek.Sunday)
            {
                return 0;
            }
            if (firstday.DayOfWeek == DayOfWeek.Monday)
            {
                return 1;
            }
            if (firstday.DayOfWeek == DayOfWeek.Tuesday)
            {
                return 2;
            }
            if (firstday.DayOfWeek == DayOfWeek.Wednesday)
            {
                return 3;
            }
            if (firstday.DayOfWeek == DayOfWeek.Thursday)
            {
                return 4;
            }
            if (firstday.DayOfWeek == DayOfWeek.Friday)
            {
                return 5;
            }
            return 6;
        }

        private Image GetBackground()
        {
            if (this.mBackground == null)
            {
                this.RecalcBackground();
            }
            return this.mBackground;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                int num = DateTime.DaysInMonth(this.mYear, this.mMonth);
                DateTime firstday = new DateTime(this.mYear, this.mMonth, 1);
                int num2 = this.FindDay(firstday);
                int num3 = 0;
                float num4 = ((float) base.Width) / 7f;
                float num5 = ((base.Height - 16f) - 40f) / 5f;
                for (int i = 1; i <= num; i++)
                {
                    float num7 = num2 * num4;
                    float num8 = ((num3 * num5) + 16f) + 40f;
                    float num9 = num7 + num4;
                    float num10 = num8 + num5;
                    if (((e.X > num7) && (e.X < num9)) && ((e.Y > num8) && (e.Y < num10)))
                    {
                        this.mDay = i;
                        base.Invalidate();
                        break;
                    }
                    num2++;
                    if (num2 > 6)
                    {
                        num2 = 0;
                        num3++;
                        if (num3 >= 5)
                        {
                            num3 = 0;
                        }
                    }
                }
            }
            this.CheckMonthButtons(e);
            this.mLastArgs = e;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button != MouseButtons.Left)
            {
                this.CheckMonthButtons(e);
            }
            this.mLastArgs = e;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics graphics = pe.Graphics;
            graphics.Clear(Color.Blue);
            Pen pen = new Pen(Color.Black);
            Brush brush = new SolidBrush(Color.White);
            Brush brush2 = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
            Brush brush3 = new SolidBrush(Color.Black);
            Brush brush4 = new SolidBrush(Color.FromArgb(0x98, 0x9a, 0xff));
            Font font = new Font(this.Font, FontStyle.Bold);
            Font font2 = new Font("Arial Black", 10f, FontStyle.Regular);
            Font font3 = new Font("Arial Black", 16f, FontStyle.Regular);
            Font font4 = new Font(this.Font.FontFamily, 7f, FontStyle.Regular);
            Font font5 = new Font(this.Font.FontFamily, 7f, FontStyle.Underline);
            Pen pen2 = new Pen(brush4, 2f);
            Brush brush5 = new LinearGradientBrush(new Point(0, 0), new Point(0, 80), Color.FromArgb(0x34, 0x35, 0x51), Color.Black);
            Rectangle rect = new Rectangle(0, 40, base.Width, base.Height - 40);
            Rectangle rectangle2 = new Rectangle(0, 0, base.Width, 40);
            graphics.FillRectangle(brush5, rectangle2);
            graphics.DrawString(this.mMonths[this.mMonth - 1] + " " + this.mYear.ToString(), font3, brush, (float) 30f, (float) 4f);
            this.mRightMonthButtonLeft = ((int) graphics.MeasureString(this.mMonths[this.mMonth - 1] + " " + this.mYear.ToString(), font3).Width) + 30;
            if (this.LeftMonthButton == 0)
            {
                graphics.DrawImage(GPGResources.btn_previous_up, 0, 5);
            }
            else if (this.LeftMonthButton == 1)
            {
                graphics.DrawImage(GPGResources.btn_previous_over, 0, 5);
            }
            else
            {
                graphics.DrawImage(GPGResources.btn_previous_down, 0, 5);
            }
            if (this.RightMonthButton == 0)
            {
                graphics.DrawImage(GPGResources.btn_next_up, this.mRightMonthButtonLeft, 5);
            }
            else if (this.RightMonthButton == 1)
            {
                graphics.DrawImage(GPGResources.btn_next_over, this.mRightMonthButtonLeft, 5);
            }
            else
            {
                graphics.DrawImage(GPGResources.btn_next_down, this.mRightMonthButtonLeft, 5);
            }
            if (this.RefreshButton == 0)
            {
                graphics.DrawImage(GPGResources.btn_refresh_up, base.Right - 40, 5);
            }
            else if (this.RefreshButton == 1)
            {
                graphics.DrawImage(GPGResources.btn_refresh_over, base.Right - 40, 5);
            }
            else
            {
                graphics.DrawImage(GPGResources.btn_refresh_down, base.Right - 40, 5);
            }
            float width = ((float) base.Width) / 7f;
            if (this.BackgroundImage != null)
            {
                graphics.DrawImage(this.GetBackground(), rect);
            }
            else
            {
                graphics.FillRectangle(brush, rect);
            }
            Rectangle rectangle3 = new Rectangle(0, 0x29, base.Width + 300, 15);
            graphics.DrawImage(GPGResources.calendardaybg, rectangle3);
            for (float i = 0f; i <= base.Width; i += width)
            {
                graphics.DrawLine(pen, i, 40f, i, (float) base.Height);
            }
            graphics.DrawLine(pen, 0, 40, base.Width, 40);
            float height = ((base.Height - 16f) - 40f) / 5f;
            for (float j = 56f; j <= base.Height; j += height)
            {
                graphics.DrawLine(pen, 0f, j, (float) base.Width, j);
            }
            for (int k = 0; k < 7; k++)
            {
                float x = k * width;
                RectangleF layoutRectangle = new RectangleF(x, 41f, width, 15f);
                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                graphics.DrawString(this.mDays[k], font, brush, layoutRectangle, format);
            }
            int num7 = DateTime.DaysInMonth(this.mYear, this.mMonth);
            DateTime firstday = new DateTime(this.mYear, this.mMonth, 1);
            int num8 = this.FindDay(firstday);
            int num9 = 0;
            for (int m = 1; m <= num7; m++)
            {
                float num11 = num8 * width;
                float y = ((num9 * height) + 16f) + 40f;
                int num13 = 0;
                foreach (CalendarDate date in this.Dates)
                {
                    if (((date.Date.Year != this.mYear) || (date.Date.Month != this.mMonth)) || (date.Date.Day != m))
                    {
                        continue;
                    }
                    if (num13 == 0)
                    {
                        graphics.FillRectangle(brush2, num11, y, width, height);
                    }
                    StringFormat format2 = new StringFormat();
                    RectangleF ef2 = new RectangleF(num11, (((y + (num13 * font4.Size)) + (num13 * 4f)) + font2.Size) + 8f, width, font4.Size * 2f);
                    if (ef2.Bottom < (y + height))
                    {
                        if (date.HighlightColor != Color.Empty)
                        {
                            Brush brush6 = new SolidBrush(date.HighlightColor);
                            graphics.DrawString(date.Label, font5, brush6, ef2, format2);
                            brush6.Dispose();
                        }
                        else
                        {
                            graphics.DrawString(date.Label, font4, brush, ef2, format2);
                        }
                    }
                    float num14 = ((num11 + width) - 8f) - (num13 * 4f);
                    graphics.DrawImage(GPGResources.calendargreenblip, num14, y + 2f);
                    num13++;
                }
                graphics.DrawString(m.ToString(), font2, brush3, (float) (num11 + 1f), (float) (y + 1f));
                graphics.DrawString(m.ToString(), font2, brush3, (float) (num11 + 1f), (float) (y - 1f));
                graphics.DrawString(m.ToString(), font2, brush3, (float) (num11 - 1f), (float) (y + 1f));
                graphics.DrawString(m.ToString(), font2, brush3, (float) (num11 - 1f), (float) (y - 1f));
                if (this.mDay == m)
                {
                    graphics.DrawString(m.ToString(), font2, brush4, num11, y);
                    graphics.DrawRectangle(pen2, num11, y, width, height);
                }
                else
                {
                    graphics.DrawString(m.ToString(), font2, brush, num11, y);
                }
                num8++;
                if (num8 > 6)
                {
                    num8 = 0;
                    num9++;
                    if (num9 >= 5)
                    {
                        num9 = 0;
                    }
                }
            }
            font3.Dispose();
            pen2.Dispose();
            brush4.Dispose();
            font5.Dispose();
            font4.Dispose();
            font2.Dispose();
            font.Dispose();
            brush.Dispose();
            brush2.Dispose();
            pen.Dispose();
            brush5.Dispose();
            brush3.Dispose();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.RecalcBackground();
            base.Invalidate();
        }

        private void RecalcBackground()
        {
            try
            {
                if (base.Visible)
                {
                    Bitmap image = new Bitmap(base.Width, base.Height - 40);
                    Graphics graphics = Graphics.FromImage(image);
                    if (this.MontlyBackgrounds.Count > (this.mMonth - 1))
                    {
                        graphics.DrawImage(this.MontlyBackgrounds[this.mMonth - 1], new Rectangle(0, 0, image.Width, image.Height));
                    }
                    else
                    {
                        graphics.DrawImage(this.BackgroundImage, new Rectangle(0, 0, image.Width, image.Height));
                    }
                    graphics.Dispose();
                    if (this.mBackground != null)
                    {
                        this.mBackground.Dispose();
                    }
                    this.mBackground = image;
                }
            }
            catch (Exception)
            {
            }
        }

        public void SetDate(DateTime date)
        {
            this.mMonth = date.Month;
            this.mDay = date.Day;
            this.mYear = date.Year;
        }

        public int LeftMonthButton
        {
            get
            {
                return this.mLeftMonthButton;
            }
            set
            {
                if (value != this.mLeftMonthButton)
                {
                    this.mLeftMonthButton = value;
                    base.Invalidate();
                }
            }
        }

        public int Month
        {
            get
            {
                return this.mMonth;
            }
            set
            {
                if (this.mMonth != value)
                {
                    this.mMonth = value;
                    if (this.mMonth < 1)
                    {
                        this.mMonth = 12;
                        this.mYear--;
                    }
                    if (this.mMonth > 12)
                    {
                        this.mMonth = 1;
                        this.mYear++;
                    }
                    base.Invalidate();
                }
            }
        }

        public int RefreshButton
        {
            get
            {
                return this.mRefreshButton;
            }
            set
            {
                if (value != this.mRefreshButton)
                {
                    this.mRefreshButton = value;
                    base.Invalidate();
                }
            }
        }

        public int RightMonthButton
        {
            get
            {
                return this.mRightMonthButton;
            }
            set
            {
                if (value != this.mRightMonthButton)
                {
                    this.mRightMonthButton = value;
                    base.Invalidate();
                }
            }
        }

        public List<CalendarDate> SelectedDates
        {
            get
            {
                List<CalendarDate> list = new List<CalendarDate>();
                foreach (CalendarDate date in this.Dates)
                {
                    if (((date.Date.Year == this.mYear) && (date.Date.Month == this.mMonth)) && (date.Date.Day == this.mDay))
                    {
                        list.Add(date);
                    }
                }
                return list;
            }
        }
    }
}

