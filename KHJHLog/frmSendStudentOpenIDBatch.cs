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

namespace KHJHLog
{
    public partial class frmSendStudentOpenIDBatch : BaseForm
    {
        List<SchoolOpenIDInfo> SchoolInfoList;
        // 學校代碼 學校稱對照
        Dictionary<string, SchoolOpenIDInfo> SchoolCodeShoolDict;


        List<StudentOpenIDInfo> StudentOpenIDInfoList;

        public frmSendStudentOpenIDBatch()
        {
            InitializeComponent();
            SchoolInfoList = new List<SchoolOpenIDInfo>();
            SchoolCodeShoolDict = new Dictionary<string, SchoolOpenIDInfo>();
            StudentOpenIDInfoList = new List<StudentOpenIDInfo>();
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
                elmReq.SetElementValue("IDNumber", drv.Cells["身分證字號"].Value.ToString());
                elmReq.SetElementValue("Gender", drv.Cells["性別"].Value.ToString());
                elmReq.SetElementValue("Name", drv.Cells["姓名"].Value.ToString());

                XmlHelper req = new XmlHelper(elmReq.ToString());
                Envelope Response = con.SendRequest("_.GetIDNumberB64", new Envelope(req));

                XElement elmResponse = XElement.Load(new StringReader(Response.Body.XmlString));
                StudentOpenIDInfo so = drv.Tag as StudentOpenIDInfo;
                if (so != null)
                {
                    so.IDNumberB64 = Utility.GetElementString(elmResponse, "IDNumberB64");
                    so.GenderB64 = Utility.GetElementString(elmResponse, "GenderB64");
                    so.NameB64 = Utility.GetElementString(elmResponse, "NameB64");
                }

                so.ImportSchoolID = so.ExportSchoolID = so.SchoolID;
                string reqRemove = "", req1 = "", req2 = "", req3 = "";
                if (!string.IsNullOrEmpty(so.ExportSchoolID))
                {
                    reqRemove = @"http://stuadm.kh.edu.tw/service/syncJH/" + so.ExportSchoolID + "/" + so.IDNumberB64 + "/remove";
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
                so.RspRemove = Utility.GetElementString(elmResponseS, "RspRemove");
                so.RspReq1 = Utility.GetElementString(elmResponseS, "Rsp1");
                so.RspReq2 = Utility.GetElementString(elmResponseS, "Rsp2");
                so.RspReq3 = Utility.GetElementString(elmResponseS, "Rsp3");
                drv.Tag = so;
                drv.Cells["移除轉出學校"].Value = so.RspRemove;
                drv.Cells["呼叫回傳1"].Value = so.RspReq1;
                drv.Cells["呼叫回傳2"].Value = so.RspReq2;
                drv.Cells["呼叫回傳3"].Value = so.RspReq3;

                // 寫入 Log
                Utility.WriteOpenSendLog("傳送學生OpenID", elmReqS.ToString(), elmResponseS.ToString());

            }

            MsgBox.Show("傳送完成");
            btnSend.Enabled = true;

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
                    // 讀取

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
                                if (SchoolCodeShoolDict.ContainsKey(si.SchoolCode))
                                {
                                    si.SchoolID = SchoolCodeShoolDict[si.SchoolCode].SchoolID;
                                    si.SchoolName = SchoolCodeShoolDict[si.SchoolCode].SchoolName;
                                }
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
            btnReadTextFile.Enabled = btnSend.Enabled = false;

            LoadDataGridViewColumns();
            try
            {
                // 讀取學校資訊
                SchoolCodeShoolDict.Clear();
                List<SchoolOpenIDInfo> SchoolOpenIDInfoList = Utility.GetSchoolOpenIDInfoList();
                foreach (SchoolOpenIDInfo si in SchoolOpenIDInfoList)
                {
                    if (!SchoolCodeShoolDict.ContainsKey(si.SchoolCode))
                        SchoolCodeShoolDict.Add(si.SchoolCode, si);
                }
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }

            btnReadTextFile.Enabled = btnSend.Enabled = true;
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

            DataGridViewTextBoxColumn tbSchoolName = new DataGridViewTextBoxColumn();
            tbSchoolName.Name = "學校名稱";
            tbSchoolName.Width = 150;
            tbSchoolName.HeaderText = "學校名稱";
            tbSchoolName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
            dgData.Columns.Add(tbBirthdate);
            dgData.Columns.Add(tbSchoolCode);
            dgData.Columns.Add(tbSchoolName);
            dgData.Columns.Add(tbClassName);
            dgData.Columns.Add(tbSeatNo);
            dgData.Columns.Add(tbStudentNumber);
            dgData.Columns.Add(tbRemove);
            dgData.Columns.Add(tbRsp1);
            dgData.Columns.Add(tbRsp2);
            dgData.Columns.Add(tbRsp3);
        }

        private void LoadDataToDataGridView()
        {
            dgData.Rows.Clear();
            foreach (StudentOpenIDInfo si in StudentOpenIDInfoList)
            {
                int rowIdx = dgData.Rows.Add();
                dgData.Rows[rowIdx].Tag = si;
                dgData.Rows[rowIdx].Cells["身分證字號"].Value = si.IDNumber;
                dgData.Rows[rowIdx].Cells["姓名"].Value = si.Name;
                dgData.Rows[rowIdx].Cells["性別"].Value = si.Gender;
                dgData.Rows[rowIdx].Cells["生日"].Value = si.BirthDate;
                dgData.Rows[rowIdx].Cells["學校代碼"].Value = si.SchoolCode;
                dgData.Rows[rowIdx].Cells["學校名稱"].Value = si.SchoolName;
                dgData.Rows[rowIdx].Cells["班級"].Value = si.ClassName;
                dgData.Rows[rowIdx].Cells["座號"].Value = si.SeatNo;
                dgData.Rows[rowIdx].Cells["學號"].Value = si.StudentNumer;

            }
            lblCount.Text = "共 " + dgData.Rows.Count + " 筆";
        }
    }
}

