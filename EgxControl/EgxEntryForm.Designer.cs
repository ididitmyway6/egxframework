namespace Egx.EgxControl
{
    partial class EgxEntryForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.addNew = new Egx.EgxControl.EgxClick();
            this.save = new Egx.EgxControl.EgxClick();
            this.Delete = new Egx.EgxControl.EgxClick();
            this.egxClick1 = new Egx.EgxControl.EgxClick();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.egxNavigator1 = new Egx.EgxControl.EgxNavigator();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Status = new System.Windows.Forms.Label();
            this.CntlPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ErrList = new System.Windows.Forms.DataGridView();
            this.ErrIcon = new System.Windows.Forms.DataGridViewImageColumn();
            this.ErrDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ErrList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.addNew);
            this.flowLayoutPanel1.Controls.Add(this.save);
            this.flowLayoutPanel1.Controls.Add(this.Delete);
            this.flowLayoutPanel1.Controls.Add(this.egxClick1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(450, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(194, 33);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // addNew
            // 
            this.addNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addNew.Image = global::Egx.Properties.Resources.Button_Add_green_32;
            this.addNew.Location = new System.Drawing.Point(152, 0);
            this.addNew.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.addNew.MouseLeaveIcon = global::Egx.Properties.Resources.Button_Add_green_32;
            this.addNew.MouseMoveIcon = global::Egx.Properties.Resources.Button_Add_blue_32;
            this.addNew.Name = "addNew";
            this.addNew.Size = new System.Drawing.Size(32, 32);
            this.addNew.TabIndex = 1;
            this.addNew.Click += new System.EventHandler(this.addNew_Click);
            // 
            // save
            // 
            this.save.Cursor = System.Windows.Forms.Cursors.Hand;
            this.save.Image = global::Egx.Properties.Resources.Save_Diskette_Floppy_Disk_32;
            this.save.Location = new System.Drawing.Point(107, 0);
            this.save.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.save.MouseLeaveIcon = global::Egx.Properties.Resources.Save_Diskette_Floppy_Disk_32;
            this.save.MouseMoveIcon = global::Egx.Properties.Resources.Save_Download_32;
            this.save.Name = "save";
            this.save.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.save.Size = new System.Drawing.Size(32, 32);
            this.save.TabIndex = 2;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // Delete
            // 
            this.Delete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Delete.Image = global::Egx.Properties.Resources.Delete_32;
            this.Delete.Location = new System.Drawing.Point(62, 0);
            this.Delete.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.Delete.MouseLeaveIcon = global::Egx.Properties.Resources.Delete_32;
            this.Delete.MouseMoveIcon = global::Egx.Properties.Resources.Delete_32__1_;
            this.Delete.Name = "Delete";
            this.Delete.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Delete.Size = new System.Drawing.Size(32, 32);
            this.Delete.TabIndex = 3;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // egxClick1
            // 
            this.egxClick1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.egxClick1.Image = global::Egx.Properties.Resources.refresh;
            this.egxClick1.Location = new System.Drawing.Point(17, 0);
            this.egxClick1.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.egxClick1.MouseLeaveIcon = global::Egx.Properties.Resources.refresh;
            this.egxClick1.MouseMoveIcon = global::Egx.Properties.Resources.refresh_yellow;
            this.egxClick1.Name = "egxClick1";
            this.egxClick1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.egxClick1.Size = new System.Drawing.Size(32, 32);
            this.egxClick1.TabIndex = 4;
            this.egxClick1.Click += new System.EventHandler(this.egxClick1_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel2.Controls.Add(this.flowLayoutPanel1);
            this.flowLayoutPanel2.Controls.Add(this.egxNavigator1);
            this.flowLayoutPanel2.Controls.Add(this.panel2);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 350);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(649, 40);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // egxNavigator1
            // 
            this.egxNavigator1.AutoSize = true;
            this.egxNavigator1.CurrentIndex = 0;
            this.egxNavigator1.CurrentObject = null;
            this.egxNavigator1.DataSource = null;
            this.egxNavigator1.FindBy = null;
            this.egxNavigator1.Location = new System.Drawing.Point(254, 3);
            this.egxNavigator1.LookupID = "";
            this.egxNavigator1.LookupValueMember = "";
            this.egxNavigator1.Name = "egxNavigator1";
            this.egxNavigator1.NavigationKey = null;
            this.egxNavigator1.NavObject = null;
            this.egxNavigator1.PKName = null;
            this.egxNavigator1.Size = new System.Drawing.Size(190, 32);
            this.egxNavigator1.StartupID = -1;
            this.egxNavigator1.TabIndex = 2;
            this.egxNavigator1.TextWidth = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Status);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(198, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(50, 33);
            this.panel2.TabIndex = 4;
            this.panel2.Visible = false;
            // 
            // Status
            // 
            this.Status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Status.Image = global::Egx.Properties.Resources._20130823070240688_easyicon_net_32;
            this.Status.Location = new System.Drawing.Point(0, 0);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(50, 33);
            this.Status.TabIndex = 3;
            // 
            // CntlPanel
            // 
            this.CntlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.CntlPanel.Location = new System.Drawing.Point(0, 36);
            this.CntlPanel.Name = "CntlPanel";
            this.CntlPanel.Size = new System.Drawing.Size(649, 225);
            this.CntlPanel.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ErrList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 261);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(649, 89);
            this.panel1.TabIndex = 5;
            // 
            // ErrList
            // 
            this.ErrList.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Red;
            this.ErrList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ErrList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ErrList.BackgroundColor = System.Drawing.Color.White;
            this.ErrList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ErrList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ErrList.ColumnHeadersHeight = 20;
            this.ErrList.ColumnHeadersVisible = false;
            this.ErrList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ErrIcon,
            this.ErrDesc});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ErrList.DefaultCellStyle = dataGridViewCellStyle3;
            this.ErrList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrList.EnableHeadersVisualStyles = false;
            this.ErrList.GridColor = System.Drawing.Color.White;
            this.ErrList.Location = new System.Drawing.Point(0, 0);
            this.ErrList.Name = "ErrList";
            this.ErrList.ReadOnly = true;
            this.ErrList.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Red;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Red;
            this.ErrList.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.ErrList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.ErrList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
            this.ErrList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.ErrList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Red;
            this.ErrList.RowTemplate.Height = 34;
            this.ErrList.Size = new System.Drawing.Size(647, 87);
            this.ErrList.TabIndex = 8;
            this.ErrList.Visible = false;
            // 
            // ErrIcon
            // 
            this.ErrIcon.FillWeight = 30.45685F;
            this.ErrIcon.HeaderText = "";
            this.ErrIcon.Image = global::Egx.Properties.Resources.Attention_32;
            this.ErrIcon.Name = "ErrIcon";
            this.ErrIcon.ReadOnly = true;
            // 
            // ErrDesc
            // 
            this.ErrDesc.DataPropertyName = "ErrorText";
            this.ErrDesc.FillWeight = 169.5432F;
            this.ErrDesc.HeaderText = "";
            this.ErrDesc.Name = "ErrDesc";
            this.ErrDesc.ReadOnly = true;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.FillWeight = 30.45685F;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::Egx.Properties.Resources.Attention_32;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Width = 323;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // EgxEntryForm
            // 
            this.ClientSize = new System.Drawing.Size(649, 390);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.CntlPanel);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Name = "EgxEntryForm";
            this.WindowTitle = "DataEntry";
            this.Controls.SetChildIndex(this.flowLayoutPanel2, 0);
            this.Controls.SetChildIndex(this.CntlPanel, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ErrList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        public System.Windows.Forms.Panel CntlPanel;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.DataGridView ErrList;
        private System.Windows.Forms.DataGridViewImageColumn ErrIcon;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrDesc;
        public EgxNavigator egxNavigator1;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        public EgxClick addNew;
        public EgxClick save;
        public EgxClick Delete;
        public EgxClick egxClick1;

    }
}
