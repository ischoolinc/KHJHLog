using FISCA.DSAClient;
using FISCA.LogAgent;
using FISCA.Presentation.Controls;
using KHJHLog.Helper;
using KHJHLog.Model;
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
    public partial class frmVerifyLock : BaseForm
    {
        private ClassOrder ClassInfo;
        public frmVerifyLock(ClassOrder classInfo)
        {
            InitializeComponent();
            this.ClassInfo = classInfo;
        }

        private void frmVerifyLock_Load(object sender, EventArgs e)
        {

            this.txtSchool.Text = ClassInfo.School.Title;
            this.txtClass.Text = this.ClassInfo.ClassName;
            cboAction.Items.AddRange(new string[] { "核准鎖班", "退回鎖班申請" });
            cboAction.SelectedIndex = 0;

        }

        private void textBoxX2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnConduct_Click(object sender, EventArgs e)
        {
            //is Appling =false
            //lock_apply_status =""

            string Action = this.cboAction.Text;
            this.Update(Action);
            this.Close();

        }


        public void Update(string action)
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
                conSchool.Connect(ClassInfo.School.DSNS, "ischool.kh.central_office", Passport);

                //依據功能不同呼叫不同service，
                //var xml = new XmlHelper("<Request/>");
                //for(var i = 0;i<10;i++)
                //{
                //    var stud = new XmlHelper(xml.AddElement("Request", "Student"));

                //    stud.AddElement("Name").InnerText = i.ToString();
                //    stud.AddElement("ID", i.ToString());

                //}

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


                if (action == "核准鎖班")
                {

                    var xml = new XmlHelper("<Request/>");
                    xml.AddElement("ApplingStatus").InnerText = "";
                    xml.AddElement("ClassID").InnerText = this.ClassInfo.ClassID;
                    xml.AddElement("DistrictComment").InnerText = this.txtDistrictComment.Text;
                    xml.AddElement("IsLock").InnerText = "true";
                    xml.AddElement("Message").InnerText = "局端核准鎖班";
                    xml.AddElement("ClientInfo").InnerText = ClinetInfo;
                    Response = conSchool.SendRequest("_.ApproveAndLock", new Envelope(xml));

                }
                else if (action == "退回鎖班申請")
                {

                    var xml = new XmlHelper("<Request/>");

                    xml.AddElement("ApplingStatus").InnerText = ApplyStatus.鎖班申請退回_鎖班數超過二分之一.ToString();
                    xml.AddElement("ClassID").InnerText = this.ClassInfo.ClassID;
                    xml.AddElement("DistrictComment").InnerText = this.txtDistrictComment.Text;
                    xml.AddElement("IsLock").InnerText = "false";
                    xml.AddElement("Message").InnerText = "退回鎖班申請";
                    xml.AddElement("ClientInfo").InnerText = ClinetInfo;

                    Response = conSchool.SendRequest("_.ApproveAndLock", new Envelope(xml));
                }

                DataTable dt = DataHelper.convertXmlToDataTable(Response.Body.XmlString);

                StringBuilder  sb  = new StringBuilder();
                sb.AppendLine($"日期時間「{DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")}」學校「{this.ClassInfo.School.Title}」，班級「{this.ClassInfo.ClassName}」 動作「{action}」備註「{ this.txtDistrictComment.Text}」");

                ApplicationLog.Log("校端鎖班申請審核", action, "", sb.ToString());

                MsgBox.Show("儲存成功!");

            }
            catch (Exception ex)
            {
                MsgBox.Show($"錯誤訊息:{ex.Message} \r\n  堆疊:{ex.StackTrace}");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MsgBox.Show("確定取消?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
