using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using FISCA.Data;
using FISCA.DSAClient;
using FISCA.UDT;
using KHJHLog.UDT;

namespace KHJHLog
{
    public partial class frmClassOrder : FISCA.Presentation.Controls.BaseForm
    {
        public frmClassOrder()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string SelectedDSNS = "" + cmbSchool.SelectedValue;
            string School = (cmbSchool.SelectedItem as School).Title;

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

                Response = conSchool.SendRequest("_.GetClassStudentCount", new Envelope());

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

                List<ClassOrder> ClassOrders = new List<ClassOrder>();

                foreach (XElement elmClass in elmResponse.Elements("Class"))
                {
                    ClassOrder vClassOrder = new ClassOrder();

                    ClassOrders.Add(vClassOrder);

                    string ClassName = elmClass.ElementText("ClassName");
                    string StudentCount = elmClass.ElementText("StudentCount");
                    string ClassStudentCount = elmClass.ElementText("ClassStudentCount");
                    string NumberReduceSum = elmClass.ElementText("NumberReduceSum");

                    if (!string.IsNullOrWhiteSpace(NumberReduceSum))
                        ClassStudentCount = ClassStudentCount + "(" + StudentCount + "+" + NumberReduceSum + ")";

                    

                    string NumberReduceCount = elmClass.ElementText("NumberReduceCount");
                    string Lock = elmClass.ElementText("Lock");
                    string Comment = elmClass.ElementText("Comment");
                    string ClassOrder = string.Empty;

                    vClassOrder.ClassName = ClassName;
                    vClassOrder.StudentCount = StudentCount;
                    vClassOrder.ClassStudentCount = ClassStudentCount;
                    vClassOrder.NumberReduceSum = NumberReduceSum;
                    vClassOrder.NumberReduceCount = NumberReduceCount;
                    vClassOrder.Lock = Lock;
                    vClassOrder.Comment = Comment;
                    vClassOrder.ClassOrderNumber = ClassOrder;
                    vClassOrder.ClassStudentCountValue = StudentCount.GetInt() + NumberReduceSum.GetInt();
                }

                ClassOrders.CalculateClassOrder();

                foreach(ClassOrder vClassOrder in ClassOrders)
                {
                    grdClassOrder.Rows.Add(
                        School,
                        vClassOrder.ClassName,
                        vClassOrder.StudentCount,
                        vClassOrder.ClassStudentCount,
                        vClassOrder.NumberReduceCount,
                        vClassOrder.ClassOrderNumber,
                        vClassOrder.Lock,
                        vClassOrder.Comment);
                }
            }
            catch (Exception ve)
            {
                MessageBox.Show(ve.Message);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataGridViewExport.Export(grdClassOrder, "查詢編班");
        }

        private void frmClassOrder_Load(object sender, EventArgs e)
        {
            AccessHelper helper = new AccessHelper();

            List<School> Schools = helper.Select<School>();

            School vSchool = new School();

            vSchool.DSNS = "dev.jh_kh";
            vSchool.Title = "高雄測試國中(內部)";


            Schools.Add(vSchool);

            cmbSchool.DataSource = Schools;
            cmbSchool.DisplayMember = "Title"; 
            cmbSchool.ValueMember = "DSNS";
        }
    }
}