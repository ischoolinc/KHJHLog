using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using FISCA.DSAClient;
using System.Xml.Linq;
using System.IO;
using Aspose.Cells;

namespace KHJHLog
{
    public partial class frmSchoolClassCount : BaseForm
    {
        BackgroundWorker _bgWorker;
        Dictionary<string, string> _SchoolDict;
        List<SchoolClassStudentCount> _SchoolClassStudentCountList;
        private int _ClassStudentMax = 0;
        List<string> _SelectSchoolList;

        Campus.Configuration.ConfigData cd = Campus.Configuration.Config.User["SchoolClassCountOption"];
        public frmSchoolClassCount()
        {
            InitializeComponent();
            
            _SelectSchoolList = new List<string>();
            _SchoolDict = new Dictionary<string, string>();
            _SchoolClassStudentCountList = new List<SchoolClassStudentCount>();
            _bgWorker = new BackgroundWorker();
            _bgWorker.DoWork += _bgWorker_DoWork;
            _bgWorker.RunWorkerCompleted += _bgWorker_RunWorkerCompleted;
            _bgWorker.ProgressChanged += _bgWorker_ProgressChanged;
            _bgWorker.WorkerReportsProgress = true;
        }

        void _bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("各校人數超過上限班級統計產生中..", e.ProgressPercentage);
        }

        void _bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnExport.Enabled = btnQuery.Enabled = true;
            
            FISCA.Presentation.MotherForm.SetStatusBarMessage("各校人數超過上限班級統計產生完成");

            if(_SchoolClassStudentCountList.Count>0)
            {
                foreach(SchoolClassStudentCount scs in _SchoolClassStudentCountList)
                {
                    int rowIdx = dgData.Rows.Add();
                    dgData.Rows[rowIdx].Cells[colSchool.Index].Value = scs.SchoolName;
                    dgData.Rows[rowIdx].Cells[colClassName.Index].Value = scs.ClassName;
                    dgData.Rows[rowIdx].Cells[colClassStudentC1.Index].Value = scs.ClassStudentCount;
                }
            }
        }

        void _bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int cc = 5;
            _bgWorker.ReportProgress(1);
            // 取得學校 UDT 並加入 Dict
            _SchoolClassStudentCountList.Clear();
            Dictionary<string, string> SelectSchoolDict = new Dictionary<string, string>();
            foreach(string key in _SchoolDict.Keys)
            {
                if (_SelectSchoolList.Contains(key))
                    SelectSchoolDict.Add(key, _SchoolDict[key]);
            }
            
            _bgWorker.ReportProgress(5);

            foreach (string ScTitle in SelectSchoolDict.Keys)
            {
                string ScDSNS = SelectSchoolDict[ScTitle];
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

                    Response = conSchool.SendRequest("_.GetClassStudentCount", new Envelope());
                    
                    XElement elmResponse = XElement.Load(new StringReader(Response.Body.XmlString)); 
                    
                    foreach (XElement elmClass in elmResponse.Elements("Class"))
                    {
                        string ClassName = elmClass.ElementText("ClassName");
                        
                        //// 班級人數
                        //string StudentCount = elmClass.ElementText("StudentCount");

                        // 編班人數
                        string ClassStudentCount = elmClass.ElementText("ClassStudentCount");

                        int cot;
                        if(int.TryParse(ClassStudentCount,out cot))
                        {
                            // 超過人數上限
                            if(cot>_ClassStudentMax)
                            {
                                SchoolClassStudentCount scs = new SchoolClassStudentCount();
                                scs.SchoolName = ScTitle;
                                scs.ClassName = ClassName;
                                scs.ClassStudentCount = cot;
                                _SchoolClassStudentCountList.Add(scs);
                            }
                        }
                    }
                   
                    cc++;
                    if (cc > 99)
                        cc = 99;
                    
                    _bgWorker.ReportProgress(cc);
                }
                catch (Exception ex)
                {

                }
            }

            _bgWorker.ReportProgress(100);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSchoolClassCount_Load(object sender, EventArgs e)
        {
            // 上限預設值
            iptClassBase.Value = 30;
            

            // 取得學校並放入選單
            AccessHelper accHepler = new AccessHelper();
            _SchoolDict.Clear();
            List<School> SchoolList = accHepler.Select<School>();
            SchoolList = (from data in SchoolList orderby data.Title ascending select data).ToList();

            foreach (School sc in SchoolList)
            {
                if (!_SchoolDict.ContainsKey(sc.Title))
                    _SchoolDict.Add(sc.Title, sc.DSNS);
            }

            // 讀取設定值
            int maxC;
            if (int.TryParse(cd["ClassStudentMax"], out maxC))
                iptClassBase.Value = maxC;

            List<string> ssc = cd["SelectSchool"].Split(',').ToList();
            if (ssc == null)
                ssc = new List<string>();

            foreach(string sc in _SchoolDict.Keys)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Name = sc;
                lvi.Text = sc;
                if (ssc.Contains(lvi.Text))
                    lvi.Checked = true;
                else
                    lvi.Checked = false;
                lvSchool.Items.Add(lvi);
            }


        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (FISCA.Authentication.DSAServices.PassportToken == null)
            {
                FISCA.Presentation.Controls.MsgBox.Show("Greening Passport 認證失敗，請檢查登入帳號!");
            } else
            {
                if(lvSchool.CheckedItems.Count>0)
                {
                    _SelectSchoolList.Clear();
                    foreach(ListViewItem lvi in lvSchool.CheckedItems)
                        _SelectSchoolList.Add(lvi.Text);

                    btnExport.Enabled = btnQuery.Enabled = false;
                    dgData.Rows.Clear();
                    // 人數上限
                    _ClassStudentMax = iptClassBase.Value;
                    // 儲存畫面值
                    cd["ClassStudentMax"] = _ClassStudentMax.ToString();
                    cd["SelectSchool"] = string.Join(",", _SelectSchoolList.ToArray());
                    cd.Save();
                    _bgWorker.RunWorkerAsync();
                }else
                {
                    FISCA.Presentation.Controls.MsgBox.Show("請勾選學校.");
                }                
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
           if(dgData.Rows.Count>0)
           {
               btnExport.Enabled = false;
               Workbook wb = new Workbook();
               wb.Open(new MemoryStream(Properties.Resources.各校人數超過上限班級統計樣版));

               int rowIdx=1;
               foreach(DataGridViewRow drv in dgData.Rows)
               {
                   if (drv.IsNewRow)
                       continue;
                   wb.Worksheets[0].Cells[rowIdx, 0].PutValue(drv.Cells[colSchool.Index].Value.ToString());
                   wb.Worksheets[0].Cells[rowIdx, 1].PutValue(drv.Cells[colClassName.Index].Value.ToString());
                   wb.Worksheets[0].Cells[rowIdx, 2].PutValue(drv.Cells[colClassStudentC1.Index].Value.ToString());
                   rowIdx++;

               }

               Utility.ExprotXls("各校人數超過上限班級統計", wb);
               btnExport.Enabled = true;
           }
        }

        private void chkSchool_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lvSchool.Items)
                lvi.Checked = chkSchool.Checked;
        }
    }
}
