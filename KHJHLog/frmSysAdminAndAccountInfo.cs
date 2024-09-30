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
using System.Web.Script.Serialization;
using System.IO;
using System.Xml.Linq;
using FISCA.DSAClient;
using FISCA.UDT;
using KHJHLog.DAO;
using FISCA.LogAgent;
using Aspose.Cells;

namespace KHJHLog
{
    public partial class frmSysAdminAndAccountInfo : BaseForm
    {
        // 選擇的類型, 系統管理師,行政帳號
        private string _SelType = "";
        BackgroundWorker _bgWorkerLoadData;
        List<string> _SelectSchoolIDList;
        // 系統管理師
        List<SchoolSysAdminInfo> SchoolSysAdminList;

        // 使用者
        List<SchoolUserInfo> SchoolUserInfoList;

        public frmSysAdminAndAccountInfo()
        {
            InitializeComponent();
            _bgWorkerLoadData = new BackgroundWorker();
            _SelectSchoolIDList = new List<string>();
            SchoolUserInfoList = new List<SchoolUserInfo>();
            SchoolSysAdminList = new List<SchoolSysAdminInfo>();
            _bgWorkerLoadData.DoWork += _bgWorkerLoadData_DoWork;
            _bgWorkerLoadData.RunWorkerCompleted += _bgWorkerLoadData_RunWorkerCompleted;
            _bgWorkerLoadData.ProgressChanged += _bgWorkerLoadData_ProgressChanged;
            _bgWorkerLoadData.WorkerReportsProgress = true;

        }

        private void _bgWorkerLoadData_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("資料讀取中...", e.ProgressPercentage);
        }

        private void _bgWorkerLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("");
            btnQuery.Enabled = btnExport.Enabled = true;
            // 檢查並填入畫面
            try
            {
                if (_SelType == "系統管理師" && SchoolSysAdminList.Count > 0)
                {
                    dgData.Rows.Clear();
                    foreach (SchoolSysAdminInfo ss in SchoolSysAdminList)
                    {
                        int rowIdx = dgData.Rows.Add();
                        dgData.Rows[rowIdx].Cells["學校名稱"].Value = ss.school_name;
                        dgData.Rows[rowIdx].Cells["系統管理師"].Value = ss.associated_with_name;
                        dgData.Rows[rowIdx].Cells["手機"].Value = ss.associated_with_cell_phone;
                        dgData.Rows[rowIdx].Cells["Email"].Value = ss.associated_with_email;
                        dgData.Rows[rowIdx].Cells["修改日期"].Value = ss.server_time;
                    }

                }

                if (_SelType == "行政帳號" && SchoolUserInfoList.Count > 0)
                {
                    dgData.Rows.Clear();
                    foreach (SchoolUserInfo su in SchoolUserInfoList)
                    {
                        int rowIdx = dgData.Rows.Add();
                        dgData.Rows[rowIdx].Cells["學校名稱"].Value = su.SchoolName;
                        dgData.Rows[rowIdx].Cells["使用帳號"].Value = su.Account;
                        dgData.Rows[rowIdx].Cells["最後使用日期"].Value = su.LastUseDate;
                        dgData.Rows[rowIdx].Cells["登入IP"].Value = su.LoginIP;
                        dgData.Rows[rowIdx].Cells["最高權限"].Value = su.HighestPermission;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void _bgWorkerLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            _bgWorkerLoadData.ReportProgress(5);
            SchoolSysAdminList.Clear();
            SchoolUserInfoList.Clear();

            // 取得學選學校資料
            List<School> schoolList = tool._a.Select<School>(_SelectSchoolIDList);

            _bgWorkerLoadData.ReportProgress(30);

            foreach (School sc in schoolList)
            {
                string ScDSNS = sc.DSNS;

                if (_SelType == "系統管理師")
                {
                    try
                    {
                        Connection con = new Connection();

                        //取得局端登入後Greening發的Passport，並登入指定的Contract
                        con.Connect(FISCA.Authentication.DSAServices.DefaultDataSource.AccessPoint, "ischool.kh.central_office.user", FISCA.Authentication.DSAServices.PassportToken);

                        //取得該Contract所發的Passport
                        Envelope Response = con.SendRequest("DS.Base.GetPassportToken", new Envelope());
                        PassportToken Passport = new PassportToken(Response.Body);

                        //TODO：拿此Passport登入各校
                        Connection conSchool = new Connection();
                        conSchool.Connect(ScDSNS, "ischool.kh.central_office", Passport);
                        Response = conSchool.SendRequest("_.GetSysAdminInfo", new Envelope());

                        XElement elmResponse = XElement.Load(new StringReader(Response.Body.XmlString));

                        // 取得學校基本資料內系統管理師與最後一筆修改log
                        SchoolSysAdminInfo ss = new SchoolSysAdminInfo();
                        ss.school_name = GetElementVal(elmResponse, "school_name");
                        ss.associated_with_name = GetElementVal(elmResponse, "associated_with_name");
                        ss.associated_with_cell_phone = GetElementVal(elmResponse, "associated_with_cell_phone");
                        ss.associated_with_email = GetElementVal(elmResponse, "associated_with_email");
                        ss.server_time = GetElementVal(elmResponse, "server_time");

                        SchoolSysAdminList.Add(ss);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                if (_SelType == "行政帳號")
                {
                    try
                    {
                        Connection con = new Connection();

                        //取得局端登入後Greening發的Passport，並登入指定的Contract
                        con.Connect(FISCA.Authentication.DSAServices.DefaultDataSource.AccessPoint, "ischool.kh.central_office.user", FISCA.Authentication.DSAServices.PassportToken);

                        //取得該Contract所發的Passport
                        Envelope Response = con.SendRequest("DS.Base.GetPassportToken", new Envelope());
                        PassportToken Passport = new PassportToken(Response.Body);

                        //TODO：拿此Passport登入各校
                        Connection conSchool = new Connection();
                        conSchool.Connect(ScDSNS, "ischool.kh.central_office", Passport);
                        Response = conSchool.SendRequest("_.GetUserLoginInfo", new Envelope());
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<root>");
                        sb.Append(Response.Body.XmlString);
                        sb.Append("</root>");

                        XElement elmResponse = XElement.Load(new StringReader(sb.ToString()));

                        // 解析資料
                        foreach (XElement elm in elmResponse.Elements("User"))
                        {
                            SchoolUserInfo su = new SchoolUserInfo();

                            // 學校名稱
                            su.SchoolName = GetElementVal(elm, "school_name");

                            // 使用帳號
                            su.Account = GetElementVal(elm, "user_name");
                            string desc = GetElementVal(elm, "description");
                            if (desc != "")
                            {
                                su.Account += "(" + desc + ")";
                            }

                            // 最後使用日期
                            su.LastUseDate = GetElementVal(elm, "server_time");

                            // 登入IP
                            string ci = GetElementVal(elm, "client_info");

                            try
                            {
                                XElement eCi = XElement.Parse(ci);
                                if (eCi != null)
                                {
                                    if (eCi.Element("NetworkAdapterList") != null)
                                    {
                                        if (eCi.Element("NetworkAdapterList").Element("NetworkAdapter") != null)
                                        {
                                            if (eCi.Element("NetworkAdapterList").Element("NetworkAdapter").Element("IPAddress") != null)
                                            {
                                                su.LoginIP = eCi.Element("NetworkAdapterList").Element("NetworkAdapter").Element("IPAddress").Value;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            // 最高權限
                            if (GetElementVal(elm, "sys_admin") == "1")
                            {
                                su.HighestPermission = "是";
                            }
                            else
                            {
                                su.HighestPermission = "否";
                            }
                            SchoolUserInfoList.Add(su);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            _bgWorkerLoadData.ReportProgress(100);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string GetElementVal(XElement elm, string name)
        {
            string value = "";
            if (elm.Element(name) != null)
                value = elm.Element(name).Value;

            return value;

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            btnQuery.Enabled = btnExport.Enabled = false;
            _bgWorkerLoadData.RunWorkerAsync();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            btnExport.Enabled = btnQuery.Enabled = false;
            try
            {
                // 讀取樣板
                Workbook wb = new Workbook(new MemoryStream(Properties.Resources.系統師及帳號資訊樣板));

                // 匯出畫面資料
                if (_SelType == "系統管理師")
                {
                    Worksheet wst = wb.Worksheets["系統管理師"];

                    int rowIdx = 1;
                    foreach(SchoolSysAdminInfo ss in SchoolSysAdminList)
                    {
                        // 學校名稱
                        wst.Cells[rowIdx, 0].PutValue(ss.school_name);

                        // 系統管理師
                        wst.Cells[rowIdx, 1].PutValue(ss.associated_with_name);

                        // 手機
                        wst.Cells[rowIdx, 2].PutValue(ss.associated_with_cell_phone);

                        // Email
                        wst.Cells[rowIdx, 3].PutValue(ss.associated_with_email);

                        // 修改日期
                        wst.Cells[rowIdx, 4].PutValue(ss.server_time);
                        rowIdx++;
                    }
                    wst.AutoFitColumns();
                    wb.Worksheets.RemoveAt("行政帳號");
                }

                if (_SelType == "行政帳號")
                {
                    Worksheet wst = wb.Worksheets["行政帳號"];
                    int rowIdx = 1;
                    foreach (SchoolUserInfo ss in SchoolUserInfoList )
                    {
                        // 學校名稱
                        wst.Cells[rowIdx, 0].PutValue(ss.SchoolName);

                        // 使用帳號
                        wst.Cells[rowIdx, 1].PutValue(ss.Account);

                        // 最後使用日期
                        wst.Cells[rowIdx, 2].PutValue(ss.LastUseDate);

                        // 登入IP
                        wst.Cells[rowIdx, 3].PutValue(ss.LoginIP);
                        // 最高權限
                        wst.Cells[rowIdx, 4].PutValue(ss.HighestPermission);

                        rowIdx++;
                    }
                    wst.AutoFitColumns();
                    wb.Worksheets.RemoveAt("系統管理師");
                }

            }
            catch (Exception ex)
            {
                MsgBox.Show("匯出發生錯誤：", ex.Message);
            }


            btnExport.Enabled = btnQuery.Enabled = true;
        }

        private void frmSysAdminAndAccountInfo_Load(object sender, EventArgs e)
        {
            // 取得使用者所選學校ID
            GetSelectSchoolIDs();


            // 填入選項，使用這只能選這兩個
            this.comboSelType.Items.Add("系統管理師");
            this.comboSelType.Items.Add("行政帳號");
            this.comboSelType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboSelType.SelectedIndex = 0;
            _SelType = "系統管理師";
            LoadDgDataColumns1();
        }

        // 載入 系統管理師 欄位 學校名稱,系統管理師,手機,Email,修改日期
        public void LoadDgDataColumns1()
        {
            try
            {
                dgData.Columns.Clear();
                string textColumnStrig = @"
                    [
                        {
                            ""HeaderText"": ""學校名稱"",
                            ""Name"": ""學校名稱"",
                            ""Width"": 150,
                            ""ReadOnly"": true
                        },
                        {
                            ""HeaderText"": ""系統管理師"",
                            ""Name"": ""系統管理師"",
                            ""Width"": 150,
                            ""ReadOnly"": true
                        },
                        {
                            ""HeaderText"": ""手機"",
                            ""Name"": ""手機"",
                            ""Width"": 120,
                            ""ReadOnly"": true
                        },
                        {
                            ""HeaderText"": ""Email"",
                            ""Name"": ""Email"",
                            ""Width"": 180,
                            ""ReadOnly"": true
                        },
                        {
                            ""HeaderText"": ""修改日期"",
                            ""Name"": ""修改日期"",
                            ""Width"": 150,
                            ""ReadOnly"": true
                        }
                    ]            
                
";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<DataGridViewTextBoxColumnInfo> jsonObjArray = serializer.Deserialize<List<DataGridViewTextBoxColumnInfo>>(textColumnStrig);
                foreach (DataGridViewTextBoxColumnInfo jObj in jsonObjArray)
                {
                    DataGridViewTextBoxColumn dgt = new DataGridViewTextBoxColumn();
                    dgt.Name = jObj.Name;
                    dgt.Width = jObj.Width;
                    dgt.HeaderText = jObj.HeaderText;
                    dgt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgt.ReadOnly = jObj.ReadOnly;
                    dgData.Columns.Add(dgt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        // 載入 行政帳號 欄位 學校名稱,使用帳號,最後使用日期,登入IP,最高權限
        public void LoadDgDataColumns2()
        {
            try
            {
                dgData.Columns.Clear();

                string textColumnStrig = @"
                    [
                        {
                            ""HeaderText"": ""學校名稱"",
                            ""Name"": ""學校名稱"",
                            ""Width"": 150,
                            ""ReadOnly"": true
                        },
                        {
                            ""HeaderText"": ""使用帳號"",
                            ""Name"": ""使用帳號"",
                            ""Width"": 150,
                            ""ReadOnly"": true
                        },
                        {
                            ""HeaderText"": ""最後使用日期"",
                            ""Name"": ""最後使用日期"",
                            ""Width"": 150,
                            ""ReadOnly"": true
                        },
                        {
                            ""HeaderText"": ""登入IP"",
                            ""Name"": ""登入IP"",
                            ""Width"": 180,
                            ""ReadOnly"": true
                        },
                        {
                            ""HeaderText"": ""最高權限"",
                            ""Name"": ""最高權限"",
                            ""Width"": 100,
                            ""ReadOnly"": true
                        }
                    ]            
                
";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<DataGridViewTextBoxColumnInfo> jsonObjArray = serializer.Deserialize<List<DataGridViewTextBoxColumnInfo>>(textColumnStrig);
                foreach (DataGridViewTextBoxColumnInfo jObj in jsonObjArray)
                {
                    DataGridViewTextBoxColumn dgt = new DataGridViewTextBoxColumn();
                    dgt.Name = jObj.Name;
                    dgt.Width = jObj.Width;
                    dgt.HeaderText = jObj.HeaderText;
                    dgt.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgt.ReadOnly = jObj.ReadOnly;
                    dgData.Columns.Add(dgt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void comboSelType_SelectedIndexChanged(object sender, EventArgs e)
        {

            // 清空 DataGridView            
            dgData.Rows.Clear();
            btnExport.Enabled = btnQuery.Enabled = dgData.Enabled = false;

            if (comboSelType.Text == "系統管理師")
            {
                _SelType = "系統管理師";
                LoadDgDataColumns1();
            }

            if (comboSelType.Text == "行政帳號")
            {
                _SelType = "行政帳號";
                LoadDgDataColumns2();

            }

            btnExport.Enabled = btnQuery.Enabled = dgData.Enabled = true;
        }

        // 取得使用者所選學校ID
        private void GetSelectSchoolIDs()
        {
            if (FISCA.Authentication.DSAServices.PassportToken == null)
            {
                FISCA.Presentation.Controls.MsgBox.Show("請使用 Greening 帳號!");
                this.Close();
            }
            else
            {
                if (Program.MainPanel.SelectedSource.Count > 0)
                {
                    _SelectSchoolIDList = Program.MainPanel.SelectedSource;
                }
            }
        }
    }
}
