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
using FISCA.DSAClient;
using System.Xml.Linq;
using System.IO;
using Aspose.Cells;
using FISCA.Data;

namespace KHJHLog
{
    public partial class frmAutoSyncView : BaseForm
    {
        BackgroundWorker bgWork;
        int Limit = 50;
        bool IsAutoSyncChecked = true;
        DataTable dt;
        public frmAutoSyncView()
        {
            InitializeComponent();
            bgWork = new BackgroundWorker();
            bgWork.DoWork += BwWork_DoWork;
            bgWork.RunWorkerCompleted += BwWork_RunWorkerCompleted;
            bgWork.ProgressChanged += BwWork_ProgressChanged;
            bgWork.WorkerReportsProgress = true;
            dt = new DataTable();
        }

        private void BwWork_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void BwWork_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsAutoSyncChecked)
            {
                LoadAutoSynColumns();
            }
            else
            {
                LoadManSyncColumns();
            }
            btnSearch.Enabled = true;
        }

        private void BwWork_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Limit = intLimit.Value;
            IsAutoSyncChecked = chkAutoSync.Checked;
            btnSearch.Enabled = false;
            bgWork.RunWorkerAsync();
        }

        private void frmAutoSyncView_Load(object sender, EventArgs e)
        {
            // 預設50筆
            intLimit.Value = 50;
            chkAutoSync.Checked = true;
        }

        private void LoadAutoSynColumns()
        {
            dgData.Columns.Clear();
            DataGridViewTextBoxColumn tbIDNumber = new DataGridViewTextBoxColumn();
            tbIDNumber.Name = "身分證號";
            tbIDNumber.Width = 100;
            tbIDNumber.HeaderText = "身分證號";
            tbIDNumber.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbName = new DataGridViewTextBoxColumn();
            tbName.Name = "姓名";
            tbName.Width = 100;
            tbName.HeaderText = "姓名";
            tbName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbStudentNumber = new DataGridViewTextBoxColumn();
            tbStudentNumber.Name = "學號";
            tbStudentNumber.Width = 100;
            tbStudentNumber.HeaderText = "學號";
            tbStudentNumber.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbExportSchool = new DataGridViewTextBoxColumn();
            tbExportSchool.Name = "轉入學校";
            tbExportSchool.Width = 100;
            tbExportSchool.HeaderText = "轉入學校";
            tbExportSchool.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


            DataGridViewTextBoxColumn tbImportSchool = new DataGridViewTextBoxColumn();
            tbImportSchool.Name = "轉入學校";
            tbImportSchool.Width = 100;
            tbImportSchool.HeaderText = "轉入學校";
            tbImportSchool.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbReq1 = new DataGridViewTextBoxColumn();
            tbReq1.Name = "傳送1";
            tbReq1.Width = 100;
            tbReq1.HeaderText = "傳送1";
            tbReq1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbReq2 = new DataGridViewTextBoxColumn();
            tbReq2.Name = "傳送2";
            tbReq2.Width = 100;
            tbReq2.HeaderText = "傳送2";
            tbReq2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbReq3 = new DataGridViewTextBoxColumn();
            tbReq3.Name = "傳送3";
            tbReq3.Width = 100;
            tbReq3.HeaderText = "傳送3";
            tbReq3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbRsp1 = new DataGridViewTextBoxColumn();
            tbRsp1.Name = "回傳1";
            tbRsp1.Width = 100;
            tbRsp1.HeaderText = "回傳1";
            tbRsp1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbRsp2 = new DataGridViewTextBoxColumn();
            tbRsp2.Name = "回傳2";
            tbRsp2.Width = 100;
            tbRsp2.HeaderText = "回傳2";
            tbRsp2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbRsp3 = new DataGridViewTextBoxColumn();
            tbRsp3.Name = "回傳3";
            tbRsp3.Width = 100;
            tbRsp3.HeaderText = "回傳3";
            tbRsp3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgData.Columns.Add(tbIDNumber);
            dgData.Columns.Add(tbName);
            dgData.Columns.Add(tbStudentNumber);
            dgData.Columns.Add(tbExportSchool);
            dgData.Columns.Add(tbImportSchool);
            dgData.Columns.Add(tbReq1);
            dgData.Columns.Add(tbReq2);
            dgData.Columns.Add(tbReq3);
            dgData.Columns.Add(tbRsp1);
            dgData.Columns.Add(tbRsp2);
            dgData.Columns.Add(tbRsp3);

        }

        private void LoadManSyncColumns()
        {
            dgData.Columns.Clear();

            DataGridViewTextBoxColumn tbIDNumber = new DataGridViewTextBoxColumn();
            tbIDNumber.Name = "身分證號";
            tbIDNumber.Width = 100;
            tbIDNumber.HeaderText = "身分證號";
            tbIDNumber.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbName = new DataGridViewTextBoxColumn();
            tbName.Name = "姓名";
            tbName.Width = 100;
            tbName.HeaderText = "姓名";
            tbName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbStudentNumber = new DataGridViewTextBoxColumn();
            tbStudentNumber.Name = "學號";
            tbStudentNumber.Width = 100;
            tbStudentNumber.HeaderText = "學號";
            tbStudentNumber.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbImportSchool = new DataGridViewTextBoxColumn();
            tbImportSchool.Name = "轉入學校";
            tbImportSchool.Width = 100;
            tbImportSchool.HeaderText = "轉入學校";
            tbImportSchool.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbReq1 = new DataGridViewTextBoxColumn();
            tbReq1.Name = "傳送1";
            tbReq1.Width = 100;
            tbReq1.HeaderText = "傳送1";
            tbReq1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbReq2 = new DataGridViewTextBoxColumn();
            tbReq2.Name = "傳送2";
            tbReq2.Width = 100;
            tbReq2.HeaderText = "傳送2";
            tbReq2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbReq3 = new DataGridViewTextBoxColumn();
            tbReq3.Name = "傳送3";
            tbReq3.Width = 100;
            tbReq3.HeaderText = "傳送3";
            tbReq3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbRsp1 = new DataGridViewTextBoxColumn();
            tbRsp1.Name = "回傳1";
            tbRsp1.Width = 100;
            tbRsp1.HeaderText = "回傳1";
            tbRsp1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbRsp2 = new DataGridViewTextBoxColumn();
            tbRsp2.Name = "回傳2";
            tbRsp2.Width = 100;
            tbRsp2.HeaderText = "回傳2";
            tbRsp2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbRsp3 = new DataGridViewTextBoxColumn();
            tbRsp3.Name = "回傳3";
            tbRsp3.Width = 100;
            tbRsp3.HeaderText = "回傳3";
            tbRsp3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgData.Columns.Add(tbIDNumber);
            dgData.Columns.Add(tbName);
            dgData.Columns.Add(tbStudentNumber);
            dgData.Columns.Add(tbImportSchool);
            dgData.Columns.Add(tbReq1);
            dgData.Columns.Add(tbReq2);
            dgData.Columns.Add(tbReq3);
            dgData.Columns.Add(tbRsp1);
            dgData.Columns.Add(tbRsp2);
            dgData.Columns.Add(tbRsp3);

        }

    }
}
