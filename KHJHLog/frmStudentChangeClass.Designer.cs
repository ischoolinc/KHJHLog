namespace KHJHLog
{
    partial class frmStudentChangeClass
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.chkSchool = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.lvSchool = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.btnQuery = new DevComponents.DotNetBar.ButtonX();
            this.btnExport = new DevComponents.DotNetBar.ButtonX();
            this.dgData = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colSchool = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStudName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClassContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOldClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOldClassContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClassOrder1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClassOrder2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClassOrder3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.txtClassComment = new DevComponents.DotNetBar.Controls.TextBoxX();
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).BeginInit();
            this.SuspendLayout();
            // 
            // chkSchool
            // 
            this.chkSchool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSchool.AutoSize = true;
            this.chkSchool.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkSchool.BackgroundStyle.Class = "";
            this.chkSchool.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkSchool.Location = new System.Drawing.Point(828, 37);
            this.chkSchool.Name = "chkSchool";
            this.chkSchool.Size = new System.Drawing.Size(54, 21);
            this.chkSchool.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkSchool.TabIndex = 13;
            this.chkSchool.Text = "全選";
            this.chkSchool.CheckedChanged += new System.EventHandler(this.chkSchool_CheckedChanged);
            // 
            // lvSchool
            // 
            this.lvSchool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.lvSchool.Border.Class = "ListViewBorder";
            this.lvSchool.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lvSchool.CheckBoxes = true;
            this.lvSchool.Location = new System.Drawing.Point(24, 68);
            this.lvSchool.Name = "lvSchool";
            this.lvSchool.Size = new System.Drawing.Size(858, 140);
            this.lvSchool.TabIndex = 12;
            this.lvSchool.UseCompatibleStateImageBehavior = false;
            this.lvSchool.View = System.Windows.Forms.View.List;
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(807, 536);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.AutoSize = true;
            this.btnQuery.BackColor = System.Drawing.Color.Transparent;
            this.btnQuery.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnQuery.Location = new System.Drawing.Point(716, 536);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 25);
            this.btnQuery.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnQuery.TabIndex = 10;
            this.btnQuery.Text = "查詢";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnExport
            // 
            this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.AutoSize = true;
            this.btnExport.BackColor = System.Drawing.Color.Transparent;
            this.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExport.Location = new System.Drawing.Point(24, 536);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 25);
            this.btnExport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExport.TabIndex = 9;
            this.btnExport.Text = "匯出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // dgData
            // 
            this.dgData.AllowUserToAddRows = false;
            this.dgData.AllowUserToDeleteRows = false;
            this.dgData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgData.BackgroundColor = System.Drawing.Color.White;
            this.dgData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSchool,
            this.colStudName,
            this.colClassName,
            this.colClassContent,
            this.colOldClassName,
            this.colOldClassContent,
            this.colClassOrder1,
            this.colClassOrder2,
            this.colClassOrder3});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgData.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dgData.Location = new System.Drawing.Point(24, 223);
            this.dgData.Name = "dgData";
            this.dgData.RowTemplate.Height = 24;
            this.dgData.Size = new System.Drawing.Size(859, 294);
            this.dgData.TabIndex = 8;
            // 
            // colSchool
            // 
            this.colSchool.HeaderText = "學校名稱";
            this.colSchool.Name = "colSchool";
            this.colSchool.ReadOnly = true;
            // 
            // colStudName
            // 
            this.colStudName.HeaderText = "學生姓名";
            this.colStudName.Name = "colStudName";
            this.colStudName.ReadOnly = true;
            // 
            // colClassName
            // 
            this.colClassName.HeaderText = "班級名稱";
            this.colClassName.Name = "colClassName";
            this.colClassName.ReadOnly = true;
            // 
            // colClassContent
            // 
            this.colClassContent.HeaderText = "班級備註";
            this.colClassContent.Name = "colClassContent";
            this.colClassContent.ReadOnly = true;
            // 
            // colOldClassName
            // 
            this.colOldClassName.HeaderText = "調整前班級";
            this.colOldClassName.Name = "colOldClassName";
            this.colOldClassName.ReadOnly = true;
            // 
            // colOldClassContent
            // 
            this.colOldClassContent.HeaderText = "調整前班級備註";
            this.colOldClassContent.Name = "colOldClassContent";
            this.colOldClassContent.ReadOnly = true;
            // 
            // colClassOrder1
            // 
            this.colClassOrder1.HeaderText = "編班順序第一";
            this.colClassOrder1.Name = "colClassOrder1";
            this.colClassOrder1.ReadOnly = true;
            // 
            // colClassOrder2
            // 
            this.colClassOrder2.HeaderText = "編班順序第二";
            this.colClassOrder2.Name = "colClassOrder2";
            this.colClassOrder2.ReadOnly = true;
            // 
            // colClassOrder3
            // 
            this.colClassOrder3.HeaderText = "編班順序第三";
            this.colClassOrder3.Name = "colClassOrder3";
            this.colClassOrder3.ReadOnly = true;
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(34, 37);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(60, 21);
            this.labelX1.TabIndex = 16;
            this.labelX1.Text = "班級備註";
            // 
            // txtClassComment
            // 
            // 
            // 
            // 
            this.txtClassComment.Border.Class = "TextBoxBorder";
            this.txtClassComment.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtClassComment.Location = new System.Drawing.Point(101, 35);
            this.txtClassComment.Name = "txtClassComment";
            this.txtClassComment.Size = new System.Drawing.Size(127, 25);
            this.txtClassComment.TabIndex = 17;
            // 
            // frmStudentChangeClass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 568);
            this.Controls.Add(this.txtClassComment);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.chkSchool);
            this.Controls.Add(this.lvSchool);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.dgData);
            this.DoubleBuffered = true;
            this.Name = "frmStudentChangeClass";
            this.Text = "學生調整班級";
            this.Load += new System.EventHandler(this.frmStudentChangeClass_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.CheckBoxX chkSchool;
        private DevComponents.DotNetBar.Controls.ListViewEx lvSchool;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.ButtonX btnQuery;
        private DevComponents.DotNetBar.ButtonX btnExport;
        private DevComponents.DotNetBar.Controls.DataGridViewX dgData;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSchool;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStudName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClassContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOldClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOldClassContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClassOrder1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClassOrder2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClassOrder3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtClassComment;

    }
}