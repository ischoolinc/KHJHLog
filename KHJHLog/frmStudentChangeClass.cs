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
    public partial class frmStudentChangeClass : BaseForm
    {
        BackgroundWorker _bgWorker;
        Dictionary<string, string> _SchoolDict;        
        List<string> _SelectSchoolList;
        Campus.Configuration.ConfigData cd = Campus.Configuration.Config.User["SchoolStudentClassChangeOption"];
        List<StudentChangeClass> _StudentChangeClassList;

        string _ClassComment = "", _OldClassComment = "",_ClassName="",_OldClassName="";

        bool _chkOp1 = false, _chkOp2 = false;

        public frmStudentChangeClass()
        {
            InitializeComponent();
            _SelectSchoolList = new List<string>();
            _SchoolDict = new Dictionary<string, string>();
            _StudentChangeClassList = new List<StudentChangeClass>();
            _bgWorker = new BackgroundWorker();
            _bgWorker.DoWork += _bgWorker_DoWork;
            _bgWorker.RunWorkerCompleted += _bgWorker_RunWorkerCompleted;
            _bgWorker.ProgressChanged += _bgWorker_ProgressChanged;
            _bgWorker.WorkerReportsProgress = true;
        }

        void _bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage("各校學生調整班級查詢產生中..", e.ProgressPercentage);
        }

        void _bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnExport.Enabled = btnQuery.Enabled = true;

            if (_StudentChangeClassList.Count > 0)
            {
                foreach (StudentChangeClass scc in _StudentChangeClassList)
                {
                    int rowIdx = dgData.Rows.Add();
                    dgData.Rows[rowIdx].Cells[colSchool.Index].Value = scc.SchoolName;
                    dgData.Rows[rowIdx].Cells[colClassName.Index].Value = scc.ClassName;
                    dgData.Rows[rowIdx].Cells[colOldClassName.Index].Value = scc.OldClassName;
                    dgData.Rows[rowIdx].Cells[colOldClassContent.Index].Value = scc.OldClassComment;
                    dgData.Rows[rowIdx].Cells[colClassContent.Index].Value = scc.ClassComment;
                    dgData.Rows[rowIdx].Cells[colClassOrder1.Index].Value = scc.ClassOrderName1;
                    dgData.Rows[rowIdx].Cells[colClassOrder2.Index].Value = scc.ClassOrderName2;
                    dgData.Rows[rowIdx].Cells[colClassOrder3.Index].Value = scc.ClassOrderName3;
                    dgData.Rows[rowIdx].Cells[colStudName.Index].Value = scc.StudentName;
                    
                }
            }

            FISCA.Presentation.MotherForm.SetStatusBarMessage("各校學生調整班級查詢產生完成");
        }

        void _bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int cc = 5;
            _bgWorker.ReportProgress(1);
            // 取得學校 UDT 並加入 Dict
            
            Dictionary<string, string> SelectSchoolDict = new Dictionary<string, string>();
            foreach (string key in _SchoolDict.Keys)
            {
                if (_SelectSchoolList.Contains(key))
                    SelectSchoolDict.Add(key, _SchoolDict[key]);
            }

            _bgWorker.ReportProgress(5);
            _StudentChangeClassList.Clear();
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

                    Response = conSchool.SendRequest("_.GetClassStudSpecial", new Envelope());

                    XElement elmResponse = XElement.Load(new StringReader(Response.Body.XmlString));

                    foreach (XElement elmStudent in elmResponse.Elements("Student"))
                    {
                        StudentChangeClass scc = new StudentChangeClass();
                        scc.ClassName = elmStudent.ElementText("ClassName");
                        scc.OldClassName = elmStudent.ElementText("OldClassName");
                        scc.SchoolName = ScDSNS;
                        scc.StudentName = elmStudent.ElementText("StudentName");
                        scc.OldClassComment = elmStudent.ElementText("OldClassComment");
                        scc.ClassComment = elmStudent.ElementText("ClassComment");

                        if (elmStudent.Element("Content")!=null)
                        {
                            if(elmStudent.Element("Content").Element("Content") !=null)
                            {
                                if (elmStudent.Element("Content").Element("Content").Element("FirstClassName") != null)
                                    scc.ClassOrderName1 = elmStudent.Element("Content").Element("Content").Element("FirstClassName").Value;

                                if (elmStudent.Element("Content").Element("Content").Element("SecondClassName") != null)
                                    scc.ClassOrderName2 = elmStudent.Element("Content").Element("Content").Element("SecondClassName").Value;

                                if (elmStudent.Element("Content").Element("Content").Element("ThridClassName") != null)
                                    scc.ClassOrderName3 = elmStudent.Element("Content").Element("Content").Element("ThridClassName").Value;
                            }
                        }                        
                        
                        if(_chkOp1 == false && _chkOp2 == false)
                            _StudentChangeClassList.Add(scc);
                        else
                        {
                            if(_chkOp1 == true  && _chkOp2 == true)
                            {
                                if (scc.ClassComment == _ClassComment && scc.OldClassComment == _OldClassComment && scc.ClassName == _ClassName && scc.OldClassName == _OldClassName)
                                {
                                    _StudentChangeClassList.Add(scc);
                                }

                            }else
                            {
                                if (_chkOp1 == true && scc.ClassComment == _ClassComment && scc.OldClassComment == _OldClassComment)
                                {
                                    _StudentChangeClassList.Add(scc);
                                }
                                if (_chkOp2 == true && scc.ClassName == _ClassName && scc.OldClassName == _OldClassName)
                                {
                                    _StudentChangeClassList.Add(scc);
                                }
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgData.Rows.Count > 0)
            {
                btnExport.Enabled = false;
                Workbook wb = new Workbook();
                Worksheet wst = wb.Worksheets[0];

                int colIdx = 0;
                // 標題
                foreach(DataGridViewColumn col in dgData.Columns )
                {
                    wst.Cells[0, colIdx].PutValue(col.HeaderText);
                    colIdx++;
                }

                int rowIdx = 1;
                foreach (DataGridViewRow drv in dgData.Rows)
                {
                    if (drv.IsNewRow)
                        continue;

                    colIdx = 0;
                    foreach(DataGridViewCell cell in drv.Cells)
                    {
                        if(cell.Value != null)
                            wst.Cells[rowIdx, colIdx].PutValue(cell.Value.ToString());
                        colIdx++;
                    }                    

                    rowIdx++;
                }

                Utility.ExprotXls("各校學生調整班級", wb);
                btnExport.Enabled = true;
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (FISCA.Authentication.DSAServices.PassportToken == null)
            {
                FISCA.Presentation.Controls.MsgBox.Show("Greening Passport 認證失敗，請檢查登入帳號!");
            }
            else
            {
                _ClassComment = txtClassComment.Text;
                _OldClassComment = txtOldClassComment.Text;
                _ClassName = txtClassName.Text;
                _OldClassName = txtOldClassName.Text;
                _chkOp1 = chkOp1.Checked;
                _chkOp2 = chkOp2.Checked;

                cd["ClassComment"] = _ClassComment;
                cd["ClassComment"] = _ClassComment;
                cd["OldClassComment"] = _OldClassComment;
                cd["ClassName"] = _ClassName;
                cd["OldClassName"] = _OldClassName;
                cd["chkOp1"] = chkOp1.Checked.ToString();
                cd["chkOp2"] = chkOp2.Checked.ToString();
               

                if (lvSchool.CheckedItems.Count > 0)
                {
                    _SelectSchoolList.Clear();
                    foreach (ListViewItem lvi in lvSchool.CheckedItems)
                        _SelectSchoolList.Add(lvi.Text);

                    btnExport.Enabled = btnQuery.Enabled = false;
                    dgData.Rows.Clear();
                    // 儲存畫面值
                    cd["SelectSchool"] = string.Join(",", _SelectSchoolList.ToArray());
                    cd.Save();
                    _bgWorker.RunWorkerAsync();
                }
                else
                {
                    FISCA.Presentation.Controls.MsgBox.Show("請勾選學校.");
                }
            }
        }

        private void frmStudentChangeClass_Load(object sender, EventArgs e)
        {
            // 取得學校並放入選單
            AccessHelper accHepler = new AccessHelper();
            _SchoolDict.Clear();
            List<School> SchoolList = accHepler.Select<School>();
            SchoolList = (from data in SchoolList orderby data.Title ascending select data).ToList();

            foreach (School sc in SchoolList)
            {
                if (sc.Title.Contains("範本"))
                    continue;

                if (sc.DSNS.Contains("dev."))
                    continue;

                if (!_SchoolDict.ContainsKey(sc.Title))
                    _SchoolDict.Add(sc.Title, sc.DSNS);
            }

            // 讀取設定值           
            txtClassComment.Text = cd["ClassComment"];
            txtOldClassComment.Text = cd["OldClassComment"];
            txtClassName.Text = cd["ClassName"];
            txtOldClassName.Text = cd["OldClassName"];
            bool c1, c2;
            if(bool.TryParse(cd["chkOp1"],out c1))
                chkOp1.Checked = c1;

            if (bool.TryParse(cd["chkOp2"], out c2))
                chkOp2.Checked = c2;

            List<string> ssc = cd["SelectSchool"].Split(',').ToList();
            if (ssc == null)
                ssc = new List<string>();

            foreach (string sc in _SchoolDict.Keys)
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

        private void chkSchool_CheckedChanged(object sender, EventArgs e)
        {
             foreach (ListViewItem lvi in lvSchool.Items)
                lvi.Checked = chkSchool.Checked;
        }
    }
}
