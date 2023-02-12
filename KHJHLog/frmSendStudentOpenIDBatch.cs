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
            if (dgData.Rows.Count == 0)
            {
                MsgBox.Show("沒有資料 ");
                return;
            }

            btnSend.Enabled = false;

            Connection con = new Connection();


            if (FISCA.Authentication.DSAServices.PassportToken == null)
            {
                FISCA.Presentation.Controls.MsgBox.Show("Greening Passport 認證失敗，請檢查登入帳號!");
            }

            //取得局端登入後Greening發的Passport，並登入指定的Contract
            con.Connect(FISCA.Authentication.DSAServices.DefaultDataSource.AccessPoint, "openid.sync", FISCA.Authentication.DSAServices.PassportToken);

            int i = 1;
            //lblS.Visible = true;
            //lblS.Text = "0";

            Dictionary<string, StudentOpenIDInfo> StudInfoDict = new Dictionary<string, StudentOpenIDInfo>();
            foreach (DataGridViewRow drv in dgData.Rows)
            {
                if (drv.IsNewRow)
                    continue;

                string idNumber = drv.Cells["身分證字號"].Value.ToString();
                if (!StudInfoDict.ContainsKey(idNumber))
                {
                    StudentOpenIDInfo si = new StudentOpenIDInfo();
                    si.IDNumber = idNumber;
                    si.Gender = drv.Cells["性別"].Value.ToString();
                    si.Name = drv.Cells["姓名"].Value.ToString();
                    StudInfoDict.Add(idNumber, si);
                }
            }

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
            Envelope Response = con.SendRequest("_.GetIDNumberB64Batch", new Envelope(req));
            XElement elmResponse = XElement.Load(new StringReader(Response.Body.XmlString));

            Dictionary<string, XElement> elmReqSDict = new Dictionary<string, XElement>();


            //  XElement elmReqS = new XElement("Request");
            int idx = 1;
            //  整理資料
            foreach (DataGridViewRow drv in dgData.Rows)
            {
                //string className = drv.Cells["班級"].Value.ToString();
                //if (!elmReqSDict.ContainsKey(className))
                //{
                //    XElement elmReqS = new XElement("Request");
                //    elmReqSDict.Add(className, elmReqS);
                //}

                StudentOpenIDInfo so = drv.Tag as StudentOpenIDInfo;

                // 比對填值
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

                so.ImportSchoolID = so.ExportSchoolID = so.SchoolID;
                string reqRemove = "", req1 = "", req2 = "", req3 = "";
                if (!string.IsNullOrEmpty(so.ExportSchoolID))
                {
                    reqRemove = @"http://stuadm.kh.edu.tw/service/syncJH/" + so.ExportSchoolID + "/" + so.IDNumberB64 + "/remove";
                }
                
                try
                {
                    //XElement elmSt = new XElement("Student");
                    //elmSt.SetElementValue("ReqRemove", reqRemove);
                    //elmSt.SetElementValue("Req1", req1);
                    //elmSt.SetElementValue("Req2", req2);
                    //elmSt.SetElementValue("Req3", req3);
                    //elmReqSDict[className].Add(elmSt);

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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


                //lblS.Text = i.ToString();
                i++;

            }


            //// 以班級傳送
            //foreach (string className in elmReqSDict.Keys)
            //{
            //    XmlHelper reqS = new XmlHelper(elmReqSDict[className].ToString());
            //    Envelope ResponseS = con.SendRequest("_.SendStudentOpenIDBatch", new Envelope(reqS));
            //    XElement elmResponseS = XElement.Load(new StringReader(ResponseS.Body.XmlString));
            //    Utility.WriteOpenSendLog("傳送學生OpenID_班級:" + className, elmReqSDict[className].ToString(), elmResponseS.ToString());
            //}

            lblS.Visible = false;
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
                        if (rowIdx >= 1)
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
                Utility.ExprotXls("批次傳送學生OpenID", wb);
            }

            btnExcel.Enabled = true;
        }

        private void btnRemoveOpenID_Click(object sender, EventArgs e)
        {
            if (dgData.Rows.Count == 0)
            {
                MsgBox.Show("沒有資料 ");
                return;
            }

            btnRemoveOpenID.Enabled = false;

            // 建立連線
            Connection con = new Connection();


            if (FISCA.Authentication.DSAServices.PassportToken == null)
            {
                FISCA.Presentation.Controls.MsgBox.Show("Greening Passport 認證失敗，請檢查登入帳號!");
            }

            //取得局端登入後Greening發的Passport，並登入指定的Contract
            con.Connect(FISCA.Authentication.DSAServices.DefaultDataSource.AccessPoint, "openid.sync", FISCA.Authentication.DSAServices.PassportToken);
            

            Dictionary<string, StudentOpenIDInfo> StudInfoDict = new Dictionary<string, StudentOpenIDInfo>();
            foreach (DataGridViewRow drv in dgData.Rows)
            {
                if (drv.IsNewRow)
                    continue;

                string idNumber = drv.Cells["身分證字號"].Value.ToString();
                if (!StudInfoDict.ContainsKey(idNumber))
                {
                    StudentOpenIDInfo si = new StudentOpenIDInfo();
                    si.IDNumber = idNumber;
                    si.Gender = drv.Cells["性別"].Value.ToString();
                    si.Name = drv.Cells["姓名"].Value.ToString();
                    StudInfoDict.Add(idNumber, si);
                }
            }

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
            Envelope Response = con.SendRequest("_.GetIDNumberB64Batch", new Envelope(req));
            XElement elmResponse = XElement.Load(new StringReader(Response.Body.XmlString));

            Dictionary<string, XElement> elmReqSDict = new Dictionary<string, XElement>();


            //  XElement elmReqS = new XElement("Request");
            int idx = 1;
            //  整理資料
            foreach (DataGridViewRow drv in dgData.Rows)
            {
                string className = drv.Cells["班級"].Value.ToString();
                if (!elmReqSDict.ContainsKey(className))
                {
                    XElement elmReqS = new XElement("Request");
                    elmReqSDict.Add(className, elmReqS);
                }

                StudentOpenIDInfo so = drv.Tag as StudentOpenIDInfo;

                // 比對填值
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

                so.ImportSchoolID = so.ExportSchoolID = so.SchoolID;
                string reqRemove = "", req1 = "", req2 = "", req3 = "";
                if (!string.IsNullOrEmpty(so.ExportSchoolID))
                {
                    reqRemove = @"http://stuadm.kh.edu.tw/service/syncJH/" + so.ExportSchoolID + "/" + so.IDNumberB64 + "/remove";
                }

                try
                {
                    XElement elmSt = new XElement("Student");
                    elmSt.SetElementValue("ReqRemove", reqRemove);

                    elmReqSDict[className].Add(elmSt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }


            // 以班級傳送
            foreach (string className in elmReqSDict.Keys)
            {
                XmlHelper reqS = new XmlHelper(elmReqSDict[className].ToString());
                Envelope ResponseS = con.SendRequest("_.SendStudentOpenIDRemove", new Envelope(reqS));
                XElement elmResponseS = XElement.Load(new StringReader(ResponseS.Body.XmlString));
                Utility.WriteOpenSendLog("傳送移除學生OpenID_班級:" + className, elmReqSDict[className].ToString(), elmResponseS.ToString());
            }


            lblS.Visible = false;
            MsgBox.Show("傳送完成");
            btnRemoveOpenID.Enabled = true;
        }
    }
}

