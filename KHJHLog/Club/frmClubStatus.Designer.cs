namespace KHJHLog
{
    partial class frmClubStatus
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.cbDefSemester = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.intDefSchoolYear = new DevComponents.Editors.IntegerInput();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.btnStart = new DevComponents.DotNetBar.ButtonX();
            this.btnViewClub = new DevComponents.DotNetBar.ButtonX();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intDefSchoolYear)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToDeleteRows = false;
            this.dataGridViewX1.AllowUserToResizeRows = false;
            this.dataGridViewX1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8});
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle24.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle24.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle24.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle24.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle24;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(12, 48);
            this.dataGridViewX1.MultiSelect = false;
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowHeadersVisible = false;
            this.dataGridViewX1.RowTemplate.Height = 24;
            this.dataGridViewX1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewX1.Size = new System.Drawing.Size(927, 394);
            this.dataGridViewX1.TabIndex = 0;
            this.dataGridViewX1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewX1_CellMouseDoubleClick);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.AutoSize = true;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(864, 451);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // cbDefSemester
            // 
            this.cbDefSemester.DisplayMember = "Text";
            this.cbDefSemester.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbDefSemester.FormattingEnabled = true;
            this.cbDefSemester.ItemHeight = 19;
            this.cbDefSemester.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2});
            this.cbDefSemester.Location = new System.Drawing.Point(233, 10);
            this.cbDefSemester.Name = "cbDefSemester";
            this.cbDefSemester.Size = new System.Drawing.Size(80, 25);
            this.cbDefSemester.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cbDefSemester.TabIndex = 2;
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "1";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "2";
            // 
            // intDefSchoolYear
            // 
            this.intDefSchoolYear.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.intDefSchoolYear.BackgroundStyle.Class = "DateTimeInputBackground";
            this.intDefSchoolYear.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.intDefSchoolYear.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.intDefSchoolYear.Location = new System.Drawing.Point(79, 10);
            this.intDefSchoolYear.MaxValue = 999;
            this.intDefSchoolYear.MinValue = 100;
            this.intDefSchoolYear.Name = "intDefSchoolYear";
            this.intDefSchoolYear.ShowUpDown = true;
            this.intDefSchoolYear.Size = new System.Drawing.Size(80, 25);
            this.intDefSchoolYear.TabIndex = 3;
            this.intDefSchoolYear.Value = 107;
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
            this.labelX1.Location = new System.Drawing.Point(12, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(47, 21);
            this.labelX1.TabIndex = 4;
            this.labelX1.Text = "學年度";
            // 
            // labelX2
            // 
            this.labelX2.AutoSize = true;
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(179, 12);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(34, 21);
            this.labelX2.TabIndex = 5;
            this.labelX2.Text = "學期";
            // 
            // btnStart
            // 
            this.btnStart.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStart.AutoSize = true;
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnStart.Location = new System.Drawing.Point(350, 10);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(78, 25);
            this.btnStart.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "查詢社團數";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnViewClub
            // 
            this.btnViewClub.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnViewClub.AutoSize = true;
            this.btnViewClub.BackColor = System.Drawing.Color.Transparent;
            this.btnViewClub.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnViewClub.Location = new System.Drawing.Point(783, 451);
            this.btnViewClub.Name = "btnViewClub";
            this.btnViewClub.Size = new System.Drawing.Size(75, 25);
            this.btnViewClub.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnViewClub.TabIndex = 8;
            this.btnViewClub.Text = "檢視詳細";
            this.btnViewClub.Click += new System.EventHandler(this.btnViewClub_Click);
            // 
            // Column1
            // 
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.LightCyan;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle17;
            this.Column1.HeaderText = "學校";
            this.Column1.Name = "Column1";
            this.Column1.Width = 200;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "社團數";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.LightCyan;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle18;
            this.Column3.HeaderText = "1年級節數";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.LightCyan;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle19;
            this.Column4.HeaderText = "隔周上課";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.LightCyan;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle20;
            this.Column5.HeaderText = "2年級節數";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            dataGridViewCellStyle21.BackColor = System.Drawing.Color.LightCyan;
            this.Column6.DefaultCellStyle = dataGridViewCellStyle21;
            this.Column6.HeaderText = "隔周上課";
            this.Column6.Name = "Column6";
            // 
            // Column7
            // 
            dataGridViewCellStyle22.BackColor = System.Drawing.Color.LightCyan;
            this.Column7.DefaultCellStyle = dataGridViewCellStyle22;
            this.Column7.HeaderText = "3年級節數";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            dataGridViewCellStyle23.BackColor = System.Drawing.Color.LightCyan;
            this.Column8.DefaultCellStyle = dataGridViewCellStyle23;
            this.Column8.HeaderText = "隔周上課";
            this.Column8.Name = "Column8";
            // 
            // frmClubStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 484);
            this.Controls.Add(this.btnViewClub);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.intDefSchoolYear);
            this.Controls.Add(this.cbDefSemester);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.dataGridViewX1);
            this.DoubleBuffered = true;
            this.Name = "frmClubStatus";
            this.Text = "社團查詢";
            this.Load += new System.EventHandler(this.frmClubStatus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intDefSchoolYear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cbDefSemester;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.IntegerInput intDefSchoolYear;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX btnStart;
        private DevComponents.DotNetBar.ButtonX btnViewClub;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}