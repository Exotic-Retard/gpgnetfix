namespace GPG.Multiplayer.Client.Games.SupremeCommander
{
    using DevExpress.Data;
    using DevExpress.XtraPivotGrid;
    using GPG.Multiplayer.Client;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Odbc;
    using System.Drawing;
    using System.Windows.Forms;

    public class DlgSupcomStats : DlgBase
    {
        private IContainer components;
        private OdbcConnection mConnection;
        private PivotGridControl pivotGridControl1;
        private PivotGridField pivotGridField1;
        private PivotGridField pivotGridField2;
        private PivotGridField pivotGridField3;
        private PivotGridField pivotGridField4;
        private PivotGridField pivotGridField5;
        private PivotGridField pivotGridField6;
        private PivotGridField pivotGridField7;

        public DlgSupcomStats()
        {
            DataRow current;
            this.components = null;
            this.mConnection = new OdbcConnection("DSN=gpgstats;");
            this.InitializeComponent();
            this.mConnection.Open();
            OdbcDataReader reader = new OdbcCommand("SELECT * FROM units_player", this.mConnection).ExecuteReader();
            DataTable table = new DataTable();
            using (IEnumerator<object> enumerator = (IEnumerator<object>)reader.GetSchemaTable().Rows.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    current = (DataRow) enumerator.Current;
                    table.Columns.Add(current[0].ToString(), (System.Type) current[5]);
                }
            }
            while (reader.Read())
            {
                current = table.NewRow();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    current[i] = reader[i];
                }
                table.Rows.Add(current);
            }
            reader.Close();
            this.mConnection.Close();
            this.pivotGridControl1.DataSource = table;
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
            this.pivotGridControl1 = new PivotGridControl();
            this.pivotGridField1 = new PivotGridField();
            this.pivotGridField2 = new PivotGridField();
            this.pivotGridField3 = new PivotGridField();
            this.pivotGridField4 = new PivotGridField();
            this.pivotGridField5 = new PivotGridField();
            this.pivotGridField6 = new PivotGridField();
            this.pivotGridField7 = new PivotGridField();
            this.pivotGridControl1.BeginInit();
            base.SuspendLayout();
            base.ttDefault.DefaultController.AutoPopDelay = 0x3e8;
            this.pivotGridControl1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.pivotGridControl1.Cursor = Cursors.Default;
            this.pivotGridControl1.Fields.AddRange(new PivotGridField[] { this.pivotGridField1, this.pivotGridField2, this.pivotGridField3, this.pivotGridField4, this.pivotGridField5, this.pivotGridField6, this.pivotGridField7 });
            this.pivotGridControl1.Location = new Point(0x1a, 0x5f);
            this.pivotGridControl1.LookAndFeel.SkinName = "Money Twins";
            this.pivotGridControl1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pivotGridControl1.Name = "pivotGridControl1";
            this.pivotGridControl1.OptionsDataField.Area = PivotDataArea.ColumnArea;
            this.pivotGridControl1.OptionsDataField.DataFieldArea = PivotArea.ColumnArea;
            this.pivotGridControl1.OptionsDataField.DataFieldAreaIndex = 0;
            this.pivotGridControl1.OptionsDataField.DataFieldVisible = true;
            this.pivotGridControl1.Size = new Size(0x298, 0x15c);
            this.pivotGridControl1.TabIndex = 7;
            this.pivotGridField1.Area = PivotArea.DataArea;
            this.pivotGridField1.AreaIndex = 0;
            this.pivotGridField1.FieldName = "built";
            this.pivotGridField1.Name = "pivotGridField1";
            this.pivotGridField1.UnboundType = UnboundColumnType.Integer;
            this.pivotGridField2.Area = PivotArea.DataArea;
            this.pivotGridField2.AreaIndex = 1;
            this.pivotGridField2.FieldName = "lost";
            this.pivotGridField2.Name = "pivotGridField2";
            this.pivotGridField2.UnboundType = UnboundColumnType.Integer;
            this.pivotGridField3.Area = PivotArea.DataArea;
            this.pivotGridField3.AreaIndex = 2;
            this.pivotGridField3.FieldName = "killed";
            this.pivotGridField3.Name = "pivotGridField3";
            this.pivotGridField3.UnboundType = UnboundColumnType.Integer;
            this.pivotGridField4.Area = PivotArea.DataArea;
            this.pivotGridField4.AreaIndex = 3;
            this.pivotGridField4.FieldName = "damage_dealt";
            this.pivotGridField4.Name = "pivotGridField4";
            this.pivotGridField4.UnboundType = UnboundColumnType.Decimal;
            this.pivotGridField5.Area = PivotArea.DataArea;
            this.pivotGridField5.AreaIndex = 4;
            this.pivotGridField5.FieldName = "damage_received";
            this.pivotGridField5.Name = "pivotGridField5";
            this.pivotGridField5.UnboundType = UnboundColumnType.Decimal;
            this.pivotGridField6.Area = PivotArea.RowArea;
            this.pivotGridField6.AreaIndex = 0;
            this.pivotGridField6.FieldName = "player_name";
            this.pivotGridField6.Name = "pivotGridField6";
            this.pivotGridField6.UnboundType = UnboundColumnType.String;
            this.pivotGridField7.Area = PivotArea.RowArea;
            this.pivotGridField7.AreaIndex = 1;
            this.pivotGridField7.FieldName = "unit_name";
            this.pivotGridField7.Name = "pivotGridField7";
            this.pivotGridField7.UnboundType = UnboundColumnType.String;
            base.AutoScaleDimensions = new SizeF(7f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x2dd, 0x207);
            base.Controls.Add(this.pivotGridControl1);
            this.Font = new Font("Verdana", 8f);
            base.Location = new Point(0, 0);
            base.Name = "DlgSupcomStats";
            this.Text = "DlgSupcomStats";
            base.Controls.SetChildIndex(this.pivotGridControl1, 0);
            this.pivotGridControl1.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

