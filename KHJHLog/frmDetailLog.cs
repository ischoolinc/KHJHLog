using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FISCA.Presentation.Controls;

namespace KHJHLog
{
    public partial class frmDetailLog : BaseForm
    {
        private DataGridViewRow Row;

        public frmDetailLog(DataGridViewRow Row)
        {
            InitializeComponent();

            this.Row = Row;
        }

        private void frmDetailLog_Load(object sender, EventArgs e)
        {
            txtDate.Text = "" + Row.Cells["colDate"].Value;
            txtSchool.Text = "" + Row.Cells["colSchool"].Value;
            txtAction.Text = "" + Row.Cells["colAction"].Value;
            txtContent.Text = "" + Row.Cells["colContent"].Value;
            txtComment.Text = "" + Row.Cells["colComment"].Value;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
