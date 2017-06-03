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
using FISCA.DSAClient;
using FISCA.UDT;
using KHJHLog;


namespace KHJHLog
{
    public partial class frmSpecial : FISCA.Presentation.Controls.BaseForm
    {
        public frmSpecial()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string SelectedDSNS = "" + cmbSchool.SelectedValue;
            string School = (cmbSchool.SelectedItem as School).Title;

            if (FISCA.Authentication.DSAServices.PassportToken == null)
            {
                FISCA.Presentation.Controls.MsgBox.Show("Greening Passport 認證失敗，請檢查登入帳號!");
            }
            else
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
                    conSchool.Connect(SelectedDSNS, "ischool.kh.central_office", Passport);

                    Response = conSchool.SendRequest("_.GetStudentHighConcern", new Envelope());

                    //<Class>
                    //  <ClassName>101</ClassName>
                    //  <StudentCount>25</StudentCount>
                    //  <Lock />
                    //  <Comment />
                    //  <NumberReduceSum />
                    //  <ClassStudentCount>25</ClassStudentCount>
                    //</Class>

                    //班級名稱、實際人數、編班人數、編班順位、編班鎖定、鎖定備註

                    XElement elmResponse = XElement.Load(new StringReader(Response.Body.XmlString));

                    grdClassOrder.Rows.Clear();

                    foreach (XElement elmStudent in elmResponse.Elements("Student"))
                    {
                        string ClassName = elmStudent.ElementText("ClassName");
                        string StudentName = elmStudent.ElementText("StudentName");
                        string SeatNo = elmStudent.ElementText("SeatNo");
                        string HighConcern = elmStudent.ElementText("HighConcern");
                        string NumberReduce = elmStudent.ElementText("NumberReduce");
                        string DocNo = elmStudent.ElementText("DocNo");

                        grdClassOrder.Rows.Add(
                            School,
                            StudentName,
                            ClassName,
                            SeatNo,
                            NumberReduce,
                            DocNo
                        );
                    }
                }
                catch (Exception ve)
                {
                    MessageBox.Show(ve.Message);
                }
            }
        }

        private void frmSpecial_Load(object sender, EventArgs e)
        {
            AccessHelper helper = new AccessHelper();

            List<School> Schools = helper.Select<School>();

            // 依學校名稱排序
            Schools = (from data in Schools orderby data.Title ascending select data).ToList();
            //School vSchool = new School();

            //vSchool.DSNS = "dev.jh_kh";
            //vSchool.Title = "高雄測試國中(內部)";


            //Schools.Add(vSchool);

            cmbSchool.DataSource = Schools;
            cmbSchool.DisplayMember = "Title";
            cmbSchool.ValueMember = "DSNS";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataGridViewExport.Export(grdClassOrder, "特殊生與高關懷學生");
        }
    }
}
