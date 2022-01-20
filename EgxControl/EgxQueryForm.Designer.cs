namespace Egx.EgxControl
{
    partial class EgxQueryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv = new Egx.EgxControl.EgxGridView();
            this.SqlTxt = new Egx.EgxControl.EgxTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.egxComboBox1 = new Egx.EgxControl.EgxComboBox();
            this.egxTextBox1 = new Egx.EgxControl.EgxTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.egxGridView1 = new Egx.EgxControl.EgxGridView();
            this.colField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExp = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colVal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.egxGridView1)).BeginInit();
            this.panel3.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AlternateRowColor = System.Drawing.Color.Empty;
            this.dgv.AlternateRowForeColor = System.Drawing.Color.Empty;
            this.dgv.AlternativeRowEffect = false;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.ColumnHeadersHeight = 25;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgv.DeleteByColumnName = null;
            this.dgv.DeleteMethod = null;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.EgxFeatures = true;
            this.dgv.EnableHeadersVisualStyles = false;
            this.dgv.InsertMethod = null;
            this.dgv.ListingMethod = null;
            this.dgv.Location = new System.Drawing.Point(0, 0);
            this.dgv.Name = "dgv";
            this.dgv.ObjectiveMood = false;
            this.dgv.ObjectType = null;
            this.dgv.QueryFilter = "";
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(846, 225);
            this.dgv.SqlQuery = null;
            this.dgv.TabIndex = 3;
            this.dgv.TableName = "";
            this.dgv.UpdateMethod = null;
            this.dgv.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.egxGridView1_CellDoubleClick);
            // 
            // SqlTxt
            // 
            this.SqlTxt.BackColor = System.Drawing.Color.White;
            this.SqlTxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SqlTxt.FocusColor = System.Drawing.Color.LightGoldenrodYellow;
            this.SqlTxt.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SqlTxt.Location = new System.Drawing.Point(73, 128);
            this.SqlTxt.Name = "SqlTxt";
            this.SqlTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.SqlTxt.Size = new System.Drawing.Size(585, 27);
            this.SqlTxt.TabIndex = 4;
            this.SqlTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SqlTxt.Type = Egx.EgxDataAccess.TextType.String;
            this.SqlTxt.WatermarkText = "جملة الاستعلام";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgv);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 305);
            this.panel1.Name = "panel1";
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panel1.Size = new System.Drawing.Size(846, 225);
            this.panel1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(664, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "جملة الاستعلام";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(717, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "بحث";
            // 
            // egxComboBox1
            // 
            this.egxComboBox1.BackColor = System.Drawing.Color.White;
            this.egxComboBox1.Egxdatatype = Egx.EgxDataAccess.EgxDataType.Mssql;
            this.egxComboBox1.FillBySql = false;
            this.egxComboBox1.FocusColor = System.Drawing.Color.LightGoldenrodYellow;
            this.egxComboBox1.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.egxComboBox1.FormattingEnabled = true;
            this.egxComboBox1.Location = new System.Drawing.Point(174, 12);
            this.egxComboBox1.Name = "egxComboBox1";
            this.egxComboBox1.Size = new System.Drawing.Size(221, 33);
            this.egxComboBox1.SqlStr = null;
            this.egxComboBox1.TabIndex = 10;
            this.egxComboBox1.Text = "egxComboBox1";
            // 
            // egxTextBox1
            // 
            this.egxTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.egxTextBox1.FocusColor = System.Drawing.Color.LightGoldenrodYellow;
            this.egxTextBox1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.egxTextBox1.Location = new System.Drawing.Point(401, 12);
            this.egxTextBox1.Multiline = true;
            this.egxTextBox1.Name = "egxTextBox1";
            this.egxTextBox1.Size = new System.Drawing.Size(314, 33);
            this.egxTextBox1.TabIndex = 11;
            this.egxTextBox1.Type = Egx.EgxDataAccess.TextType.String;
            this.egxTextBox1.WatermarkText = null;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.egxGridView1);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.SqlTxt);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panel2.Size = new System.Drawing.Size(749, 160);
            this.panel2.TabIndex = 14;
            // 
            // egxGridView1
            // 
            this.egxGridView1.AllowUserToDeleteRows = false;
            this.egxGridView1.AlternateRowColor = System.Drawing.Color.Empty;
            this.egxGridView1.AlternateRowForeColor = System.Drawing.Color.Empty;
            this.egxGridView1.AlternativeRowEffect = false;
            this.egxGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.egxGridView1.BackgroundColor = System.Drawing.Color.White;
            this.egxGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.egxGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.egxGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.egxGridView1.ColumnHeadersHeight = 25;
            this.egxGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.egxGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colField,
            this.colExp,
            this.colVal});
            this.egxGridView1.DeleteByColumnName = null;
            this.egxGridView1.DeleteMethod = null;
            this.egxGridView1.EgxFeatures = false;
            this.egxGridView1.EnableHeadersVisualStyles = false;
            this.egxGridView1.InsertMethod = null;
            this.egxGridView1.ListingMethod = null;
            this.egxGridView1.Location = new System.Drawing.Point(73, 10);
            this.egxGridView1.Name = "egxGridView1";
            this.egxGridView1.ObjectiveMood = false;
            this.egxGridView1.ObjectType = null;
            this.egxGridView1.QueryFilter = "";
            this.egxGridView1.RowHeadersVisible = false;
            this.egxGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.egxGridView1.Size = new System.Drawing.Size(668, 113);
            this.egxGridView1.SqlQuery = null;
            this.egxGridView1.TabIndex = 13;
            this.egxGridView1.TableName = "";
            this.egxGridView1.UpdateMethod = null;
            // 
            // colField
            // 
            this.colField.HeaderText = "الحقل";
            this.colField.Name = "colField";
            // 
            // colExp
            // 
            this.colExp.HeaderText = "معامل البحث";
            this.colExp.Items.AddRange(new object[] {
            ">",
            "<",
            "=",
            "AND",
            "OR",
            "NOT"});
            this.colExp.Name = "colExp";
            this.colExp.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colExp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colVal
            // 
            this.colVal.HeaderText = "القيمة";
            this.colVal.Name = "colVal";
            // 
            // button4
            // 
            this.button4.BackgroundImage = global::Egx.Properties.Resources.undo;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button4.Location = new System.Drawing.Point(23, 62);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(44, 41);
            this.button4.TabIndex = 9;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::Egx.Properties.Resources.Execute;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.Location = new System.Drawing.Point(23, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(44, 46);
            this.button1.TabIndex = 6;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::Egx.Properties.Resources.Setting;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button2.Location = new System.Drawing.Point(84, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(42, 37);
            this.button2.TabIndex = 7;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackgroundImage = global::Egx.Properties.Resources.search;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button3.Location = new System.Drawing.Point(129, 10);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(40, 37);
            this.button3.TabIndex = 8;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.egxComboBox1);
            this.panel3.Controls.Add(this.egxTextBox1);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Controls.Add(this.button2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(749, 56);
            this.panel3.TabIndex = 16;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(24, 57);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.splitContainer1.Size = new System.Drawing.Size(749, 220);
            this.splitContainer1.SplitterDistance = 160;
            this.splitContainer1.TabIndex = 17;
            // 
            // EgxQueryForm
            // 
            this.ClientSize = new System.Drawing.Size(846, 530);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "EgxQueryForm";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.egxGridView1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private EgxGridView dgv;
        private EgxTextBox SqlTxt;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private EgxComboBox egxComboBox1;
        private EgxTextBox egxTextBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private EgxGridView egxGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colField;
        private System.Windows.Forms.DataGridViewComboBoxColumn colExp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVal;
    }
}
