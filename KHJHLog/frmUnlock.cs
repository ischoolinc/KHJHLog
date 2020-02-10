using FISCA.DSAClient;
using FISCA.LogAgent;
using FISCA.Presentation;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using KHJHLog.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KHJHLog
{
    public partial class frmUnlock : FISCA.Presentation.Controls.BaseForm
    {

        private List<string> SelectedSchoolIDs;
        private List<School> SelectedSchools;
        private AccessHelper _Helper;
        private BackgroundWorker _BGWorker;
        private int currentSchoolIndex;
        private List<string> SuccesLock;
        private string DistictComment;
        private List<string> SchoolError = new List<string>();

        private List<string> selectedSchools;

        public frmUnlock()
        {
            InitializeComponent();
        }


        private void frmUnlock_Load(object sender, EventArgs e)
        {
            //初始化backGroundWorker
            _BGWorker = new BackgroundWorker();
            _BGWorker.DoWork += _BGWorker_DoWork;
            _BGWorker.RunWorkerCompleted += _BGWorker_RunWorkerCompleted;
            _BGWorker.ProgressChanged += _BGWorker_ProgressChanged;
            _BGWorker.WorkerReportsProgress = true;

            this._Helper = new AccessHelper();
            this.SuccesLock = new List<string>();
            Authentication();//驗證是否為Grenning 帳號
            loadSchools();

            //預設學校解鎖 說明 for 學校方便使用
            // txtDistrictComment.Text = "局端解鎖(未設定不自動解鎖班級)";

        }

        private void _BGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage(e.UserState.ToString());
        }

        private void _BGWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    if (this.SchoolError.Count < this.SelectedSchools.Count)  //如果不是所有學校都發生錯誤
                    {
                        String SuccessLockSchools = String.Join("\n", this.SuccesLock);
                        MsgBox.Show($"已成功完成解鎖{SuccessLockSchools}");
                    }
                    else
                    {
                        String ErrrorLockSchools = String.Join("\n", this.SchoolError);
                        MsgBox.Show($"錯誤清單:{ErrrorLockSchools}");
                    }
                }
            }
        }

        //DoWork 執行
        private void _BGWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.currentSchoolIndex = 0;

            foreach (School school in this.SelectedSchools)
            {
                this.currentSchoolIndex++;

                _BGWorker.ReportProgress(this.getProgressPercent(), $"({this.currentSchoolIndex} / {this.SelectedSchools.Count}) 處理 {school.Title} 中");
                try
                {

                    UpdateSchoolDate();
                    SuccesLock.Add(school.Title);
                }
                catch (Exception ex)
                {

                    MsgBox.Show($"解鎖{school.Title}時，發生錯誤 \n，錯誤訊息:{ex.Message} \n  堆疊:{ex.StackTrace}");
                    SchoolError.Add(school.Title);

                }

                _BGWorker.ReportProgress(this.getProgressPercent(), $"完成 {school.Title}");
            }
            ApplicationLog.Log("局端手動解鎖", "執行", $"解鎖學校：\n {  string.Join("\n", this.SuccesLock)}");

            _BGWorker.ReportProgress(this.getProgressPercent(), $"全部完成");

        }

        private void UpdateSchoolDate()
        {
            foreach (School schoolInfo in SelectedSchools)
            {

                Connection con = new Connection();

                //取得局端登入後Greening發的Passport，並登入指定的Contract
                con.Connect(FISCA.Authentication.DSAServices.DefaultDataSource.AccessPoint, "ischool.kh.central_office.user", FISCA.Authentication.DSAServices.PassportToken);

                //取得該Contract所發的Passport
                Envelope Response = con.SendRequest("DS.Base.GetPassportToken", new Envelope());
                PassportToken Passport = new PassportToken(Response.Body);

                //TODO：拿此Passport登入各校

                Connection conSchool = new Connection();
                conSchool.Connect(schoolInfo.DSNS, "ischool.kh.central_office", Passport);

                //依據功能不同呼叫不同service，
                //var xml = new XmlHelper("<Request/>");
                //for(var i = 0;i<10;i++)
                //{
                //    var stud = new XmlHelper(xml.AddElement("Request", "Student"));

                //    stud.AddElement("Name").InnerText = i.ToString();
                //    stud.AddElement("ID", i.ToString());

                //}



                // log用
                string ClinetInfo = @"
<ClientInfo>
  <HostName></HostName>
  <NetworkAdapterList>
    <NetworkAdapter>
      <IPAddress></IPAddress>
      <PhysicalAddress></PhysicalAddress>
    </NetworkAdapter>
    <NetworkAdapter>
      <IPAddress></IPAddress>
      <PhysicalAddress></PhysicalAddress>
    </NetworkAdapter>
    <NetworkAdapter>
      <IPAddress></IPAddress>
      <PhysicalAddress></PhysicalAddress>
    </NetworkAdapter>
    <NetworkAdapter>
      <IPAddress></IPAddress>
      <PhysicalAddress/>
    </NetworkAdapter>
    <NetworkAdapter>
      <IPAddress></IPAddress>
      <PhysicalAddress></PhysicalAddress>
    </NetworkAdapter>
    <NetworkAdapter>
      <IPAddress></IPAddress>
      <PhysicalAddress></PhysicalAddress>
    </NetworkAdapter>
  </NetworkAdapterList>
</ClientInfo>
";

                var xml = new XmlHelper("<Request/>");
                xml.AddElement("districtComment").InnerText = this.DistictComment;
                xml.AddElement("ClientInfo").InnerText = ClinetInfo;


               
                    Response = conSchool.SendRequest("_.UpdateClassNulock", new Envelope(xml)); //update unlock
               
             
            }
        }

        private int getProgressPercent()
        {

            return this.currentSchoolIndex * 100 / this.SelectedSchools.Count;

        }


        //驗證是否為 Greening 帳號 
        private void Authentication()
        {
            if (FISCA.Authentication.DSAServices.PassportToken == null)
            {
                FISCA.Presentation.Controls.MsgBox.Show("請使用 Greening 帳號!");

                this.Close();
            }

        }

        //載入學校資料
        private void loadSchools()
        {
            NLDPanel panel = (NLDPanel)MotherForm.Panels.ToList<INCPanel>()[0];
            this.SelectedSchoolIDs = panel.SelectedSource;

            if (this.SelectedSchoolIDs.Count < 1)
            {
                FISCA.Presentation.Controls.MsgBox.Show("請先選擇要匯出資料的學校！");
                this.Close();
            }

            this.SelectedSchools = _Helper.Select<School>(this.SelectedSchoolIDs);

            this.lstSchools.Items.Clear();
            foreach (School sch in SelectedSchools)
            {
                this.lstSchools.Items.Add($"{sch.Title}  {sch.DSNS}");
            }
            this.labelX1.Text = $"將針對以下 {SelectedSchools.Count} 所學校執行解鎖 :";
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MsgBox.Show("此動作會將所選各校之\"未\"設定【不自動解鎖】之班級解鎖。", MessageBoxButtons.YesNo);

            //蒐集畫面資料
            //  this.DistictComment = this.txtDistrictComment.Text;


            if (dialogResult == DialogResult.Yes)
            {
                _BGWorker.RunWorkerAsync();
            }
        }
    }
}
