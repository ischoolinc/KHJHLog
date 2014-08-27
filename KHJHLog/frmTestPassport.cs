using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.DSAClient;

namespace KHJHLog
{
    public partial class frmTestPassport : Form
    {
        public frmTestPassport()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

            //FISCA.Authentication.DSAServices.p

            Connection con = new Connection();
 
            //取得局端登入後Greening發的Passport，並登入指定的Contract
            con.Connect(FISCA.Authentication.DSAServices.DefaultDataSource.AccessPoint, "ischool.kh.central_office.user", FISCA.Authentication.DSAServices.PassportToken);

            //取得該Contract所發的Passport
            Envelope Response = con.SendRequest("DS.Base.GetPassportToken", new Envelope());
            PassportToken Passport = new PassportToken(Response.Body);

            //TODO：拿此Passport登入各校
            Connection conSchool = new Connection();
            conSchool.Connect("dev.jh_kh", "ischool.kh.central_office", Passport);
        }
    }
}