namespace Egx.EgxControl
{
    partial class DBConnection
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.egxComboBox1 = new Egx.EgxControl.EgxComboBox();
            this.egxTextBox2 = new Egx.EgxControl.EgxTextBox();
            this.Username = new Egx.EgxControl.EgxTextBox();
            this.pwd = new Egx.EgxControl.EgxTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.egxComboBox2 = new Egx.EgxControl.EgxComboBox();
            this.cmpName = new Egx.EgxControl.EgxTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmpAddr = new Egx.EgxControl.EgxTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmpPhone = new Egx.EgxControl.EgxTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.egxTextBox1 = new Egx.EgxControl.EgxTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Database Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Database Type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Connection String";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Username";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Password";
            // 
            // egxComboBox1
            // 
            this.egxComboBox1.BackColor = System.Drawing.Color.White;
            this.egxComboBox1.Egxdatatype = Egx.EgxDataAccess.EgxDataType.Mssql;
            this.egxComboBox1.FillBySql = false;
            this.egxComboBox1.FocusColor = System.Drawing.Color.LightGoldenrodYellow;
            this.egxComboBox1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.egxComboBox1.FormattingEnabled = true;
            this.egxComboBox1.Items.AddRange(new object[] {
            "MS-SQL Server",
            "Access",
            "SqlLite",
            "SqlCE",
            "Oracle"});
            this.egxComboBox1.Location = new System.Drawing.Point(129, 44);
            this.egxComboBox1.Name = "egxComboBox1";
            this.egxComboBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.egxComboBox1.Size = new System.Drawing.Size(206, 21);
            this.egxComboBox1.SqlStr = null;
            this.egxComboBox1.TabIndex = 0;
            this.egxComboBox1.Text = "egxComboBox1";
            // 
            // egxTextBox2
            // 
            this.egxTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.egxTextBox2.FocusColor = System.Drawing.Color.LightGoldenrodYellow;
            this.egxTextBox2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.egxTextBox2.Location = new System.Drawing.Point(128, 232);
            this.egxTextBox2.Name = "egxTextBox2";
            this.egxTextBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.egxTextBox2.Size = new System.Drawing.Size(208, 20);
            this.egxTextBox2.TabIndex = 5;
            this.egxTextBox2.Type = Egx.EgxDataAccess.TextType.String;
            this.egxTextBox2.WatermarkText = "Connection String";
            // 
            // Username
            // 
            this.Username.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Username.FocusColor = System.Drawing.Color.LightGoldenrodYellow;
            this.Username.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.Username.Location = new System.Drawing.Point(128, 156);
            this.Username.Name = "Username";
            this.Username.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Username.Size = new System.Drawing.Size(208, 20);
            this.Username.TabIndex = 3;
            this.Username.Type = Egx.EgxDataAccess.TextType.String;
            this.Username.WatermarkText = "Username";
            // 
            // pwd
            // 
            this.pwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pwd.FocusColor = System.Drawing.Color.LightGoldenrodYellow;
            this.pwd.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.pwd.Location = new System.Drawing.Point(128, 195);
            this.pwd.Name = "pwd";
            this.pwd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.pwd.Size = new System.Drawing.Size(208, 20);
            this.pwd.TabIndex = 4;
            this.pwd.Type = Egx.EgxDataAccess.TextType.String;
            this.pwd.UseSystemPasswordChar = true;
            this.pwd.WatermarkText = "Password";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Server Address";
            // 
            // button2
            // 
            this.button2.Image = global::Egx.Properties.Resources.Test64;
            this.button2.Location = new System.Drawing.Point(227, 218);
            this.button2.Name = "button2";
            this.button2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button2.Size = new System.Drawing.Size(135, 123);
            this.button2.TabIndex = 9;
            this.button2.Text = "Test Connection";
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // egxComboBox2
            // 
            this.egxComboBox2.BackColor = System.Drawing.Color.White;
            this.egxComboBox2.Egxdatatype = Egx.EgxDataAccess.EgxDataType.Mssql;
            this.egxComboBox2.FillBySql = false;
            this.egxComboBox2.FocusColor = System.Drawing.Color.LightGoldenrodYellow;
            this.egxComboBox2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.egxComboBox2.FormattingEnabled = true;
            this.egxComboBox2.Location = new System.Drawing.Point(128, 118);
            this.egxComboBox2.Name = "egxComboBox2";
            this.egxComboBox2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.egxComboBox2.Size = new System.Drawing.Size(208, 21);
            this.egxComboBox2.SqlStr = null;
            this.egxComboBox2.TabIndex = 2;
            this.egxComboBox2.Text = "egxComboBox2";
            // 
            // cmpName
            // 
            this.cmpName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cmpName.FocusColor = System.Drawing.Color.LightGoldenrodYellow;
            this.cmpName.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.cmpName.Location = new System.Drawing.Point(125, 31);
            this.cmpName.Name = "cmpName";
            this.cmpName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmpName.Size = new System.Drawing.Size(154, 20);
            this.cmpName.TabIndex = 6;
            this.cmpName.Type = Egx.EgxDataAccess.TextType.String;
            this.cmpName.WatermarkText = "Company Name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Company Name";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Company Address";
            // 
            // cmpAddr
            // 
            this.cmpAddr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cmpAddr.FocusColor = System.Drawing.Color.LightGoldenrodYellow;
            this.cmpAddr.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.cmpAddr.Location = new System.Drawing.Point(125, 68);
            this.cmpAddr.Name = "cmpAddr";
            this.cmpAddr.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmpAddr.Size = new System.Drawing.Size(154, 20);
            this.cmpAddr.TabIndex = 7;
            this.cmpAddr.Type = Egx.EgxDataAccess.TextType.String;
            this.cmpAddr.WatermarkText = "Company Address";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Company Phone";
            // 
            // cmpPhone
            // 
            this.cmpPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cmpPhone.FocusColor = System.Drawing.Color.LightGoldenrodYellow;
            this.cmpPhone.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.cmpPhone.Location = new System.Drawing.Point(125, 101);
            this.cmpPhone.Name = "cmpPhone";
            this.cmpPhone.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmpPhone.Size = new System.Drawing.Size(154, 20);
            this.cmpPhone.TabIndex = 8;
            this.cmpPhone.Type = Egx.EgxDataAccess.TextType.String;
            this.cmpPhone.WatermarkText = "Company Name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.egxTextBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.egxComboBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.egxComboBox2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.Username);
            this.groupBox1.Controls.Add(this.pwd);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.egxTextBox2);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(374, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(342, 281);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection Configurations";
            // 
            // egxTextBox1
            // 
            this.egxTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.egxTextBox1.FocusColor = System.Drawing.Color.Empty;
            this.egxTextBox1.Location = new System.Drawing.Point(128, 84);
            this.egxTextBox1.Name = "egxTextBox1";
            this.egxTextBox1.Size = new System.Drawing.Size(207, 20);
            this.egxTextBox1.TabIndex = 1;
            this.egxTextBox1.Type = Egx.EgxDataAccess.TextType.String;
            this.egxTextBox1.WatermarkText = null;
            this.egxTextBox1.Leave += new System.EventHandler(this.egxTextBox1_Leave_1);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmpName);
            this.groupBox2.Controls.Add(this.cmpAddr);
            this.groupBox2.Controls.Add(this.cmpPhone);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(83, 60);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(285, 140);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Company Info";
            // 
            // button1
            // 
            this.button1.Image = global::Egx.Properties.Resources.Setup641;
            this.button1.Location = new System.Drawing.Point(83, 218);
            this.button1.Name = "button1";
            this.button1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button1.Size = new System.Drawing.Size(138, 123);
            this.button1.TabIndex = 10;
            this.button1.Text = "Setup new connection ";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DBConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 374);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "DBConnection";
            this.Text = "DBConnection";
            this.Load += new System.EventHandler(this.DBConnection_Load);
            this.Controls.SetChildIndex(this.button1, 0);
            this.Controls.SetChildIndex(this.button2, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private EgxComboBox egxComboBox1;
        private EgxTextBox egxTextBox2;
        private EgxTextBox Username;
        private EgxTextBox pwd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private EgxComboBox egxComboBox2;
        private EgxTextBox cmpName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private EgxTextBox cmpAddr;
        private System.Windows.Forms.Label label9;
        private EgxTextBox cmpPhone;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private EgxTextBox egxTextBox1;
    }
}