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
    public partial class frmSendClassOpenIDBatch : BaseForm
    {
        List<SchoolOpenIDInfo> SchoolInfoList;
        // 學校代碼 學校名稱對照
        Dictionary<string, string> SchoolCodeNameDict;

        // 學校代碼 學校ID對照
        Dictionary<string, string> SchoolCodeIDDict;

        List<ClassOpenIDInfo> ClassOpenIDInfoList;

        public frmSendClassOpenIDBatch()
        {
            InitializeComponent();
            SchoolInfoList = new List<SchoolOpenIDInfo>();
            SchoolCodeNameDict = new Dictionary<string, string>();
            SchoolCodeIDDict = new Dictionary<string, string>();
            ClassOpenIDInfoList = new List<ClassOpenIDInfo>();

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

                    ClassOpenIDInfoList.Clear();

                    int rowIdx = 1;
                    while (sr.Peek() >= 0)
                    {

                        string str = sr.ReadLine();
                        if (rowIdx > 1)
                        {
                            p1.Clear();
                            p1 = str.Split(';').ToList();
                            //    Console.WriteLine(x1.Count);
                            ClassOpenIDInfo ci = new ClassOpenIDInfo();
                            if (p1.Count > 10)
                            {
                                string ssy = p1[0];
                                if (ssy.Length == 4)
                                {
                                    ci.SchoolYear = ssy.Substring(0, 3);
                                    if (ssy.Substring(3, 1) == "a")
                                        ci.Semester = "1";
                                    else
                                        ci.Semester = "2";
                                }
                                else
                                {
                                    continue;
                                }

                                ci.SchoolCode = p1[1];
                                ci.ClassName = p1[7];
                                ci.ClassDisplayName = p1[8];
                                ci.ClassType = p1[9];
                                ClassOpenIDInfoList.Add(ci);
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

        private void frmSendClassOpenIDBatch_Load(object sender, EventArgs e)
        {
            LoadDataGridViewColumns();
        }

        private void LoadDataGridViewColumns()
        {
            dgData.Columns.Clear();

            DataGridViewTextBoxColumn tbSchoolYear = new DataGridViewTextBoxColumn();
            tbSchoolYear.Name = "學年度";
            tbSchoolYear.Width = 80;
            tbSchoolYear.HeaderText = "學年度";
            tbSchoolYear.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbSemester = new DataGridViewTextBoxColumn();
            tbSemester.Name = "學期";
            tbSemester.Width = 40;
            tbSemester.HeaderText = "學期";
            tbSemester.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

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

            DataGridViewTextBoxColumn tbClassDispaly = new DataGridViewTextBoxColumn();
            tbClassDispaly.Name = "顯示班級";
            tbClassDispaly.Width = 100;
            tbClassDispaly.HeaderText = "顯示班級";
            tbClassDispaly.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbClassType = new DataGridViewTextBoxColumn();
            tbClassType.Name = "類別";
            tbClassType.Width = 80;
            tbClassType.HeaderText = "類別";
            tbClassType.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgData.Columns.Add(tbSchoolYear);
            dgData.Columns.Add(tbSemester);
            dgData.Columns.Add(tbSchoolCode);
            dgData.Columns.Add(tbClassName);
            dgData.Columns.Add(tbClassDispaly);
            dgData.Columns.Add(tbClassType);

        }

        private void LoadDataToDataGridView()
        {
            dgData.Rows.Clear();
            foreach (ClassOpenIDInfo ci in ClassOpenIDInfoList)
            {
                int rowIdx = dgData.Rows.Add();
                dgData.Rows[rowIdx].Tag = ci;
                dgData.Rows[rowIdx].Cells["學年度"].Value = ci.SchoolYear;
                dgData.Rows[rowIdx].Cells["學期"].Value = ci.Semester;
                dgData.Rows[rowIdx].Cells["學校代碼"].Value = ci.SchoolCode;
                dgData.Rows[rowIdx].Cells["班級"].Value = ci.ClassName;
                dgData.Rows[rowIdx].Cells["顯示班級"].Value = ci.ClassDisplayName;
                dgData.Rows[rowIdx].Cells["類別"].Value = ci.ClassType;
            }
            lblCount.Text = "共 " + dgData.Rows.Count + " 筆";
        }
    }
}
