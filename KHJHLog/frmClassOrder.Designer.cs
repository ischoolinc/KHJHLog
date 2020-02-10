namespace KHJHLog
{
    partial class frmClassOrder
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
            this.grdClassOrder = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.cmbSchool = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnExport = new DevComponents.DotNetBar.ButtonX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.colSchool = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClassName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTeacherName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRealNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuspensionStudentCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDropOutStudentCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLockComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DistrictComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DistrictLockOrUnlockDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGradeYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDisplayOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdClassOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // grdClassOrder
            // 
            this.grdClassOrder.AllowUserToAddRows = false;
            this.grdClassOrder.AllowUserToDeleteRows = false;
            this.grdClassOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdClassOrder.BackgroundColor = System.Drawing.Color.White;
            this.grdClassOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdClassOrder.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSchool,
            this.colClassName,
            this.colTeacherName,
            this.colRealNumber,
            this.colSuspensionStudentCount,
            this.colDropOutStudentCount,
            this.colEstNumber,
            this.Column1,
            this.colOrder,
            this.colLock,
            this.colLockComment,
            this.DistrictComment,
            this.DistrictLockOrUnlockDate,
            this.colGradeYear,
            this.colDisplayOrder});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdClassOrder.DefaultCellStyle = dataGridViewCellStyle1;
            this.grdClassOrder.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.grdClassOrder.Location = new System.Drawing.Point(12, 58);
            this.grdClassOrder.Name = "grdClassOrder";
            this.grdClassOrder.ReadOnly = true;
            this.grdClassOrder.RowHeadersVisible = false;
            this.grdClassOrder.RowTemplate.Height = 24;
            this.grdClassOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdClassOrder.Size = new System.Drawing.Size(1229, 623);
            this.grdClassOrder.TabIndex = 0;
            this.grdClassOrder.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdClassOrder_CellDoubleClick);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX1.BackColor = System.Drawing.Color.Transparent;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(1068, 17);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 1;
            this.buttonX1.Text = "查詢";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // cmbSchool
            // 
            this.cmbSchool.DisplayMember = "Text";
            this.cmbSchool.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbSchool.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSchool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSchool.FormattingEnabled = true;
            this.cmbSchool.ItemHeight = 19;
            this.cmbSchool.Location = new System.Drawing.Point(92, 16);
            this.cmbSchool.Name = "cmbSchool";
            this.cmbSchool.Size = new System.Drawing.Size(553, 25);
            this.cmbSchool.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cmbSchool.TabIndex = 2;
            this.cmbSchool.SelectedIndexChanged += new System.EventHandler(this.cmbSchool_SelectedIndexChanged);
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
            this.labelX1.Location = new System.Drawing.Point(12, 18);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(74, 21);
            this.labelX1.TabIndex = 3;
            this.labelX1.Text = "選擇學校：";
            // 
            // btnExport
            // 
            this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.Transparent;
            this.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExport.Location = new System.Drawing.Point(1166, 17);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "匯出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(651, 17);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(318, 23);
            this.labelX2.TabIndex = 5;
            this.labelX2.Text = "說明：「實際人數」包含「休學人數」及「輟學人數」";
            // 
            // colSchool
            // 
            this.colSchool.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSchool.HeaderText = "學校";
            this.colSchool.Name = "colSchool";
            this.colSchool.ReadOnly = true;
            // 
            // colClassName
            // 
            this.colClassName.HeaderText = "班級名稱";
            this.colClassName.Name = "colClassName";
            this.colClassName.ReadOnly = true;
            // 
            // colTeacherName
            // 
            this.colTeacherName.HeaderText = "班導師";
            this.colTeacherName.Name = "colTeacherName";
            this.colTeacherName.ReadOnly = true;
            // 
            // colRealNumber
            // 
            this.colRealNumber.HeaderText = "實際人數";
            this.colRealNumber.Name = "colRealNumber";
            this.colRealNumber.ReadOnly = true;
            this.colRealNumber.Width = 90;
            // 
            // colSuspensionStudentCount
            // 
            this.colSuspensionStudentCount.HeaderText = "休學人數";
            this.colSuspensionStudentCount.Name = "colSuspensionStudentCount";
            this.colSuspensionStudentCount.ReadOnly = true;
            this.colSuspensionStudentCount.Width = 90;
            // 
            // colDropOutStudentCount
            // 
            this.colDropOutStudentCount.HeaderText = "輟學人數";
            this.colDropOutStudentCount.Name = "colDropOutStudentCount";
            this.colDropOutStudentCount.ReadOnly = true;
            this.colDropOutStudentCount.Width = 90;
            // 
            // colEstNumber
            // 
            this.colEstNumber.HeaderText = "編班人數";
            this.colEstNumber.Name = "colEstNumber";
            this.colEstNumber.ReadOnly = true;
            this.colEstNumber.Width = 90;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "特殊生人數";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // colOrder
            // 
            this.colOrder.HeaderText = "編班順位";
            this.colOrder.Name = "colOrder";
            this.colOrder.ReadOnly = true;
            this.colOrder.Width = 90;
            // 
            // colLock
            // 
            this.colLock.HeaderText = "編班鎖定";
            this.colLock.Name = "colLock";
            this.colLock.ReadOnly = true;
            this.colLock.Width = 90;
            // 
            // colLockComment
            // 
            this.colLockComment.HeaderText = "鎖定備註";
            this.colLockComment.Name = "colLockComment";
            this.colLockComment.ReadOnly = true;
            this.colLockComment.Width = 90;
            // 
            // DistrictComment
            // 
            this.DistrictComment.HeaderText = "局端備註";
            this.DistrictComment.Name = "DistrictComment";
            this.DistrictComment.ReadOnly = true;
            // 
            // DistrictLockOrUnlockDate
            // 
            this.DistrictLockOrUnlockDate.HeaderText = "局端解鎖/鎖班日期";
            this.DistrictLockOrUnlockDate.Name = "DistrictLockOrUnlockDate";
            this.DistrictLockOrUnlockDate.ReadOnly = true;
            // 
            // colGradeYear
            // 
            this.colGradeYear.HeaderText = "年級";
            this.colGradeYear.Name = "colGradeYear";
            this.colGradeYear.ReadOnly = true;
            this.colGradeYear.Width = 60;
            // 
            // colDisplayOrder
            // 
            this.colDisplayOrder.HeaderText = "班級順序";
            this.colDisplayOrder.Name = "colDisplayOrder";
            this.colDisplayOrder.ReadOnly = true;
            this.colDisplayOrder.Width = 90;
            // 
            // frmClassOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1250, 691);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.cmbSchool);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.grdClassOrder);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(870, 600);
            this.Name = "frmClassOrder";
            this.Text = "查詢編班";
            this.TitleText = "查詢編班";
            this.Load += new System.EventHandler(this.frmClassOrder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdClassOrder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX grdClassOrder;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cmbSchool;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnExport;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSchool;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClassName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTeacherName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRealNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuspensionStudentCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDropOutStudentCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLock;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLockComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn DistrictComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn DistrictLockOrUnlockDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGradeYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDisplayOrder;
    }
}