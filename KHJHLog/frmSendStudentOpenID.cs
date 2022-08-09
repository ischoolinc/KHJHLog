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
using FISCA.Data;
using System.Net;

namespace KHJHLog
{
    public partial class frmSendStudentOpenID : BaseForm
    {

        List<StudentOpenIDInfo> StudentOpenIDInfoList;
        // 學校名稱 ID 對照
        Dictionary<string, string> SchoolNameIDDict;

        public frmSendStudentOpenID()
        {
            InitializeComponent();
            StudentOpenIDInfoList = new List<StudentOpenIDInfo>();
            SchoolNameIDDict = new Dictionary<string, string>();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            btnSend.Enabled = false;

            Connection con = new Connection();

            if (FISCA.Authentication.DSAServices.PassportToken == null)
            {
                FISCA.Presentation.Controls.MsgBox.Show("Greening Passport 認證失敗，請檢查登入帳號!");
            }

            //取得局端登入後Greening發的Passport，並登入指定的Contract
            con.Connect(FISCA.Authentication.DSAServices.DefaultDataSource.AccessPoint, "openid.sync", FISCA.Authentication.DSAServices.PassportToken);

            foreach (DataGridViewRow drv in dgData.Rows)
            {
                if (drv.IsNewRow)
                    continue;

                XElement elmReq = new XElement("Request");
                elmReq.SetElementValue("IDNumber", drv.Cells["身分證號"].Value.ToString());
                elmReq.SetElementValue("Gender", drv.Cells["性別"].Value.ToString());
                elmReq.SetElementValue("Name", drv.Cells["姓名"].Value.ToString());

                XmlHelper req = new XmlHelper(elmReq.ToString());
                Envelope Response = con.SendRequest("_.GetIDNumberB64", new Envelope(req));

                XElement elmResponse = XElement.Load(new StringReader(Response.Body.XmlString));
                StudentOpenIDInfo so = drv.Tag as StudentOpenIDInfo;
                if (so != null)
                {
                    so.IDNumberB64 = GetElementString(elmResponse, "IDNumberB64");
                    so.GenderB64 = GetElementString(elmResponse, "GenderB64");
                    so.NameB64 = GetElementString(elmResponse, "NameB64");
                }

                string reqRemove = "", req1 = "", req2 = "", req3 = "";
                if (!string.IsNullOrEmpty(so.ExportSchoolID))
                {
                    reqRemove = @"http://stuadm.kh.edu.tw/service/syncJH/" + so.ExportSchoolID + "/"+ so.IDNumberB64 + "/remove";
                }

                if (!string.IsNullOrEmpty(so.ImportSchoolID))
                {
                    req1 = @"http://stuadm.kh.edu.tw/service/syncJH/" + so.ImportSchoolID + "/" + so.IDNumber + "/init";
                    req2 = @"http://stuadm.kh.edu.tw/service/syncJH/" + so.ImportSchoolID + "/" + so.IDNumberB64 + "/" + so.NameB64 + "/" + so.GenderB64 + "/" + so.BirthDate;
                    req3 = @"http://stuadm.kh.edu.tw/service/syncJH/" + so.ImportSchoolID + "/" + so.IDNumberB64 + "/D/J/0/" + so.ClassName + "/" + so.SeatNo + "/" + so.StudentNumer;
                }

                XElement elmReqS = new XElement("Request");
                elmReqS.SetElementValue("ReqRemove", reqRemove);
                elmReqS.SetElementValue("Req1", req1);
                elmReqS.SetElementValue("Req2", req2);
                elmReqS.SetElementValue("Req3", req3);

                XmlHelper reqS = new XmlHelper(elmReqS.ToString());
                Envelope ResponseS = con.SendRequest("_.SendStudentOpenID", new Envelope(reqS));

                XElement elmResponseS = XElement.Load(new StringReader(ResponseS.Body.XmlString));
                so.RspRemove = GetElementString(elmResponseS, "RspRemove");
                so.RspReq1 = GetElementString(elmResponseS, "Rsp1");
                so.RspReq2 = GetElementString(elmResponseS, "Rsp2");
                so.RspReq3 = GetElementString(elmResponseS, "Rsp3");
                drv.Tag = so;
                drv.Cells["移除轉出學校"].Value = so.RspRemove;
                drv.Cells["呼叫回傳1"].Value = so.RspReq1;
                drv.Cells["呼叫回傳2"].Value = so.RspReq2;
                drv.Cells["呼叫回傳3"].Value = so.RspReq3;

            }

            btnSend.Enabled = true;

        }

        private string GetElementString(XElement elm, string name)
        {
            string value = "";
            if (elm.Element(name) != null)
                value = elm.Element(name).Value;

            return value;
        }

        private void frmSendStudentOpenID_Load(object sender, EventArgs e)
        {
            // this.MaximumSize = this.MinimumSize = this.Size;
            // 載入欄位
            LoadDataGridViewColumns();

        }

        private void LoadSchoolNameDict()
        {
            SchoolNameIDDict.Clear();
            string qry = "SELECT school_name,school_id,dsns FROM $openid.school.list";
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(qry);
            foreach (DataRow dr in dt.Rows)
            {
                string SchoolName = dr["school_name"] + "";
                string SchoolID = dr["school_id"] + "";
                if (!SchoolNameIDDict.ContainsKey(SchoolName))
                    SchoolNameIDDict.Add(SchoolName, SchoolID);
            }

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
                LoadSchoolNameDict();

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    StreamReader sr = new StreamReader(ofd.FileName, Encoding.UTF8);

                    StudentOpenIDInfoList.Clear();

                    while (sr.Peek() >= 0)
                    {
                        string str = sr.ReadLine();
                        p1.Clear();
                        p1 = str.Split(';').ToList();
                        //    Console.WriteLine(x1.Count);
                        StudentOpenIDInfo so = new StudentOpenIDInfo();
                        if (p1.Count >= 13)
                        {
                            so.IDNumber = p1[0].Trim();
                            so.Name = p1[1].Trim();
                            so.Gender = p1[2].Trim();
                            so.BirthDate = p1[3].Trim();

                            so.ClassName = p1[9].Trim();
                            so.SeatNo = p1[10].Trim();
                            so.StudentNumer = p1[11].Trim();
                            string school = p1[13].Replace("(", "").Replace(")", "");
                            p2.Clear();
                            p2 = school.Split('>').ToList();
                            if (p2.Count == 2)
                            {
                                so.ExportSchoolName = p2[0].Trim();
                                so.ExportSchoolID = GetSchoolNameID(so.ExportSchoolName.Substring(0, 2));
                                so.ImportSchoolName = p2[1].Trim();
                                so.ImportSchoolID = GetSchoolNameID(so.ImportSchoolName.Substring(0, 2));
                            }

                            StudentOpenIDInfoList.Add(so);
                        }
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

        private string GetSchoolNameID(string name)
        {
            string value = "";
            foreach (string key in SchoolNameIDDict.Keys)
            {
                if (key.Contains(name))
                {
                    value = SchoolNameIDDict[key];
                }
            }
            return value;
        }

        private void LoadDataGridViewColumns()
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

            DataGridViewTextBoxColumn tbGender = new DataGridViewTextBoxColumn();
            tbGender.Name = "性別";
            tbGender.Width = 60;
            tbGender.HeaderText = "性別";
            tbGender.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbBirthDate = new DataGridViewTextBoxColumn();
            tbBirthDate.Name = "生日";
            tbBirthDate.Width = 100;
            tbBirthDate.HeaderText = "生日";
            tbBirthDate.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbClassName = new DataGridViewTextBoxColumn();
            tbClassName.Name = "班級";
            tbClassName.Width = 100;
            tbClassName.HeaderText = "班級";
            tbClassName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbSeatNo = new DataGridViewTextBoxColumn();
            tbSeatNo.Name = "座號";
            tbSeatNo.Width = 60;
            tbSeatNo.HeaderText = "座號";
            tbSeatNo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbStudentNumber = new DataGridViewTextBoxColumn();
            tbStudentNumber.Name = "學號";
            tbStudentNumber.Width = 100;
            tbStudentNumber.HeaderText = "學號";
            tbStudentNumber.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbExportSchool = new DataGridViewTextBoxColumn();
            tbExportSchool.Name = "轉出學校";
            tbExportSchool.Width = 100;
            tbExportSchool.HeaderText = "轉出學校";
            tbExportSchool.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbImportSchool = new DataGridViewTextBoxColumn();
            tbImportSchool.Name = "轉入學校";
            tbImportSchool.Width = 100;
            tbImportSchool.HeaderText = "轉入學校";
            tbImportSchool.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //DataGridViewTextBoxColumn tbExportSchoolID = new DataGridViewTextBoxColumn();
            //tbExportSchoolID.Name = "轉出學校ID";
            //tbExportSchoolID.Width = 100;
            //tbExportSchoolID.HeaderText = "轉出學校ID";
            //tbExportSchoolID.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //DataGridViewTextBoxColumn tbImportSchoolID = new DataGridViewTextBoxColumn();
            //tbImportSchoolID.Name = "轉入學校ID";
            //tbImportSchoolID.Width = 100;
            //tbImportSchoolID.HeaderText = "轉入學校ID";
            //tbImportSchoolID.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbRemove = new DataGridViewTextBoxColumn();
            tbRemove.Name = "移除轉出學校";
            tbRemove.Width = 100;
            tbRemove.HeaderText = "移除轉出學校";
            tbRemove.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbRsp1 = new DataGridViewTextBoxColumn();
            tbRsp1.Name = "呼叫回傳1";
            tbRsp1.Width = 100;
            tbRsp1.HeaderText = "呼叫回傳1";
            tbRsp1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbRsp2 = new DataGridViewTextBoxColumn();
            tbRsp2.Name = "呼叫回傳2";
            tbRsp2.Width = 100;
            tbRsp2.HeaderText = "呼叫回傳2";
            tbRsp2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbRsp3 = new DataGridViewTextBoxColumn();
            tbRsp3.Name = "呼叫回傳3";
            tbRsp3.Width = 100;
            tbRsp3.HeaderText = "呼叫回傳3";
            tbRsp3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgData.Columns.Add(tbIDNumber);
            dgData.Columns.Add(tbName);
            dgData.Columns.Add(tbGender);
            dgData.Columns.Add(tbBirthDate);
            dgData.Columns.Add(tbClassName);
            dgData.Columns.Add(tbSeatNo);
            dgData.Columns.Add(tbStudentNumber);
            dgData.Columns.Add(tbExportSchool);
            dgData.Columns.Add(tbImportSchool);
            dgData.Columns.Add(tbRemove);
            dgData.Columns.Add(tbRsp1);
            dgData.Columns.Add(tbRsp2);
            dgData.Columns.Add(tbRsp3);
            //dgData.Columns.Add(tbExportSchoolID);
            //dgData.Columns.Add(tbImportSchoolID);
        }

        private void LoadDataToDataGridView()
        {
            dgData.Rows.Clear();
            foreach (StudentOpenIDInfo so in StudentOpenIDInfoList)
            {
                int rowIdx = dgData.Rows.Add();
                dgData.Rows[rowIdx].Tag = so;
                dgData.Rows[rowIdx].Cells["身分證號"].Value = so.IDNumber;
                dgData.Rows[rowIdx].Cells["姓名"].Value = so.Name;
                dgData.Rows[rowIdx].Cells["性別"].Value = so.Gender;
                dgData.Rows[rowIdx].Cells["生日"].Value = so.BirthDate;
                dgData.Rows[rowIdx].Cells["班級"].Value = so.ClassName;
                dgData.Rows[rowIdx].Cells["座號"].Value = so.SeatNo;
                dgData.Rows[rowIdx].Cells["學號"].Value = so.StudentNumer;
                dgData.Rows[rowIdx].Cells["轉出學校"].Value = so.ExportSchoolName;
                dgData.Rows[rowIdx].Cells["轉入學校"].Value = so.ImportSchoolName;
            }

            lblCount.Text = "共 " + dgData.Rows.Count + " 筆";
        }

    }
}
