﻿using System;
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
using KHJHLog;
using KHJHLog.Model;

namespace KHJHLog
{
    public partial class frmClassOrder : FISCA.Presentation.Controls.BaseForm
    {
        School School;
        string SelectedDSNS;
        public frmClassOrder()
        {
            InitializeComponent();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            SelectedDSNS = "" + cmbSchool.SelectedValue;
            this.School = (cmbSchool.SelectedItem as School);

            if (FISCA.Authentication.DSAServices.PassportToken == null)
            {
                FISCA.Presentation.Controls.MsgBox.Show("Greening Passport 認證失敗，請檢查登入帳號!");
            }
            else
            {
                LoadClassInfoBySchool();
            }
        }


        private void LoadClassInfoBySchool()
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
                conSchool.Connect(this.SelectedDSNS, "ischool.kh.central_office", Passport);

                Response = conSchool.SendRequest("_.GetClassStudentCount", new Envelope());

                //<Class>
                //  <ClassName>101</ClassName>
                //  <StudentCount>25</StudentCount>
                //  <Lock />
                //  <Comment />
                //  <NumberReduceSum />
                //  <ClassStudentCount>25</ClassStudentCount>
                //</Class>

                //班級名稱、實際人數、編班人數、編班順位、編班鎖定、鎖定備註、休學生人數、輟學生人數

                XElement elmResponse = XElement.Load(new StringReader(Response.Body.XmlString));

                grdClassOrder.Rows.Clear();

                List<ClassOrder> ClassOrders = new List<ClassOrder>();

                foreach (XElement elmClass in elmResponse.Elements("Class"))
                {
                    ClassOrder vClassOrder = new ClassOrder();

                    ClassOrders.Add(vClassOrder);

                    string ClassID = elmClass.ElementText("ClassID");
                    string ClassName = elmClass.ElementText("ClassName");
                    string TeacherName = elmClass.ElementText("TeacherName");
                    string StudentCount = elmClass.ElementText("StudentCount");
                    string ClassStudentCount = elmClass.ElementText("ClassStudentCount");
                    string NumberReduceSum = elmClass.ElementText("NumberReduceSum");

                    if (!string.IsNullOrWhiteSpace(NumberReduceSum))
                        ClassStudentCount = ClassStudentCount + "(" + StudentCount + "+" + NumberReduceSum + ")";

                    string NumberReduceCount = elmClass.ElementText("NumberReduceCount");
                    string Lock = elmClass.ElementText("Lock");
                    string UnautoUnlock = elmClass.ElementText("UnautoUnlock");
                    string LockAppling = elmClass.ElementText("LockAppling");//Jean


                    string Comment = elmClass.ElementText("Comment");
                    string DistrictComment = elmClass.ElementText("DistrictComment");//Jean 增加局端解鎖住記
                    string DistrictUulockDate = elmClass.ElementText("DistrictUulockDate");//Jean 局端Service
                    string LastUpdateByDistrict = elmClass.ElementText("LastUpdateByDistrict");
                    string ClassOrder = string.Empty;

                    string DisplayOrder = elmClass.ElementText("DisplayOrder");
                    string GradeYear = elmClass.ElementText("GradeYear");

                    string SuspensionStudentCount = elmClass.ElementText("SuspensionStudentCount");
                    string DropOutStudentCount = elmClass.ElementText("DropOutStudentCount");
                    //string LockApplyStatus = elmClass.ElementText("LockApplyStatus");

                    try
                    {
                        string LockApplyStatus = elmClass.ElementText("LockApplyStatus");
                        //string  LockApplyStatus = String.IsNullOrWhiteSpace(lockApplyStatusString) ? "" : ;


                        vClassOrder.School = this.School;
                        vClassOrder.ClassID = ClassID;
                        vClassOrder.ClassName = ClassName;
                        vClassOrder.TeacherName = TeacherName;
                        vClassOrder.StudentCount = StudentCount;
                        vClassOrder.SuspensionStudentCount = "" + SuspensionStudentCount;
                        vClassOrder.DropOutStudentCount = "" + DropOutStudentCount;
                        vClassOrder.ClassStudentCount = ClassStudentCount;
                        vClassOrder.NumberReduceSum = NumberReduceSum;
                        vClassOrder.NumberReduceCount = NumberReduceCount;
                        vClassOrder.UnautoUnlock = UnautoUnlock == "t" ? "(不自動解鎖)" : "";
                    //    vClassOrder.LockAppling = LockAppling == "t" ? "(校端鎖班申請中)" : "";

                        vClassOrder.LockApplingStatus = LockApplyStatus;


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"錯誤訊息{ex.Message} ，{ex.StackTrace}");
                    }
                    // 檢查是否有特殊生人數
                    // 檢查是否有特殊生人數
                    vClassOrder.hasNumberReduce = 0;
                    int nrc;
                    if (int.TryParse(NumberReduceCount, out nrc))
                        if (nrc > 0)
                            vClassOrder.hasNumberReduce = 1;

                    vClassOrder.Lock = Lock;
                    vClassOrder.Comment = Comment;
                    vClassOrder.DistrictComment = DistrictComment;
                    vClassOrder.DistrictUulockDate = DistrictUulockDate;
                    vClassOrder.LastUpdateByDistrict = LastUpdateByDistrict;
                    vClassOrder.ClassOrderNumber = 0;

                    //if(Lock=="鎖定")
                    //    vClassOrder.ClassOrderNumber = 999;

                    vClassOrder.ClassStudentCountValue = StudentCount.GetInt() + NumberReduceSum.GetInt();
                    if (string.IsNullOrEmpty(DisplayOrder))
                        vClassOrder.DisplayOrder = 999;
                    else
                        vClassOrder.DisplayOrder = int.Parse(DisplayOrder);

                    if (string.IsNullOrEmpty(GradeYear))
                        vClassOrder.GradeYear = 999;
                    else
                        vClassOrder.GradeYear = int.Parse(GradeYear);
                }

                ClassOrders.CalculateClassOrder();

                // 判斷排序方式
                int NoCount = 0;

                foreach (ClassOrder co in ClassOrders)
                {
                    int xx;
                    if (int.TryParse(co.ClassName, out xx))
                        NoCount++;
                }

                // 有3筆以上用班級名稱排，不然用班級顯示順序
                if (NoCount > 3)
                {
                    ClassOrders = (from data in ClassOrders orderby data.GradeYear ascending, data.ClassOrderNumber, data.ClassStudentCountValue, data.ClassName ascending select data).ToList();
                }
                else
                {
                    ClassOrders = (from data in ClassOrders orderby data.GradeYear ascending, data.ClassOrderNumber, data.ClassStudentCountValue, data.DisplayOrder ascending select data).ToList();
                }

                foreach (ClassOrder vClassOrder in ClassOrders)
                {

                    string vDisplayOrder;
                    string vGradeYear;
                    //string vClassOrderNumber;

                    if (vClassOrder.DisplayOrder == 999)
                        vDisplayOrder = "";
                    else
                        vDisplayOrder = vClassOrder.DisplayOrder.ToString();

                    if (vClassOrder.GradeYear == 999)
                        vGradeYear = "";
                    else
                        vGradeYear = vClassOrder.GradeYear.ToString();

                    //if (vClassOrder.ClassOrderNumber == 999)
                    //    vClassOrderNumber = "";
                    //else
                    //    vClassOrderNumber = vClassOrder.ClassOrderNumber.ToString();



                    string LockStatus = "";
                    if (vClassOrder.Lock == "鎖定")
                    {
                        LockStatus = $" {vClassOrder.Lock} {vClassOrder.UnautoUnlock} ";

                        //if (vClassOrder.Lock == "鎖定" )
                        //{
                        //    LockStatus = $"{vClassOrder.LockApplyStatus}{vClassOrder.Una";
                        //}
                    }
                    else //不鎖定
                    {
                        if (vClassOrder.LockApplingStatus == ApplyStatus.鎖班申請中_鎖班數超過二分之一.ToString())
                        {
                            LockStatus = $"{vClassOrder.LockApplingStatus}";
                        }
                        if (vClassOrder.LockApplingStatus == ApplyStatus.鎖班申請退回_鎖班數超過二分之一.ToString())
                        {
                            LockStatus = $"{vClassOrder.LockApplingStatus}";
                        }
                    }



                    int rowIndex = grdClassOrder.Rows.Add(
                         vClassOrder.School.Title,
                         vClassOrder.ClassName,
                         vClassOrder.TeacherName,
                         vClassOrder.StudentCount,
                         vClassOrder.SuspensionStudentCount,
                         vClassOrder.DropOutStudentCount,
                         vClassOrder.ClassStudentCount,
                         vClassOrder.NumberReduceCount,
                         vClassOrder.ClassOrderNumber,
                         LockStatus,
                         vClassOrder.Comment,
                         vClassOrder.DistrictComment,
                         vClassOrder.DistrictUulockDate,
                         vGradeYear,
                         vDisplayOrder
                         );
                    grdClassOrder.Rows[rowIndex].Tag = vClassOrder;
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

        private void cmbSchool_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void grdClassOrder_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            ClassOrder classInfo = grdClassOrder.Rows[row].Tag as ClassOrder;
            if (classInfo.LockApplingStatus == ApplyStatus.鎖班申請中_鎖班數超過二分之一.ToString()) //如果學校鎖班狀態是 申請中
            {
                (new frmVerifyLock(classInfo)).ShowDialog();
                LoadClassInfoBySchool();
            }
            else // 非申請中
            {
                return;
            }
        }
    }
}