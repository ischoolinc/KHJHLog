namespace KHJHLog
{
    partial class frmVerifyLock
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
            this.labSchool = new DevComponents.DotNetBar.LabelX();
            this.txtSchool = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.txtDistrictComment = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnConduct = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.cboAction = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.txtClass = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.SuspendLayout();
            // 
            // labSchool
            // 
            this.labSchool.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labSchool.BackgroundStyle.Class = "";
            this.labSchool.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labSchool.Location = new System.Drawing.Point(12, 25);
            this.labSchool.Name = "labSchool";
            this.labSchool.Size = new System.Drawing.Size(75, 23);
            this.labSchool.TabIndex = 0;
            this.labSchool.Text = "學校名稱：";
            // 
            // txtSchool
            // 
            this.txtSchool.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtSchool.Border.Class = "TextBoxBorder";
            this.txtSchool.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtSchool.Enabled = false;
            this.txtSchool.Location = new System.Drawing.Point(93, 27);
            this.txtSchool.Name = "txtSchool";
            this.txtSchool.Size = new System.Drawing.Size(317, 25);
            this.txtSchool.TabIndex = 1;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(12, 105);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 2;
            this.labelX1.Text = "動作：";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(12, 143);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(64, 23);
            this.labelX2.TabIndex = 3;
            this.labelX2.Text = "備註：";
            // 
            // txtDistrictComment
            // 
            this.txtDistrictComment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtDistrictComment.Border.Class = "TextBoxBorder";
            this.txtDistrictComment.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtDistrictComment.Location = new System.Drawing.Point(93, 145);
            this.txtDistrictComment.Multiline = true;
            this.txtDistrictComment.Name = "txtDistrictComment";
            this.txtDistrictComment.Size = new System.Drawing.Size(331, 146);
            this.txtDistrictComment.TabIndex = 4;
            this.txtDistrictComment.TextChanged += new System.EventHandler(this.textBoxX2_TextChanged);
            // 
            // btnConduct
            // 
            this.btnConduct.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnConduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConduct.BackColor = System.Drawing.Color.Transparent;
            this.btnConduct.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnConduct.Location = new System.Drawing.Point(258, 315);
            this.btnConduct.Name = "btnConduct";
            this.btnConduct.Size = new System.Drawing.Size(75, 23);
            this.btnConduct.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnConduct.TabIndex = 6;
            this.btnConduct.Text = "執行";
            this.btnConduct.Click += new System.EventHandler(this.btnConduct_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(349, 315);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "取消";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // cboAction
            // 
            this.cboAction.DisplayMember = "Text";
            this.cboAction.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAction.FormattingEnabled = true;
            this.cboAction.ItemHeight = 19;
            this.cboAction.Location = new System.Drawing.Point(93, 105);
            this.cboAction.Name = "cboAction";
            this.cboAction.Size = new System.Drawing.Size(121, 25);
            this.cboAction.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cboAction.TabIndex = 8;
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(12, 64);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 9;
            this.labelX3.Text = "班級名稱：";
            // 
            // txtClass
            // 
            this.txtClass.AcceptsReturn = true;
            // 
            // 
            // 
            this.txtClass.Border.Class = "TextBoxBorder";
            this.txtClass.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtClass.Enabled = false;
            this.txtClass.Location = new System.Drawing.Point(93, 66);
            this.txtClass.Name = "txtClass";
            this.txtClass.Size = new System.Drawing.Size(122, 25);
            this.txtClass.TabIndex = 10;
            // 
            // frmVerifyLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 350);
            this.Controls.Add(this.txtClass);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.cboAction);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnConduct);
            this.Controls.Add(this.txtDistrictComment);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.txtSchool);
            this.Controls.Add(this.labSchool);
            this.DoubleBuffered = true;
            this.Name = "frmVerifyLock";
            this.Text = "申請鎖班審核";
            this.Load += new System.EventHandler(this.frmVerifyLock_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labSchool;
        private DevComponents.DotNetBar.Controls.TextBoxX txtSchool;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.TextBoxX txtDistrictComment;
        private DevComponents.DotNetBar.ButtonX btnConduct;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboAction;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.Controls.TextBoxX txtClass;
    }
}