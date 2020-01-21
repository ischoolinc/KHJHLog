namespace KHJHLog
{
    partial class frmUnlock
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
            this.lstSchools = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.btnUnlock = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // lstSchools
            // 
            this.lstSchools.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.lstSchools.Border.Class = "ListViewBorder";
            this.lstSchools.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lstSchools.HideSelection = false;
            this.lstSchools.Location = new System.Drawing.Point(12, 66);
            this.lstSchools.Name = "lstSchools";
            this.lstSchools.Size = new System.Drawing.Size(380, 467);
            this.lstSchools.TabIndex = 0;
            this.lstSchools.UseCompatibleStateImageBehavior = false;
            this.lstSchools.View = System.Windows.Forms.View.List;
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(12, 26);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(192, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "針對下列學校進行班級解鎖";
            // 
            // btnUnlock
            // 
            this.btnUnlock.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnUnlock.BackColor = System.Drawing.Color.Transparent;
            this.btnUnlock.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnUnlock.Location = new System.Drawing.Point(253, 14);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(139, 35);
            this.btnUnlock.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnUnlock.TabIndex = 2;
            this.btnUnlock.Text = "執行解鎖";
            this.btnUnlock.Click += new System.EventHandler(this.btnUnlock_Click);
            // 
            // frmUnlock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 545);
            this.Controls.Add(this.lstSchools);
            this.Controls.Add(this.btnUnlock);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.Name = "frmUnlock";
            this.Text = "局端手動解鎖";
            this.Load += new System.EventHandler(this.frmUnlock_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private DevComponents.DotNetBar.Controls.ListViewEx lstSchools;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX btnUnlock;
    }
}