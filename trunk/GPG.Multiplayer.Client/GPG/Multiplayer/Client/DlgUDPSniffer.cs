namespace GPG.Multiplayer.Client
{
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraGrid.Views.Base;
    using GPG;
    using GPG.Multiplayer.Game.Network;
    using GPG.Multiplayer.UI.Controls;
    using GPG.UI.Controls;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Text;
    using System.Windows.Forms;

    public class DlgUDPSniffer : DlgBase
    {
        private GPGButton btnBegin;
        private GPGButton btnEnd;
        private GPGButton btnTestPacket;
        private GridColumn colBytes;
        private GridColumn colData;
        private GridColumn colDestAddress;
        private GridColumn colDestPort;
        private GridColumn colPacketType;
        private GridColumn colSourceAddress;
        private GridColumn colSourcePort;
        private GridColumn colTime;
        private IContainer components = null;
        private GPGChatGrid gpgPacketGrid;
        private GridView gvPacketGrid;
        private BindingList<PacketInfo> mPackets = new BindingList<PacketInfo>();
        private NetListener mSniffer = new NetListener(0);
        private RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private RepositoryItemTimeEdit repositoryItemTimeEdit1;
        private RepositoryItemMemoEdit rimMemoEdit3;
        private RepositoryItemPictureEdit rimPictureEdit3;
        private RepositoryItemTextEdit rimTextEdit;

        public DlgUDPSniffer()
        {
            this.InitializeComponent();
            Loc.LocObject(this);
            this.btnEnd.Enabled = false;
            this.gpgPacketGrid.DataSource = this.mPackets;
            this.mSniffer.ByteMessageHandler += new ByteMessage(this.mSniffer_ByteMessageHandler);
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            this.btnBegin.Enabled = false;
            this.btnEnd.Enabled = true;
            this.mPackets.Clear();
            this.mSniffer.StartServer();
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            this.btnBegin.Enabled = true;
            this.btnEnd.Enabled = false;
            this.mSniffer.StopServer();
        }

        private void btnTestPacket_Click(object sender, EventArgs e)
        {
            this.mSniffer.SendMessage(Encoding.ASCII.GetBytes("This is a test."), "localhost", 0x17e0);
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
            this.gpgPacketGrid = new GPGChatGrid();
            this.gvPacketGrid = new GridView();
            this.colSourceAddress = new GridColumn();
            this.colSourcePort = new GridColumn();
            this.colDestAddress = new GridColumn();
            this.colDestPort = new GridColumn();
            this.colData = new GridColumn();
            this.repositoryItemMemoEdit1 = new RepositoryItemMemoEdit();
            this.colTime = new GridColumn();
            this.repositoryItemTimeEdit1 = new RepositoryItemTimeEdit();
            this.colBytes = new GridColumn();
            this.colPacketType = new GridColumn();
            this.rimPictureEdit3 = new RepositoryItemPictureEdit();
            this.rimTextEdit = new RepositoryItemTextEdit();
            this.rimMemoEdit3 = new RepositoryItemMemoEdit();
            this.btnEnd = new GPGButton();
            this.btnBegin = new GPGButton();
            this.btnTestPacket = new GPGButton();
            this.gpgPacketGrid.BeginInit();
            this.gvPacketGrid.BeginInit();
            this.repositoryItemMemoEdit1.BeginInit();
            this.repositoryItemTimeEdit1.BeginInit();
            this.rimPictureEdit3.BeginInit();
            this.rimTextEdit.BeginInit();
            this.rimMemoEdit3.BeginInit();
            base.SuspendLayout();
            this.gpgPacketGrid.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.gpgPacketGrid.EmbeddedNavigator.Name = "";
            this.gpgPacketGrid.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gpgPacketGrid.IgnoreMouseWheel = false;
            this.gpgPacketGrid.Location = new Point(12, 0x53);
            this.gpgPacketGrid.LookAndFeel.SkinName = "Money Twins";
            this.gpgPacketGrid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gpgPacketGrid.MainView = this.gvPacketGrid;
            this.gpgPacketGrid.Name = "gpgPacketGrid";
            this.gpgPacketGrid.RepositoryItems.AddRange(new RepositoryItem[] { this.rimPictureEdit3, this.rimTextEdit, this.rimMemoEdit3, this.repositoryItemMemoEdit1, this.repositoryItemTimeEdit1 });
            this.gpgPacketGrid.Size = new Size(0x32d, 350);
            this.gpgPacketGrid.TabIndex = 12;
            this.gpgPacketGrid.ViewCollection.AddRange(new BaseView[] { this.gvPacketGrid });
            this.gvPacketGrid.ActiveFilterString = "";
            this.gvPacketGrid.Appearance.ColumnFilterButton.BackColor = Color.Black;
            this.gvPacketGrid.Appearance.ColumnFilterButton.BackColor2 = Color.FromArgb(20, 20, 20);
            this.gvPacketGrid.Appearance.ColumnFilterButton.BorderColor = Color.Black;
            this.gvPacketGrid.Appearance.ColumnFilterButton.ForeColor = Color.Gray;
            this.gvPacketGrid.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvPacketGrid.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.ColumnFilterButtonActive.BackColor = Color.FromArgb(20, 20, 20);
            this.gvPacketGrid.Appearance.ColumnFilterButtonActive.BackColor2 = Color.FromArgb(0x4e, 0x4e, 0x4e);
            this.gvPacketGrid.Appearance.ColumnFilterButtonActive.BorderColor = Color.FromArgb(20, 20, 20);
            this.gvPacketGrid.Appearance.ColumnFilterButtonActive.ForeColor = Color.Blue;
            this.gvPacketGrid.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvPacketGrid.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.Empty.BackColor = Color.Black;
            this.gvPacketGrid.Appearance.Empty.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.FilterCloseButton.BackColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvPacketGrid.Appearance.FilterCloseButton.BackColor2 = Color.FromArgb(90, 90, 90);
            this.gvPacketGrid.Appearance.FilterCloseButton.BorderColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvPacketGrid.Appearance.FilterCloseButton.ForeColor = Color.Black;
            this.gvPacketGrid.Appearance.FilterCloseButton.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvPacketGrid.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvPacketGrid.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.FilterPanel.BackColor = Color.Black;
            this.gvPacketGrid.Appearance.FilterPanel.BackColor2 = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvPacketGrid.Appearance.FilterPanel.ForeColor = Color.White;
            this.gvPacketGrid.Appearance.FilterPanel.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvPacketGrid.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.FixedLine.BackColor = Color.FromArgb(0x3a, 0x3a, 0x3a);
            this.gvPacketGrid.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.FocusedCell.BackColor = Color.Black;
            this.gvPacketGrid.Appearance.FocusedCell.Font = new Font("Tahoma", 10f);
            this.gvPacketGrid.Appearance.FocusedCell.ForeColor = Color.White;
            this.gvPacketGrid.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.FocusedCell.Options.UseFont = true;
            this.gvPacketGrid.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.FocusedRow.BackColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gvPacketGrid.Appearance.FocusedRow.BackColor2 = Color.FromArgb(0x52, 0x83, 190);
            this.gvPacketGrid.Appearance.FocusedRow.Font = new Font("Arial", 9.75f);
            this.gvPacketGrid.Appearance.FocusedRow.ForeColor = Color.White;
            this.gvPacketGrid.Appearance.FocusedRow.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvPacketGrid.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.FocusedRow.Options.UseFont = true;
            this.gvPacketGrid.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.FooterPanel.BackColor = Color.Black;
            this.gvPacketGrid.Appearance.FooterPanel.BorderColor = Color.Black;
            this.gvPacketGrid.Appearance.FooterPanel.Font = new Font("Tahoma", 10f);
            this.gvPacketGrid.Appearance.FooterPanel.ForeColor = Color.White;
            this.gvPacketGrid.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvPacketGrid.Appearance.FooterPanel.Options.UseFont = true;
            this.gvPacketGrid.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.GroupButton.BackColor = Color.Black;
            this.gvPacketGrid.Appearance.GroupButton.BorderColor = Color.Black;
            this.gvPacketGrid.Appearance.GroupButton.ForeColor = Color.White;
            this.gvPacketGrid.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvPacketGrid.Appearance.GroupButton.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.GroupFooter.BackColor = Color.FromArgb(10, 10, 10);
            this.gvPacketGrid.Appearance.GroupFooter.BorderColor = Color.FromArgb(10, 10, 10);
            this.gvPacketGrid.Appearance.GroupFooter.ForeColor = Color.White;
            this.gvPacketGrid.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvPacketGrid.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.GroupPanel.BackColor = Color.Black;
            this.gvPacketGrid.Appearance.GroupPanel.BackColor2 = Color.Black;
            this.gvPacketGrid.Appearance.GroupPanel.Font = new Font("Tahoma", 10f, FontStyle.Bold);
            this.gvPacketGrid.Appearance.GroupPanel.ForeColor = Color.White;
            this.gvPacketGrid.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.GroupPanel.Options.UseFont = true;
            this.gvPacketGrid.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.GroupRow.BackColor = Color.Gray;
            this.gvPacketGrid.Appearance.GroupRow.Font = new Font("Tahoma", 10f);
            this.gvPacketGrid.Appearance.GroupRow.ForeColor = Color.White;
            this.gvPacketGrid.Appearance.GroupRow.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.GroupRow.Options.UseFont = true;
            this.gvPacketGrid.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.HeaderPanel.BackColor = Color.Black;
            this.gvPacketGrid.Appearance.HeaderPanel.BorderColor = Color.Black;
            this.gvPacketGrid.Appearance.HeaderPanel.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.gvPacketGrid.Appearance.HeaderPanel.ForeColor = Color.Black;
            this.gvPacketGrid.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvPacketGrid.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvPacketGrid.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.HideSelectionRow.BackColor = Color.Gray;
            this.gvPacketGrid.Appearance.HideSelectionRow.Font = new Font("Tahoma", 10f);
            this.gvPacketGrid.Appearance.HideSelectionRow.ForeColor = Color.FromArgb(0xd4, 0xd0, 200);
            this.gvPacketGrid.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvPacketGrid.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.HorzLine.BackColor = Color.FromArgb(0x52, 0x83, 190);
            this.gvPacketGrid.Appearance.HorzLine.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.Preview.BackColor = Color.White;
            this.gvPacketGrid.Appearance.Preview.Font = new Font("Tahoma", 10f);
            this.gvPacketGrid.Appearance.Preview.ForeColor = Color.Purple;
            this.gvPacketGrid.Appearance.Preview.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.Preview.Options.UseFont = true;
            this.gvPacketGrid.Appearance.Preview.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.Row.BackColor = Color.Black;
            this.gvPacketGrid.Appearance.Row.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0xb2);
            this.gvPacketGrid.Appearance.Row.ForeColor = Color.White;
            this.gvPacketGrid.Appearance.Row.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.Row.Options.UseFont = true;
            this.gvPacketGrid.Appearance.Row.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.RowSeparator.BackColor = Color.White;
            this.gvPacketGrid.Appearance.RowSeparator.BackColor2 = Color.White;
            this.gvPacketGrid.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.SelectedRow.BackColor = Color.FromArgb(0xbb, 0xc9, 0xe2);
            this.gvPacketGrid.Appearance.SelectedRow.BackColor2 = Color.FromArgb(0x52, 0x83, 190);
            this.gvPacketGrid.Appearance.SelectedRow.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.gvPacketGrid.Appearance.SelectedRow.ForeColor = Color.White;
            this.gvPacketGrid.Appearance.SelectedRow.GradientMode = LinearGradientMode.ForwardDiagonal;
            this.gvPacketGrid.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvPacketGrid.Appearance.SelectedRow.Options.UseFont = true;
            this.gvPacketGrid.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.TopNewRow.Font = new Font("Tahoma", 10f);
            this.gvPacketGrid.Appearance.TopNewRow.ForeColor = Color.White;
            this.gvPacketGrid.Appearance.TopNewRow.Options.UseFont = true;
            this.gvPacketGrid.Appearance.TopNewRow.Options.UseForeColor = true;
            this.gvPacketGrid.Appearance.VertLine.BackColor = Color.FromArgb(0x52, 0x83, 190);
            this.gvPacketGrid.Appearance.VertLine.Options.UseBackColor = true;
            this.gvPacketGrid.BorderStyle = BorderStyles.NoBorder;
            this.gvPacketGrid.Columns.AddRange(new GridColumn[] { this.colSourceAddress, this.colSourcePort, this.colDestAddress, this.colDestPort, this.colData, this.colTime, this.colBytes, this.colPacketType });
            this.gvPacketGrid.GridControl = this.gpgPacketGrid;
            this.gvPacketGrid.Name = "gvPacketGrid";
            this.gvPacketGrid.OptionsDetail.AllowZoomDetail = false;
            this.gvPacketGrid.OptionsDetail.EnableMasterViewMode = false;
            this.gvPacketGrid.OptionsDetail.ShowDetailTabs = false;
            this.gvPacketGrid.OptionsDetail.SmartDetailExpand = false;
            this.gvPacketGrid.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvPacketGrid.OptionsSelection.MultiSelect = true;
            this.gvPacketGrid.OptionsView.RowAutoHeight = true;
            this.colSourceAddress.Caption = "Source Address";
            this.colSourceAddress.FieldName = "SourceAddress";
            this.colSourceAddress.Name = "colSourceAddress";
            this.colSourceAddress.OptionsColumn.AllowEdit = false;
            this.colSourceAddress.Visible = true;
            this.colSourceAddress.VisibleIndex = 1;
            this.colSourceAddress.Width = 0x73;
            this.colSourcePort.Caption = "Source Port";
            this.colSourcePort.FieldName = "SourcePort";
            this.colSourcePort.Name = "colSourcePort";
            this.colSourcePort.OptionsColumn.AllowEdit = false;
            this.colSourcePort.Visible = true;
            this.colSourcePort.VisibleIndex = 2;
            this.colSourcePort.Width = 0x60;
            this.colDestAddress.Caption = "Dest Address";
            this.colDestAddress.FieldName = "DestAddress";
            this.colDestAddress.Name = "colDestAddress";
            this.colDestAddress.OptionsColumn.AllowEdit = false;
            this.colDestAddress.Visible = true;
            this.colDestAddress.VisibleIndex = 3;
            this.colDestAddress.Width = 0x63;
            this.colDestPort.Caption = "Dest Port";
            this.colDestPort.FieldName = "DestPort";
            this.colDestPort.Name = "colDestPort";
            this.colDestPort.OptionsColumn.AllowEdit = false;
            this.colDestPort.Visible = true;
            this.colDestPort.VisibleIndex = 4;
            this.colDestPort.Width = 0x5e;
            this.colData.Caption = "Data";
            this.colData.ColumnEdit = this.repositoryItemMemoEdit1;
            this.colData.FieldName = "Data";
            this.colData.Name = "colData";
            this.colData.OptionsColumn.AllowEdit = false;
            this.colData.Visible = true;
            this.colData.VisibleIndex = 5;
            this.colData.Width = 0x188;
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            this.colTime.Caption = "Time";
            this.colTime.ColumnEdit = this.repositoryItemTimeEdit1;
            this.colTime.FieldName = "Time";
            this.colTime.Name = "colTime";
            this.colTime.OptionsColumn.AllowEdit = false;
            this.colTime.Visible = true;
            this.colTime.VisibleIndex = 0;
            this.repositoryItemTimeEdit1.AutoHeight = false;
            this.repositoryItemTimeEdit1.Buttons.AddRange(new EditorButton[] { new EditorButton() });
            this.repositoryItemTimeEdit1.Name = "repositoryItemTimeEdit1";
            this.colBytes.Caption = "Byte List";
            this.colBytes.ColumnEdit = this.repositoryItemMemoEdit1;
            this.colBytes.FieldName = "ByteList";
            this.colBytes.Name = "colBytes";
            this.colBytes.OptionsColumn.AllowEdit = false;
            this.colPacketType.Caption = "Packet Type";
            this.colPacketType.FieldName = "PacketType";
            this.colPacketType.Name = "colPacketType";
            this.colPacketType.OptionsColumn.AllowEdit = false;
            this.rimPictureEdit3.Name = "rimPictureEdit3";
            this.rimPictureEdit3.PictureAlignment = ContentAlignment.TopCenter;
            this.rimTextEdit.AutoHeight = false;
            this.rimTextEdit.Name = "rimTextEdit";
            this.rimMemoEdit3.MaxLength = 500;
            this.rimMemoEdit3.Name = "rimMemoEdit3";
            this.btnEnd.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnEnd.ForeColor = Color.Black;
            this.btnEnd.Location = new Point(0x259, 0x1b7);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new Size(0x68, 0x17);
            this.btnEnd.TabIndex = 0x1a;
            this.btnEnd.Text = "End Sniffing";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new EventHandler(this.btnEnd_Click);
            this.btnBegin.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnBegin.ForeColor = Color.Black;
            this.btnBegin.Location = new Point(0x2c7, 0x1b7);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new Size(0x68, 0x17);
            this.btnBegin.TabIndex = 0x19;
            this.btnBegin.Text = "Begin Sniffing";
            this.btnBegin.UseVisualStyleBackColor = true;
            this.btnBegin.Click += new EventHandler(this.btnBegin_Click);
            this.btnTestPacket.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnTestPacket.ForeColor = Color.Black;
            this.btnTestPacket.Location = new Point(0x1eb, 0x1b7);
            this.btnTestPacket.Name = "btnTestPacket";
            this.btnTestPacket.Size = new Size(0x68, 0x17);
            this.btnTestPacket.TabIndex = 0x1b;
            this.btnTestPacket.Text = "Test";
            this.btnTestPacket.UseVisualStyleBackColor = true;
            this.btnTestPacket.Click += new EventHandler(this.btnTestPacket_Click);
            base.AutoScaleDimensions = new SizeF(7f, 16f);
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(0x345, 0x214);
            base.Controls.Add(this.btnTestPacket);
            base.Controls.Add(this.btnEnd);
            base.Controls.Add(this.btnBegin);
            base.Controls.Add(this.gpgPacketGrid);
            base.Location = new Point(0, 0);
            base.Name = "DlgUDPSniffer";
            this.Text = "Packet Sniffer";
            base.Controls.SetChildIndex(this.gpgPacketGrid, 0);
            base.Controls.SetChildIndex(this.btnBegin, 0);
            base.Controls.SetChildIndex(this.btnEnd, 0);
            base.Controls.SetChildIndex(this.btnTestPacket, 0);
            this.gpgPacketGrid.EndInit();
            this.gvPacketGrid.EndInit();
            this.repositoryItemMemoEdit1.EndInit();
            this.repositoryItemTimeEdit1.EndInit();
            this.rimPictureEdit3.EndInit();
            this.rimTextEdit.EndInit();
            this.rimMemoEdit3.EndInit();
            base.ResumeLayout(false);
        }

        private void mSniffer_ByteMessageHandler(string type, string srcaddr, int srcport, string destaddr, int destport, byte[] message)
        {
            VGen6 method = null;
            if (!base.Disposing && !base.IsDisposed)
            {
                if (method == null)
                {
                    method = delegate (object _type, object _srcaddr, object _srcport, object _destaddr, object _destport, object _message) {
                        PacketInfo item = new PacketInfo();
                        item.SetData(_message as byte[]);
                        item.DestAddress = _destaddr.ToString();
                        item.DestPort = (int) _destport;
                        item.SourceAddress = _srcaddr.ToString();
                        item.SourcePort = (int) _srcport;
                        item.PacketType = _type.ToString();
                        this.mPackets.Add(item);
                    };
                }
                base.BeginInvoke(method, new object[] { type, srcaddr, srcport, destaddr, destport, message });
            }
        }
    }
}

