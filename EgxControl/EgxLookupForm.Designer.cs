namespace Egx.EgxControl
{
    partial class EgxLookupForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.egxClick2 = new Egx.EgxControl.EgxClick();
            this.egxClick1 = new Egx.EgxControl.EgxClick();
            this.label1 = new System.Windows.Forms.Label();
            this.egxTextBox1 = new Egx.EgxControl.EgxTextBox();
            this.egxComboBox1 = new Egx.EgxControl.EgxComboBox();
            this.dgv = new Egx.EgxControl.EgxGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.egxClick2);
            this.panel1.Controls.Add(this.egxClick1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.egxTextBox1);
            this.panel1.Controls.Add(this.egxComboBox1);
            this.panel1.Location = new System.Drawing.Point(12, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(520, 82);
            this.panel1.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(446, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "مفتاح البحث";
            // 
            // egxClick2
            // 
            this.egxClick2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.egxClick2.Image = global::Egx.Properties.Resources._20130821055433403_easyicon_net_32;
            this.egxClick2.Location = new System.Drawing.Point(143, 42);
            this.egxClick2.MouseLeaveIcon = global::Egx.Properties.Resources._20130821055433403_easyicon_net_32;
            this.egxClick2.MouseMoveIcon = global::Egx.Properties.Resources._2013082105543793_easyicon_net_32;
            this.egxClick2.Name = "egxClick2";
            this.egxClick2.Size = new System.Drawing.Size(32, 32);
            this.egxClick2.TabIndex = 4;
            this.egxClick2.Click += new System.EventHandler(this.egxClick2_Click);
            // 
            // egxClick1
            // 
            this.egxClick1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.egxClick1.Image = global::Egx.Properties.Resources._20130821054822638_easyicon_net_32;
            this.egxClick1.Location = new System.Drawing.Point(145, 7);
            this.egxClick1.MouseLeaveIcon = global::Egx.Properties.Resources._20130821054822638_easyicon_net_32;
            this.egxClick1.MouseMoveIcon = global::Egx.Properties.Resources._20130821054829803_easyicon_net_32;
            this.egxClick1.Name = "egxClick1";
            this.egxClick1.Size = new System.Drawing.Size(32, 32);
            this.egxClick1.TabIndex = 3;
            this.egxClick1.Click += new System.EventHandler(this.egxClick1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(446, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "بحث بواسطة";
            // 
            // egxTextBox1
            // 
            this.egxTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.egxTextBox1.FocusColor = System.Drawing.Color.Empty;
            this.egxTextBox1.Location = new System.Drawing.Point(218, 42);
            this.egxTextBox1.Multiline = true;
            this.egxTextBox1.Name = "egxTextBox1";
            this.egxTextBox1.Size = new System.Drawing.Size(202, 32);
            this.egxTextBox1.TabIndex = 1;
           // this.egxTextBox1.TextBoxType = Egx.EgxDataAccess.TextBoxTypes.String;
            this.egxTextBox1.WatermarkText = null;
            // 
            // egxComboBox1
            // 
            this.egxComboBox1.BackColor = System.Drawing.Color.White;
            this.egxComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.egxComboBox1.Egxdatatype = Egx.EgxDataAccess.EgxDataType.Mssql;
            this.egxComboBox1.FillBySql = false;
            this.egxComboBox1.FocusColor = System.Drawing.Color.Empty;
            this.egxComboBox1.FormattingEnabled = true;
            this.egxComboBox1.Location = new System.Drawing.Point(218, 17);
            this.egxComboBox1.Name = "egxComboBox1";
            this.egxComboBox1.Size = new System.Drawing.Size(202, 21);
            this.egxComboBox1.SqlStr = null;
            this.egxComboBox1.TabIndex = 0;
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToOrderColumns = true;
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
            this.dgv.EnableHeadersVisualStyles = false;
            this.dgv.Location = new System.Drawing.Point(12, 127);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.Size = new System.Drawing.Size(520, 213);
            this.dgv.TabIndex = 9;
            // 
            // EgxLookupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 360);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.panel1);
            this.Name = "EgxLookupForm";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.dgv, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private EgxClick egxClick1;
        private System.Windows.Forms.Label label1;
        private EgxTextBox egxTextBox1;
        private EgxComboBox egxComboBox1;
        private EgxClick egxClick2;
        private System.Windows.Forms.Label label2;
        public EgxGridView dgv;
    }
}
