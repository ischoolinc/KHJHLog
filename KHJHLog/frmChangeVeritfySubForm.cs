using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KHJHLog
{
    public partial class frmChangeVeritfySubForm : FISCA.Presentation.Controls.BaseForm
    {
        string _SelectItem = "";        

        public frmChangeVeritfySubForm()
        {
            InitializeComponent();
        }

        private void frmChangeVeritfySubForm_Load(object sender, EventArgs e)
        {
            this.MinimumSize = this.MaximumSize = this.Size;
            // select itenms
            cboItem.Items.Add("");
            cboItem.Items.Add("通過");
            cboItem.Items.Add("不通過");
            cboItem.Items.Add("待修正");
            cboItem.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this._SelectItem = cboItem.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>
        /// 取得選項
        /// </summary>
        /// <returns></returns>
        public string GetSelectItem()
        {
            return _SelectItem;
        }
    }
}
