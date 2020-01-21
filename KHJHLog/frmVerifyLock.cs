using FISCA.DSAClient;
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

                if (action == "核准鎖班")
                {

                    var xml = new XmlHelper("<Request/>");
                    xml.AddElement("ApplingStatus").InnerText = ApplyStatus.局端同意鎖班.ToString(); ;
                    xml.AddElement("ClassID").InnerText = this.ClassInfo.ClassID;
                    xml.AddElement("DistrictComment").InnerText = this.txtDistrictComment.Text;

                    Response = conSchool.SendRequest("_.ApproveAndLock", new Envelope(xml));

                } else if (action== "退回鎖班申請")
                {

                    var xml = new XmlHelper("<Request/>");
                    xml.AddElement("ApplingStatus").InnerText = this.cboAction.Text;
                    xml.AddElement("ClassID").InnerText = this.ClassInfo.ClassID;
                    xml.AddElement("DistrictComment").InnerText = this.txtDistrictComment.Text;

                    Response = conSchool.SendRequest("_.ReturnApplication", new Envelope(xml));
                }

                DataTable dt = DataHelper.convertXmlToDataTable(Response.Body.XmlString);


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
