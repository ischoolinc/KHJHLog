using FISCA.Presentation.Controls;
using FISCA.UDT;
using FISCA.DSAClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace KHJHLog
{
    public partial class frmClubStatus : BaseForm
    {
        BackgroundWorker _bgWorker;

        string defSchoolYear = "";
        string defSemester = "";

        Campus.Configuration.ConfigData cd = Campus.Configuration.Config.User["KHJHLog.frmClubStatus"];

        public frmClubStatus()
        {
            InitializeComponent();

            _bgWorker = new BackgroundWorker();
            _bgWorker.DoWork += _bgWorker_DoWork;
            _bgWorker.RunWorkerCompleted += _bgWorker_RunWorkerCompleted;
            _bgWorker.ProgressChanged += _bgWorker_ProgressChanged;
            _bgWorker.WorkerReportsProgress = true;
        }

        private void _bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState.ToString() != "完成")
            {
                FISCA.Presentation.MotherForm.SetStatusBarMessage(e.UserState.ToString(), e.ProgressPercentage);
            }
            else
            {
                FISCA.Presentation.MotherForm.SetStatusBarMessage("");
            }
        }

        private void frmClubStatus_Load(object sender, EventArgs e)
        {

            int DefSchoolYear = 107;
            if (int.TryParse(cd["DefSchoolYear"], out DefSchoolYear))
            {
                intDefSchoolYear.Value = DefSchoolYear;
            }

            int DefSemester = 1;
            if (int.TryParse(cd["DefSemester"], out DefSemester))
            {
                cbDefSemester.SelectedIndex = DefSemester - 1;
            }
            Authentication();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!_bgWorker.IsBusy)
            {
                //認證是否使用ischool帳號
                Authentication();
            }
            else
            {
                MsgBox.Show("系統忙碌中");
            }
        }

        /// <summary>
        /// 認證邏輯
        /// </summary>
        private void Authentication()
        {
            if (FISCA.Authentication.DSAServices.PassportToken == null)
            {
                FISCA.Presentation.Controls.MsgBox.Show("請使用 Greening 帳號!");
            }
            else
            {
                if (Program.MainPanel.SelectedSource.Count > 0)
                {
                    defSchoolYear = intDefSchoolYear.Value.ToString();
                    defSemester = cbDefSemester.SelectedItem.ToString();

                    List<string> schoolIDList = Program.MainPanel.SelectedSource;
                    _bgWorker.RunWorkerAsync(schoolIDList);
                }
            }
        }

        private void _bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //儲存查詢資料
            cd["DefSchoolYear"] = defSchoolYear;
            cd["DefSemester"] = defSemester;
            cd.Save();

            //選擇的學校
            List<string> schoolIDList = (List<string>)e.Argument;

            List<School> schoolList = tool._a.Select<School>(schoolIDList);

            List<SchoolClubRecord> SUMRecordList = new List<SchoolClubRecord>();
            int schoolCount = 0;
            _bgWorker.ReportProgress(schoolCount, "開始取得資料");

            foreach (School school in schoolList)
            {
                SchoolClubRecord schoolClub = NowConnSchool(school);
                if (schoolClub != null)
                {
                    schoolClub.SchoolYear = defSchoolYear;
                    schoolClub.Semester = defSemester;
                    SUMRecordList.Add(schoolClub);
                }

                if (schoolCount <= 100)
                {
                    schoolCount++;
                    _bgWorker.ReportProgress(schoolCount, "取得資料中");
                }
            }

            _bgWorker.ReportProgress(schoolCount, "完成");
            e.Result = SUMRecordList;
        }

        private SchoolClubRecord NowConnSchool(School school)
        {
            try
            {
                SchoolClubRecord scr = new SchoolClubRecord();
                scr.school = school;

                //取得局端登入後Greening發的Passport，並登入指定的Contract
                Connection con = new Connection();
                con.Connect(FISCA.Authentication.DSAServices.DefaultDataSource.AccessPoint, "ischool.kh.central_office.user", FISCA.Authentication.DSAServices.PassportToken);

                //取得該Contract所發的Passport
                Envelope Response1 = con.SendRequest("DS.Base.GetPassportToken", new Envelope());
                PassportToken Passport = new PassportToken(Response1.Body);

                //TODO：拿此Passport登入各校
                Connection conSchool = new Connection();
                conSchool.Connect(school.DSNS, "ischool.kh.central_office", Passport);

                //取得學校各年級上課時間表
                //此SQL 共用2個Service GetClubStatus,GetClubCount

                XmlHelper req = new XmlHelper("<Request/>");
                req.AddElement("Condition");
                req.AddElement("Condition", "SchoolYear", defSchoolYear);
                req.AddElement("Condition", "Semester", defSemester);

                Envelope Response2 = conSchool.SendRequest("_.GetClubStatus", new Envelope(req));
                XElement elmResponse1 = XElement.Load(new StringReader(Response2.Body.XmlString));

                //整理日期資料
                foreach (XElement elmClub in elmResponse1.Elements("OccurClub"))
                {
                    OccurClub club = new OccurClub(elmClub);
                    scr.clubList.Add(club);
                }

                //取得學校各社團清單與數量
                Envelope Response3 = conSchool.SendRequest("_.GetClubCount", new Envelope(req));
                XElement elmResponse3 = XElement.Load(new StringReader(Response3.Body.XmlString));

                //整理社團資料
                foreach (XElement elmCourse in elmResponse3.Elements("Course"))
                {
                    scr.社團數 += 1;
                    scr.clubNameList.Add(elmCourse.ElementText("CourseName"));
                }

                return scr;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Can't find service:GetClubStatus")
                {
                    MsgBox.Show("發生錯誤:\n" + school.Title + " - " + " 校端未安裝UDM(GetClubStatus)");
                }
                else
                {
                    MsgBox.Show("發生錯誤:\n" + school.Title + " - " + ex.Message);
          
                }
                //發生錯誤
            }
            return null;
        }

        private void _bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    List<SchoolClubRecord> SUMRecordList = (List<SchoolClubRecord>)e.Result;

                    dataGridViewX1.Rows.Clear();
                    foreach (SchoolClubRecord club in SUMRecordList)
                    {
                        club.Sum();

                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dataGridViewX1);

                        row.Cells[Column1.Index].Value = club.school.Title;
                        row.Cells[Column2.Index].Value = club.社團數;

                        row.Cells[Column3.Index].Value = club.一年級節數;
                        row.Cells[Column4.Index].Value = club.一年級隔周上課 ? "是" : "否";

                        row.Cells[Column5.Index].Value = club.二年級節數;
                        row.Cells[Column6.Index].Value = club.二年級隔周上課 ? "是" : "否";

                        row.Cells[Column7.Index].Value = club.三年級節數;
                        row.Cells[Column8.Index].Value = club.三年級隔周上課 ? "是" : "否";

                        row.Tag = club;

                        dataGridViewX1.Rows.Add(row);
                    }

                    if (dataGridViewX1.Rows.Count == 0)
                    {
                        MsgBox.Show("查無符合條件資料");
                    }
                }
                else
                {
                    MsgBox.Show("發生錯誤:\n" + e.Error.Message);
                }
            }
            else
            {
                MsgBox.Show("已停止作業");
            }
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewX1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1)
            {
                SchoolClubRecord club = (SchoolClubRecord)dataGridViewX1.Rows[e.RowIndex].Tag;
                frmClubDetail fCD = new frmClubDetail(club);
                fCD.ShowDialog();
            }
        }

        private void btnViewClub_Click(object sender, EventArgs e)
        {
            SchoolClubRecord club = (SchoolClubRecord)dataGridViewX1.CurrentRow.Tag;
            frmClubDetail fCD = new frmClubDetail(club);
            fCD.ShowDialog();
        }
    }
}
