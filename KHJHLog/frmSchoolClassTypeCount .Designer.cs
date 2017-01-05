namespace KHJHLog
{
    partial class frmSchoolClassTypeCount
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
            this.btnQuery = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.lvSchool = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.chkSchool = new DevComponents.DotNetBar.Controls.CheckBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.SuspendLayout();
            // 
            // btnQuery
            // 
            this.btnQuery.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.AutoSize = true;
            this.btnQuery.BackColor = System.Drawing.Color.Transparent;
            this.btnQuery.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnQuery.Location = new System.Drawing.Point(420, 196);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 25);
            this.btnQuery.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "列印報表";
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(522, 196);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lvSchool
            // 
            // 
            // 
            // 
            this.lvSchool.Border.Class = "ListViewBorder";
            this.lvSchool.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lvSchool.CheckBoxes = true;
            this.lvSchool.Location = new System.Drawing.Point(23, 50);
            this.lvSchool.Name = "lvSchool";
            this.lvSchool.Size = new System.Drawing.Size(576, 139);
            this.lvSchool.TabIndex = 6;
            this.lvSchool.UseCompatibleStateImageBehavior = false;
            this.lvSchool.View = System.Windows.Forms.View.List;
            // 
            // chkSchool
            // 
            this.chkSchool.AutoSize = true;
            this.chkSchool.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.chkSchool.BackgroundStyle.Class = "";
            this.chkSchool.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.chkSchool.Location = new System.Drawing.Point(522, 23);
            this.chkSchool.Name = "chkSchool";
            this.chkSchool.Size = new System.Drawing.Size(54, 21);
            this.chkSchool.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.chkSchool.TabIndex = 7;
            this.chkSchool.Text = "全選";
            this.chkSchool.CheckedChanged += new System.EventHandler(this.chkSchool_CheckedChanged);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(23, 21);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(423, 23);
            this.labelX1.TabIndex = 8;
            this.labelX1.Text = "備註:若學校未將班級設定\"班級分類\"，預設一律視該班級為\"普通班\"";
            // 
            // frmSchoolClassTypeCount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 233);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.chkSchool);
            this.Controls.Add(this.lvSchool);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnQuery);
            this.DoubleBuffered = true;
            this.Name = "frmSchoolClassTypeCount";
            this.Text = "各校人數班級類別統計";
            this.Load += new System.EventHandler(this.frmSchoolClassCount_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnQuery;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.Controls.ListViewEx lvSchool;
        private DevComponents.DotNetBar.Controls.CheckBoxX chkSchool;
        private DevComponents.DotNetBar.LabelX labelX1;
    }
}