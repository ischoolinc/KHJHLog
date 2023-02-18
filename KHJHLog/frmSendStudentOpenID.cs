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

namespace KHJHLog
{
    public partial class frmSendStudentOpenID : BaseForm
    {

        List<StudentOpenIDInfo> StudentOpenIDInfoList;
        // 學校代碼 學校稱對照
        Dictionary<string, SchoolOpenIDInfo> SchoolCodeShoolDict;

        // 學校名稱 ID 對照
        Dictionary<string, string> SchoolNameIDDict;

        public frmSendStudentOpenID()
        {
            InitializeComponent();
            StudentOpenIDInfoList = new List<StudentOpenIDInfo>();
            SchoolNameIDDict = new Dictionary<string, string>();
            SchoolCodeShoolDict = new Dictionary<string, SchoolOpenIDInfo>();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //   傳送按鈕不能使用
            btnSend.Enabled = false;

            // 將畫面上學生從104所學校中移除，目的給轉入使用。
            // 檢查畫面上是否有資料
            if (dgData.Rows.Count == 0)
            {
                MsgBox.Show("沒有資料 ");
                return;
            }

            // 建立連線相關資訊
            Connection con = new Connection();
            if (FISCA.Authentication.DSAServices.PassportToken == null)
            {
                FISCA.Presentation.Controls.MsgBox.Show("Greening Passport 認證失敗，請檢查登入帳號!");
            }
            //取得局端登入後Greening發的Passport，並登入指定的Contract
            con.Connect(FISCA.Authentication.DSAServices.DefaultDataSource.AccessPoint, "openid.sync", FISCA.Authentication.DSAServices.PassportToken);

            // 建立學生暫存資訊，並取得畫面上學生、身分證號、姓名、性別		 
            Dictionary<string, StudentOpenIDInfo> StudInfoDict = new Dictionary<string, StudentOpenIDInfo>();
            foreach (DataGridViewRow drv in dgData.Rows)
            {
                if (drv.IsNewRow)
                    continue;

                string idNumber = drv.Cells["身分證號"].Value.ToString();
                if (!StudInfoDict.ContainsKey(idNumber))
                {
                    StudentOpenIDInfo si = new StudentOpenIDInfo();
                    si.IDNumber = idNumber;
                    si.Gender = drv.Cells["性別"].Value.ToString();
                    si.Name = drv.Cells["姓名"].Value.ToString();
                    StudInfoDict.Add(idNumber, si);
                }
            }

            // 建立傳送 Request，送出學生IDNumber、Gender、Name，  給Service:GetIDNumberB64Batch，使用Server共通編碼機制，取得身分證邊碼，給移除學校使用。
            XElement elmReq = new XElement("Request");
            foreach (string idNumber in StudInfoDict.Keys)
            {
                XElement elm = new XElement("Student");
                elm.SetElementValue("IDNumber", idNumber);
                elm.SetElementValue("Gender", StudInfoDict[idNumber].Gender);
                elm.SetElementValue("Name", StudInfoDict[idNumber].Name);
                elmReq.Add(elm);
            }
            XmlHelper req = new XmlHelper(elmReq.ToString());

            // 呼叫並取得回傳資料
            try
            {
                Envelope Response = con.SendRequest("_.GetIDNumberB64Batch", new Envelope(req));
                XElement elmResponse = XElement.Load(new StringReader(Response.Body.XmlString));

                int idx = 1;
                //  整理資料
                foreach (DataGridViewRow drv in dgData.Rows)
                {
                    XElement elmReqS = new XElement("Request");


                    StudentOpenIDInfo so = drv.Tag as StudentOpenIDInfo;

                    // 比對回傳資料填值
                    if (so != null)
                    {
                        foreach (XElement elmR in elmResponse.Elements("Rsp"))
                        {
                            string id = Utility.GetElementString(elmR, "IDNumberSource");
                            if (so.IDNumber == id)
                            {
                                so.IDNumberB64 = Utility.GetElementString(elmR, "IDNumberB64");
                                so.GenderB64 = Utility.GetElementString(elmR, "GenderB64");
                                so.NameB64 = Utility.GetElementString(elmR, "NameB64");
                                break;
                            }
                        }
                    }

                    // 跑104所學校移除學生
                    foreach (string code in SchoolCodeShoolDict.Keys)
                    {
                        SchoolOpenIDInfo si = SchoolCodeShoolDict[code];

                        //  so.ImportSchoolID = so.ExportSchoolID = so.SchoolID;
                        string reqRemove = "";
                        reqRemove = @"http://stuadm.kh.edu.tw/service/syncJH/" + si.SchoolID + "/" + so.IDNumberB64 + "/remove";

                        try
                        {
                            XElement elmSt = new XElement("Student");
                            elmSt.SetElementValue("ReqRemove", reqRemove);

                            elmReqS.Add(elmSt);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    // 每位學生送一次(104所學校)
                    XmlHelper reqS = new XmlHelper(elmReqS.ToString());
                    Envelope ResponseS = con.SendRequest("_.SendStudentOpenIDRemove", new Envelope(reqS));
                    XElement elmResponseS = XElement.Load(new StringReader(ResponseS.Body.XmlString));
                }
                // 移除完成
            }
            catch (Exception ex)
            {
                MsgBox.Show("處理移除104學校發生錯誤," + ex.Message);
            }


            // 轉換畫面上學生轉入學校並轉成傳送格式送出，無法轉換不送出
            foreach (DataGridViewRow drv in dgData.Rows)
            {
                if (drv.IsNewRow)
                    continue;

                XElement elmReqS1 = new XElement("Request");
                elmReqS1.SetElementValue("IDNumber", drv.Cells["身分證號"].Value.ToString());
                elmReqS1.SetElementValue("Gender", drv.Cells["性別"].Value.ToString());
                elmReqS1.SetElementValue("Name", drv.Cells["姓名"].Value.ToString());

                XmlHelper reqS1 = new XmlHelper(elmReqS1.ToString());
                Envelope Response = con.SendRequest("_.GetIDNumberB64", new Envelope(reqS1));

                XElement elmResponse = XElement.Load(new StringReader(Response.Body.XmlString));
                StudentOpenIDInfo so = drv.Tag as StudentOpenIDInfo;
                if (so != null)
                {
                    so.IDNumberB64 = Utility.GetElementString(elmResponse, "IDNumberB64");
                    so.GenderB64 = Utility.GetElementString(elmResponse, "GenderB64");
                    so.NameB64 = Utility.GetElementString(elmResponse, "NameB64");
                }

                string reqRemove = "", req1 = "", req2 = "", req3 = "";
                if (!string.IsNullOrEmpty(so.ExportSchoolID))
                {
                    reqRemove = @"http://stuadm.kh.edu.tw/service/syncJH/" + so.ExportSchoolID + "/" + so.IDNumberB64 + "/remove";
                }

                // 有解析轉入學校才送送
                if (!string.IsNullOrEmpty(so.ImportSchoolID))
                {
                    req1 = @"http://stuadm.kh.edu.tw/service/syncJH/" + so.ImportSchoolID + "/" + so.IDNumber + "/init";
                    req2 = @"http://stuadm.kh.edu.tw/service/syncJH/" + so.ImportSchoolID + "/" + so.IDNumberB64 + "/" + so.NameB64 + "/" + so.GenderB64 + "/" + so.BirthDate;
                    req3 = @"http://stuadm.kh.edu.tw/service/syncJH/" + so.ImportSchoolID + "/" + so.IDNumberB64 + "/D/J/0/" + so.ClassName + "/" + so.SeatNo + "/" + so.StudentNumer;

                    // 檢查資料完成才送出
                    bool pass = true;

                    if (string.IsNullOrEmpty(so.IDNumberB64) || string.IsNullOrEmpty(so.NameB64) || string.IsNullOrEmpty(so.GenderB64) || string.IsNullOrEmpty(so.BirthDate) || string.IsNullOrEmpty(so.ClassName) || string.IsNullOrEmpty(so.SeatNo) || string.IsNullOrEmpty(so.StudentNumer))
                    {
                        pass = false;
                    }

                    if (pass)
                    {
                        XElement elmReqS = new XElement("Request");
                        elmReqS.SetElementValue("ReqRemove", reqRemove);
                        elmReqS.SetElementValue("Req1", req1);
                        elmReqS.SetElementValue("Req2", req2);
                        elmReqS.SetElementValue("Req3", req3);
                        elmReqS.SetElementValue("SchoolName", so.ImportSchoolName);
                        elmReqS.SetElementValue("StudentName", so.Name);
                        elmReqS.SetElementValue("StudentNumber", so.StudentNumer);
                        elmReqS.SetElementValue("IDNumber", so.IDNumber);

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
                        Utility.WriteOpenSendLog("傳送轉學學生OpenID", elmReqS.ToString(), elmResponseS.ToString());
                    }
                }
            }

            MsgBox.Show("傳送完成");
            btnSend.Enabled = true;

        }

        private void frmSendStudentOpenID_Load(object sender, EventArgs e)
        {
            // this.MaximumSize = this.MinimumSize = this.Size;

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

            // 載入欄位
            LoadDataGridViewColumns();

        }

        private void LoadSchoolNameDict()
        {
            SchoolNameIDDict.Clear();
            List<SchoolOpenIDInfo> schoolList = Utility.GetSchoolOpenIDInfoList();

            foreach (SchoolOpenIDInfo si in schoolList)
            {
                if (!SchoolNameIDDict.ContainsKey(si.SchoolName))
                    SchoolNameIDDict.Add(si.SchoolName, si.SchoolID);
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
                            if (p2.Count > 1)
                            {
                                so.ExportSchoolName = p2[0].Trim();
                                if (so.ExportSchoolName.Length > 3)
                                    so.ExportSchoolID = GetSchoolNameID(so.ExportSchoolName.Substring(0, 4));

                                so.ImportSchoolName = p2[1].Trim();
                                if (so.ImportSchoolName.Length > 3)
                                    so.ImportSchoolID = GetSchoolNameID(so.ImportSchoolName.Substring(0, 4));
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

        private void btnExcel_Click(object sender, EventArgs e)
        {
            btnExcel.Enabled = false;

            if (dgData.Rows.Count > 0)
            {
                Workbook wb = new Workbook();
                Worksheet wst = wb.Worksheets[0];
                int rowIdx = 1, colIdx = 0;
                foreach (DataGridViewColumn col in dgData.Columns)
                {
                    wst.Cells[0, colIdx].PutValue(col.HeaderText);
                    colIdx++;
                }

                foreach (DataGridViewRow dr in dgData.Rows)
                {
                    if (dr.IsNewRow)
                        continue;
                    colIdx = 0;
                    foreach (DataGridViewCell cell in dr.Cells)
                    {
                        if (cell.Value != null)
                        {
                            wst.Cells[rowIdx, colIdx].PutValue(cell.Value.ToString());

                        }
                        colIdx++;
                    }
                    rowIdx++;
                }
                Utility.ExprotXls("傳送轉學學生OpenID", wb);
            }

            btnExcel.Enabled = true;
        }

    }
}