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
    public partial class frmSendClassOpenIDBatch : BaseForm
    {
        List<SchoolOpenIDInfo> SchoolInfoList;
        // 學校代碼 學校稱對照
        Dictionary<string, SchoolOpenIDInfo> SchoolCodeShoolDict;

        List<ClassOpenIDInfo> ClassOpenIDInfoList;

        public frmSendClassOpenIDBatch()
        {
            InitializeComponent();
            SchoolInfoList = new List<SchoolOpenIDInfo>();
            SchoolCodeShoolDict = new Dictionary<string, SchoolOpenIDInfo>();
            ClassOpenIDInfoList = new List<ClassOpenIDInfo>();

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


            // http://stuadm.kh.edu.tw/service/syncJHClass/khjh/110b/D/J/0/301

            // 取得班級 B64
            XElement elmReqc = new XElement("Request");
            Dictionary<string, string> classNameB64Dict = new Dictionary<string, string>();
            foreach (DataGridViewRow drv in dgData.Rows)
            {
                ClassOpenIDInfo co = drv.Tag as ClassOpenIDInfo;
                if (co != null)
                {
                    XElement elmc1 = new XElement("ClassRec");
                    elmc1.SetElementValue("ClassName", co.ClassDisplayName);
                    elmReqc.Add(elmc1);
                }
            }

            XmlHelper reqC = new XmlHelper(elmReqc.ToString());
            Envelope ResponseC = con.SendRequest("_.GetClassNameB64Batch", new Envelope(reqC));
            XElement elmResponseC = XElement.Load(new StringReader(ResponseC.Body.XmlString));
            foreach (XElement elmc in elmResponseC.Elements("Rsp"))
            {
                string cName = Utility.GetElementString(elmc, "ClassNameSource");
                string cNameB64 = Utility.GetElementString(elmc, "ClassNameB64");
                if (!classNameB64Dict.ContainsKey(cName))
                    classNameB64Dict.Add(cName, cNameB64);
            }

            XElement elmReq = new XElement("Request");

            foreach (DataGridViewRow drv in dgData.Rows)
            {
                ClassOpenIDInfo co = drv.Tag as ClassOpenIDInfo;
                if (co != null)
                {
                    if (classNameB64Dict.ContainsKey(co.ClassDisplayName))
                    {
                        co.ClassNameB64 = classNameB64Dict[co.ClassDisplayName];
                        drv.Tag = co;
                    }
                }
            }


            foreach (DataGridViewRow drv in dgData.Rows)
            {
                ClassOpenIDInfo co = drv.Tag as ClassOpenIDInfo;
                if (co != null)
                {
                    string value = @"http://stuadm.kh.edu.tw/service/syncJClass/" + co.SchoolID + "/" + co.strSchoolYearSems + "/D/J/0/" + co.ClassName + "/className/" + co.ClassNameB64;
                    XElement elm = new XElement("Req", value);
                    elmReq.Add(elm);
                }

            }

            XmlHelper req = new XmlHelper(elmReq.ToString());

            Envelope Response = con.SendRequest("_.SendData", new Envelope(req));

            XElement elmResponse = XElement.Load(new StringReader(Response.Body.XmlString));

            // 填入回傳
            try
            {
                XElement elmRsp = XElement.Parse(elmResponse.ToString());
                int rowIdx = 0;
                foreach (XElement elm in elmRsp.Elements("Rsp"))
                {
                    if (dgData.Rows[rowIdx] != null)
                    {
                        dgData.Rows[rowIdx].Cells["呼叫回傳"].Value = elm.Value;
                        rowIdx++;
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // 寫入 Log
            Utility.WriteOpenSendLog("傳送班級", elmReq.ToString(), elmResponse.ToString());

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
                    StreamReader sr = new StreamReader(ofd.FileName, Encoding.UTF8);

                    ClassOpenIDInfoList.Clear();

                    int rowIdx = 1;
                    while (sr.Peek() >= 0)
                    {

                        string str = sr.ReadLine();
                        if (rowIdx >= 1)
                        {
                            p1.Clear();
                            p1 = str.Split(';').ToList();
                            //    Console.WriteLine(x1.Count);
                            ClassOpenIDInfo ci = new ClassOpenIDInfo();
                            if (p1.Count > 10)
                            {
                                string ssy = p1[0];
                                ci.strSchoolYearSems = ssy;
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

                                if (SchoolCodeShoolDict.ContainsKey(ci.SchoolCode))
                                {
                                    ci.SchoolName = SchoolCodeShoolDict[ci.SchoolCode].SchoolName;
                                    ci.SchoolID = SchoolCodeShoolDict[ci.SchoolCode].SchoolID;
                                }

                                ci.ClassName = p1[7];

                                // 班級需要轉 7,8,9
                                if (ci.ClassName.Length > 2)
                                {
                                    string ck1 = ci.ClassName.Substring(0, 1);
                                    string ck2 = ci.ClassName.Substring(1, ci.ClassName.Length - 1);
                                    if (ck1 == "1")
                                        ci.ClassName = "7" + ck2;
                                    if (ck1 == "2")
                                        ci.ClassName = "8" + ck2;
                                    if (ck1 == "3")
                                        ci.ClassName = "9" + ck2;
                                }

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

            DataGridViewTextBoxColumn tbRsp = new DataGridViewTextBoxColumn();
            tbRsp.Name = "呼叫回傳";
            tbRsp.Width = 300;
            tbRsp.HeaderText = "呼叫回傳";
            tbRsp.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgData.Columns.Add(tbSchoolYear);
            dgData.Columns.Add(tbSemester);
            dgData.Columns.Add(tbSchoolCode);
            dgData.Columns.Add(tbSchoolName);
            dgData.Columns.Add(tbClassName);
            dgData.Columns.Add(tbClassDispaly);
            dgData.Columns.Add(tbClassType);
            dgData.Columns.Add(tbRsp);
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
                dgData.Rows[rowIdx].Cells["學校名稱"].Value = ci.SchoolName;
                dgData.Rows[rowIdx].Cells["班級"].Value = ci.ClassName;
                dgData.Rows[rowIdx].Cells["顯示班級"].Value = ci.ClassDisplayName;
                dgData.Rows[rowIdx].Cells["類別"].Value = ci.ClassType;
                dgData.Rows[rowIdx].Cells["呼叫回傳"].Value = ci.RspContent;
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
                Utility.ExprotXls("批次傳送班級OpenID", wb);
            }

            btnExcel.Enabled = true;
        }
    }
}
