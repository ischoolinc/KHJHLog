using FISCA.DSAClient;
using FISCA.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace KHJHLog
{
    public partial class frmClassDetail : BaseForm
    {

        BackgroundWorker _bgWorker;
        List<SchoolClassRecord> RecordList;

        public frmClassDetail()
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

        private void frmClassDetail_Load(object sender, EventArgs e)
        {
            Authentication();


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
                    List<string> schoolIDList = Program.MainPanel.SelectedSource;
                    _bgWorker.RunWorkerAsync(schoolIDList);
                }
            }
        }

        private void _bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //選擇的學校
            List<string> schoolIDList = (List<string>)e.Argument;

            List<School> schoolList = tool._a.Select<School>(schoolIDList);

            RecordList = new List<SchoolClassRecord>();
            int schoolCount = 0;

            foreach (School school in schoolList)
            {
                SchoolClassRecord schoolClub = NowConnSchool(school);
                if (schoolClub != null)
                {
                    RecordList.Add(schoolClub);
                }
                if (schoolCount <= 100)
                {
                    schoolCount++;
                    _bgWorker.ReportProgress(schoolCount, "取得資料中");
                }
            }

            _bgWorker.ReportProgress(schoolCount, "完成");
        }

        private SchoolClassRecord NowConnSchool(School school)
        {
            try
            {
                SchoolClassRecord scr = new SchoolClassRecord();
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

                //取得學校無班導師清單
                Envelope Response2 = conSchool.SendRequest("_.GetClassTeacherStatus", new Envelope());
                XElement elmResponse1 = XElement.Load(new StringReader(Response2.Body.XmlString));

                //整理日期資料
                foreach (XElement elmClub in elmResponse1.Elements("ClassRecord"))
                {
                    OccurClass classRecord = new OccurClass(elmClub);
                    scr.classList.Add(classRecord);
                }

                return scr;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Can't find service:GetClassTeacherStatus")
                {
                    MsgBox.Show("發生錯誤:\n" + school.Title + " - " + " 校端未安裝UDM(GetClassTeacherStatus)");
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
                    Bindata();
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

        private void Bindata()
        {
            dataGridViewX1.Rows.Clear();

            foreach (SchoolClassRecord classR in RecordList)
            {
                foreach (OccurClass each in classR.classList)
                {
                    if (checkBoxX1.Checked)
                    {
                        AddRow(classR, each);
                    }
                    else
                    {
                        if (each.TeacherID == "")
                        {
                            AddRow(classR, each);
                        }
                    }
                }
            }

            if (dataGridViewX1.Rows.Count == 0)
            {
                MsgBox.Show("查無符合條件資料");
            }
        }

        private void AddRow(SchoolClassRecord classR, OccurClass each)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dataGridViewX1);
            row.Cells[0].Value = classR.school.Title;
            row.Cells[1].Value = each.GradeYear;
            row.Cells[2].Value = each.ClassName;
            row.Cells[3].Value = each.TeacherName;
            row.Tag = each;
            dataGridViewX1.Rows.Add(row);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {
            Bindata();
        }
    }
}
