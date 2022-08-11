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
using System.IO;

namespace KHJHLog
{
    public partial class frmSendStudentOpenIDBatch : BaseForm
    {
        List<SchoolOpenIDInfo> SchoolInfoList;
        // 學校代碼 學校名稱對照
        Dictionary<string, string> SchoolCodeNameDict;

        // 學校代碼 學校ID對照
        Dictionary<string, string> SchoolCodeIDDict;

        List<StudentOpenIDInfo> StudentOpenIDInfoList;

        public frmSendStudentOpenIDBatch()
        {
            InitializeComponent();
            SchoolInfoList = new List<SchoolOpenIDInfo>();
            SchoolCodeNameDict = new Dictionary<string, string>();
            SchoolCodeIDDict = new Dictionary<string, string>();
            StudentOpenIDInfoList = new List<StudentOpenIDInfo>();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

        }

        private void btnReadTextFile_Click(object sender, EventArgs e)
        {
            btnReadTextFile.Enabled = false;
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.FileName = "請選擇文字檔";
                ofd.Title = "開啟文字檔";
                ofd.Filter = "Text files (*.txt)|*.txt";
                List<string> p1 = new List<string>();
                List<string> p2 = new List<string>();

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    StreamReader sr = new StreamReader(ofd.FileName, Encoding.UTF8);

                    StudentOpenIDInfoList.Clear();

                    int rowIdx = 1;
                    while (sr.Peek() >= 0)
                    {

                        string str = sr.ReadLine();
                        if (rowIdx > 1)
                        {
                            p1.Clear();
                            p1 = str.Split(';').ToList();
                            //    Console.WriteLine(x1.Count);
                            StudentOpenIDInfo si = new StudentOpenIDInfo();
                            if (p1.Count > 11)
                            {
                                si.IDNumber = p1[0];
                                si.Name = p1[1];
                                si.Gender = p1[2];
                                si.BirthDate = p1[3];
                                si.SchoolCode = p1[4];
                                si.ClassName = p1[9];
                                si.SeatNo = p1[10];
                                si.StudentNumer = p1[11];

                                StudentOpenIDInfoList.Add(si);
                            }
                        }
                        rowIdx++;
                    }
                    LoadDataToDataGridView();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            btnReadTextFile.Enabled = true;
        }

        private void frmSendStudentOpenIDBatch_Load(object sender, EventArgs e)
        {
            LoadDataGridViewColumns();
        }

        private void LoadDataGridViewColumns()
        {
            dgData.Columns.Clear();

            DataGridViewTextBoxColumn tbIDNumber = new DataGridViewTextBoxColumn();
            tbIDNumber.Name = "身分證字號";
            tbIDNumber.Width = 100;
            tbIDNumber.HeaderText = "身分證字號";
            tbIDNumber.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbName = new DataGridViewTextBoxColumn();
            tbName.Name = "姓名";
            tbName.Width = 100;
            tbName.HeaderText = "姓名";
            tbName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbGender = new DataGridViewTextBoxColumn();
            tbGender.Name = "性別";
            tbGender.Width = 40;
            tbGender.HeaderText = "性別";
            tbGender.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbBirthdate = new DataGridViewTextBoxColumn();
            tbBirthdate.Name = "生日";
            tbBirthdate.Width = 100;
            tbBirthdate.HeaderText = "生日";
            tbBirthdate.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbSchoolCode = new DataGridViewTextBoxColumn();
            tbSchoolCode.Name = "學校代碼";
            tbSchoolCode.Width = 100;
            tbSchoolCode.HeaderText = "學校代碼";
            tbSchoolCode.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbClassName = new DataGridViewTextBoxColumn();
            tbClassName.Name = "班級";
            tbClassName.Width = 100;
            tbClassName.HeaderText = "班級";
            tbClassName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbSeatNo = new DataGridViewTextBoxColumn();
            tbSeatNo.Name = "座號";
            tbSeatNo.Width = 40;
            tbSeatNo.HeaderText = "座號";
            tbSeatNo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbStudentNumber = new DataGridViewTextBoxColumn();
            tbStudentNumber.Name = "學號";
            tbStudentNumber.Width = 100;
            tbStudentNumber.HeaderText = "學號";
            tbStudentNumber.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgData.Columns.Add(tbIDNumber);
            dgData.Columns.Add(tbName);
            dgData.Columns.Add(tbGender);
            dgData.Columns.Add(tbBirthdate);
            dgData.Columns.Add(tbSchoolCode);
            dgData.Columns.Add(tbClassName);
            dgData.Columns.Add(tbSeatNo);
            dgData.Columns.Add(tbStudentNumber);

        }

        private void LoadDataToDataGridView()
        {
            dgData.Rows.Clear();
            foreach ( StudentOpenIDInfo si in StudentOpenIDInfoList)
            {
                int rowIdx = dgData.Rows.Add();
                dgData.Rows[rowIdx].Tag = si;
                dgData.Rows[rowIdx].Cells["身分證字號"].Value = si.IDNumber;
                dgData.Rows[rowIdx].Cells["姓名"].Value = si.Name;
                dgData.Rows[rowIdx].Cells["性別"].Value = si.Gender;
                dgData.Rows[rowIdx].Cells["生日"].Value = si.BirthDate;
                dgData.Rows[rowIdx].Cells["學校代碼"].Value = si.SchoolCode;
                dgData.Rows[rowIdx].Cells["班級"].Value = si.ClassName;
                dgData.Rows[rowIdx].Cells["座號"].Value = si.SeatNo;
                dgData.Rows[rowIdx].Cells["學號"].Value = si.StudentNumer;

            }
            lblCount.Text = "共 " + dgData.Rows.Count + " 筆";
        }
    }
}

