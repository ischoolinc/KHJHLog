using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.DSAClient;
using System.Xml.Linq;
using System.IO;
using Aspose.Cells;
using FISCA.Data;
using DevComponents.DotNetBar;

namespace KHJHLog
{
    public partial class frmAutoSyncView : BaseForm
    {
        BackgroundWorker bgWork;
        int Limit = 50;
        bool IsAutoSyncChecked = true;
        DataTable dt;

        public frmAutoSyncView()
        {
            InitializeComponent();
            bgWork = new BackgroundWorker();
            bgWork.DoWork += BwWork_DoWork;
            bgWork.RunWorkerCompleted += BwWork_RunWorkerCompleted;
            bgWork.ProgressChanged += BwWork_ProgressChanged;
            bgWork.WorkerReportsProgress = true;
            dt = new DataTable();
        }

        private void BwWork_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void BwWork_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsAutoSyncChecked)
            {
                // 載入對應欄位
                LoadAutoSynColumns();

                // 載入資料
                LoadAutoSyncDataGridViewData();

            }
            else
            {
                LoadManSyncColumns();

                LoadManSyncDataGridViewData();
            }
            btnSearch.Enabled = true;
        }

        private void BwWork_DoWork(object sender, DoWorkEventArgs e)
        {
            string strQuery = "";

            if (IsAutoSyncChecked)
            {

                // 自動同步
                strQuery = @"
                SELECT
                    school_name AS 學校名稱,
                    class_grade_year AS 年級,
                    student_number AS 學號,
                    class_name AS 班級,
                    student_seat_no AS 座號,
                    student_name AS 姓名,
                    student_gender AS 性別,
                    student_id_number AS 身分證號,
                    student_birthday AS 生日,
                    student_id AS 學生系統編號,
                    school_dsns AS 學校主機,
                    student_status_code AS 學生狀態碼,
                    sync_excute_time AS 執行時間,
                    sync_action1 AS 傳送內容1,
                    sync_action2 AS 傳送內容2,
                    sync_action3 AS 傳送內容3,
                    sync_action_result1 AS 回傳內容1,
                    sync_action_result2 AS 回傳內容2,
                    sync_action_result3 AS 回傳內容3,
                    school_id AS OpenID學校編號
                FROM
                    $openid.autosync.log
                ORDER BY
                    last_update DESC,
                    uid DESC
                LIMIT
                    " + Limit + ";";

            }
            else
            {
                // 取得手動同步資料
                strQuery = @"
                SELECT
                    uid,
                    last_update,
                    request_content,
                    response_content
                FROM
                    $openid.send.log
                WHERE
                    action_name = '傳送轉學學生OpenID'
                ORDER BY
                    last_update DESC,
                    uid DESC
                LIMIT
                    " + Limit + ";";
            }

//            // query debug 用
//            strQuery = @"
//SELECT
//	uid,
//	last_update,
//	request_content,
//	response_content
//FROM
//	$openid.send.log
//WHERE
//	action_name = '傳送轉學學生OpenID'
// AND request_content like '%A231097250%'
//UNION ALL
//SELECT
//	uid,
//	last_update,
//	request_content,
//	response_content
//FROM
//	$openid.send.log
//WHERE
//	action_name = '傳送轉學學生OpenID'
// AND request_content like '%H127051269%'
// UNION ALL
//SELECT
//	uid,
//	last_update,
//	request_content,
//	response_content
//FROM
//	$openid.send.log
//WHERE
//	action_name = '傳送轉學學生OpenID'
// AND request_content like '%D123667623%'
// UNION ALL
//SELECT
//	uid,
//	last_update,
//	request_content,
//	response_content
//FROM
//	$openid.send.log
//WHERE
//	action_name = '傳送轉學學生OpenID'
// AND request_content like '%O100887216%'
//    ORDER BY
//                    last_update DESC,
//                    uid DESC

//";




            QueryHelper qh = new QueryHelper();
            dt = qh.Select(strQuery);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Run();
        }

        private void Run()
        {
            Limit = intLimit.Value;
            IsAutoSyncChecked = chkAutoSync.Checked;
            btnSearch.Enabled = false;
            bgWork.RunWorkerAsync();
        }

        private void frmAutoSyncView_Load(object sender, EventArgs e)
        {
            // 預設50筆
            intLimit.Value = 50;
            chkAutoSync.Checked = true;
            Run();
        }

        private void LoadAutoSynColumns()
        {
            dgData.Columns.Clear();
            DataGridViewTextBoxColumn tb_school_name = new DataGridViewTextBoxColumn();
            tb_school_name.Name = "學校名稱";
            tb_school_name.Width = 120;
            tb_school_name.HeaderText = "學校名稱";
            tb_school_name.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_school_name.ReadOnly = true;

            DataGridViewTextBoxColumn tb_class_grade_year = new DataGridViewTextBoxColumn();
            tb_class_grade_year.Name = "年級";
            tb_class_grade_year.Width = 40;
            tb_class_grade_year.HeaderText = "年級";
            tb_class_grade_year.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_class_grade_year.ReadOnly = true;

            DataGridViewTextBoxColumn tb_student_number = new DataGridViewTextBoxColumn();
            tb_student_number.Name = "學號";
            tb_student_number.Width = 80;
            tb_student_number.HeaderText = "學號";
            tb_student_number.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_student_number.ReadOnly = true;

            DataGridViewTextBoxColumn tb_class_name = new DataGridViewTextBoxColumn();
            tb_class_name.Name = "班級";
            tb_class_name.Width = 60;
            tb_class_name.HeaderText = "班級";
            tb_class_name.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_class_name.ReadOnly = true;

            DataGridViewTextBoxColumn tb_student_seat_no = new DataGridViewTextBoxColumn();
            tb_student_seat_no.Name = "座號";
            tb_student_seat_no.Width = 30;
            tb_student_seat_no.HeaderText = "座號";
            tb_student_seat_no.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_student_seat_no.ReadOnly = true;

            DataGridViewTextBoxColumn tb_student_name = new DataGridViewTextBoxColumn();
            tb_student_name.Name = "姓名";
            tb_student_name.Width = 80;
            tb_student_name.HeaderText = "姓名";
            tb_student_name.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_student_name.ReadOnly = true;

            DataGridViewTextBoxColumn tb_student_gender = new DataGridViewTextBoxColumn();
            tb_student_gender.Name = "性別";
            tb_student_gender.Width = 30;
            tb_student_gender.HeaderText = "性別";
            tb_student_gender.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_student_gender.ReadOnly = true;

            DataGridViewTextBoxColumn tb_student_id_number = new DataGridViewTextBoxColumn();
            tb_student_id_number.Name = "身分證號";
            tb_student_id_number.Width = 100;
            tb_student_id_number.HeaderText = "身分證號";
            tb_student_id_number.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_student_id_number.ReadOnly = true;

            DataGridViewTextBoxColumn tb_student_birthday = new DataGridViewTextBoxColumn();
            tb_student_birthday.Name = "生日";
            tb_student_birthday.Width = 80;
            tb_student_birthday.HeaderText = "生日";
            tb_student_birthday.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_student_birthday.ReadOnly = true;

            DataGridViewTextBoxColumn tb_student_id = new DataGridViewTextBoxColumn();
            tb_student_id.Name = "學生系統編號";
            tb_student_id.Width = 60;
            tb_student_id.HeaderText = "學生系統編號";
            tb_student_id.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_student_id.ReadOnly = true;

            DataGridViewTextBoxColumn tb_school_dsns = new DataGridViewTextBoxColumn();
            tb_school_dsns.Name = "學校主機";
            tb_school_dsns.Width = 80;
            tb_school_dsns.HeaderText = "學校主機";
            tb_school_dsns.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_school_dsns.ReadOnly = true;

            DataGridViewTextBoxColumn tb_student_status_code = new DataGridViewTextBoxColumn();
            tb_student_status_code.Name = "學生狀態碼";
            tb_student_status_code.Width = 30;
            tb_student_status_code.HeaderText = "學生狀態碼";
            tb_student_status_code.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_student_status_code.ReadOnly = true;

            DataGridViewTextBoxColumn tb_sync_excute_time = new DataGridViewTextBoxColumn();
            tb_sync_excute_time.Name = "執行時間";
            tb_sync_excute_time.Width = 140;
            tb_sync_excute_time.HeaderText = "執行時間";
            tb_sync_excute_time.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_sync_excute_time.ReadOnly = true;

            DataGridViewTextBoxColumn tb_sync_action1 = new DataGridViewTextBoxColumn();
            tb_sync_action1.Name = "傳送內容1";
            tb_sync_action1.Width = 100;
            tb_sync_action1.HeaderText = "傳送內容1";
            tb_sync_action1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_sync_action1.ReadOnly = true;

            DataGridViewTextBoxColumn tb_sync_action2 = new DataGridViewTextBoxColumn();
            tb_sync_action2.Name = "傳送內容2";
            tb_sync_action2.Width = 100;
            tb_sync_action2.HeaderText = "傳送內容2";
            tb_sync_action2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_sync_action2.ReadOnly = true;

            DataGridViewTextBoxColumn tb_sync_action3 = new DataGridViewTextBoxColumn();
            tb_sync_action3.Name = "傳送內容3";
            tb_sync_action3.Width = 100;
            tb_sync_action3.HeaderText = "傳送內容3";
            tb_sync_action3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_sync_action3.ReadOnly = true;

            DataGridViewTextBoxColumn tb_sync_action_result1 = new DataGridViewTextBoxColumn();
            tb_sync_action_result1.Name = "回傳內容1";
            tb_sync_action_result1.Width = 100;
            tb_sync_action_result1.HeaderText = "回傳內容1";
            tb_sync_action_result1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_sync_action_result1.ReadOnly = true;

            DataGridViewTextBoxColumn tb_sync_action_result2 = new DataGridViewTextBoxColumn();
            tb_sync_action_result2.Name = "回傳內容2";
            tb_sync_action_result2.Width = 100;
            tb_sync_action_result2.HeaderText = "回傳內容2";
            tb_sync_action_result2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_sync_action_result2.ReadOnly = true;

            DataGridViewTextBoxColumn tb_sync_action_result3 = new DataGridViewTextBoxColumn();
            tb_sync_action_result3.Name = "回傳內容3";
            tb_sync_action_result3.Width = 100;
            tb_sync_action_result3.HeaderText = "回傳內容3";
            tb_sync_action_result3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_sync_action_result3.ReadOnly = true;

            DataGridViewTextBoxColumn tb_school_id = new DataGridViewTextBoxColumn();
            tb_school_id.Name = "OpenID學校編號";
            tb_school_id.Width = 60;
            tb_school_id.HeaderText = "OpenID學校編號";
            tb_school_id.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tb_school_id.ReadOnly = true;

            dgData.Columns.Add(tb_school_name);
            dgData.Columns.Add(tb_class_grade_year);
            dgData.Columns.Add(tb_student_number);
            dgData.Columns.Add(tb_class_name);
            dgData.Columns.Add(tb_student_seat_no);
            dgData.Columns.Add(tb_student_name);
            dgData.Columns.Add(tb_student_gender);
            dgData.Columns.Add(tb_student_id_number);
            dgData.Columns.Add(tb_student_birthday);
            dgData.Columns.Add(tb_student_id);
            dgData.Columns.Add(tb_school_dsns);
            dgData.Columns.Add(tb_student_status_code);
            dgData.Columns.Add(tb_sync_excute_time);
            dgData.Columns.Add(tb_sync_action1);
            dgData.Columns.Add(tb_sync_action2);
            dgData.Columns.Add(tb_sync_action3);
            dgData.Columns.Add(tb_sync_action_result1);
            dgData.Columns.Add(tb_sync_action_result2);
            dgData.Columns.Add(tb_sync_action_result3);
            dgData.Columns.Add(tb_school_id);


        }

        private void LoadAutoSyncDataGridViewData()
        {

            dgData.Rows.Clear();
            try
            {
                if (dt != null)
                {
                    int rowCount = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        int rowIdx = dgData.Rows.Add();
                        dgData.Rows[rowIdx].Cells["學校名稱"].Value = dr["學校名稱"] + "";
                        dgData.Rows[rowIdx].Cells["年級"].Value = dr["年級"] + "";
                        dgData.Rows[rowIdx].Cells["學號"].Value = dr["學號"] + "";
                        dgData.Rows[rowIdx].Cells["班級"].Value = dr["班級"] + "";
                        dgData.Rows[rowIdx].Cells["座號"].Value = dr["座號"] + "";
                        dgData.Rows[rowIdx].Cells["姓名"].Value = dr["姓名"] + "";
                        dgData.Rows[rowIdx].Cells["性別"].Value = dr["性別"] + "";
                        dgData.Rows[rowIdx].Cells["身分證號"].Value = dr["身分證號"] + "";
                        dgData.Rows[rowIdx].Cells["生日"].Value = dr["生日"] + "";
                        dgData.Rows[rowIdx].Cells["學生系統編號"].Value = dr["學生系統編號"] + "";
                        dgData.Rows[rowIdx].Cells["學校主機"].Value = dr["學校主機"] + "";
                        dgData.Rows[rowIdx].Cells["學生狀態碼"].Value = dr["學生狀態碼"] + "";
                        dgData.Rows[rowIdx].Cells["執行時間"].Value = dr["執行時間"] + "";
                        dgData.Rows[rowIdx].Cells["傳送內容1"].Value = dr["傳送內容1"] + "";
                        dgData.Rows[rowIdx].Cells["傳送內容2"].Value = dr["傳送內容2"] + "";
                        dgData.Rows[rowIdx].Cells["傳送內容3"].Value = dr["傳送內容3"] + "";
                        dgData.Rows[rowIdx].Cells["回傳內容1"].Value = dr["回傳內容1"] + "";
                        dgData.Rows[rowIdx].Cells["回傳內容2"].Value = dr["回傳內容2"] + "";
                        dgData.Rows[rowIdx].Cells["回傳內容3"].Value = dr["回傳內容3"] + "";
                        dgData.Rows[rowIdx].Cells["OpenID學校編號"].Value = dr["OpenID學校編號"] + "";
                        rowCount++;

                    }

                    SetLblCount(rowCount);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void SetLblCount(int num)
        {
            lblCount.Text = "共" + num + "筆";
        }

        private void LoadManSyncDataGridViewData()
        {
            dgData.Rows.Clear();
            try
            {
                if (dt != null)
                {
                    int rowCount = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        int rowIdx = dgData.Rows.Add();
                        dgData.Rows[rowIdx].Cells["執行時間"].Value = dr["last_update"] + "";

                        // 解析 XML
                        try
                        {
                            XElement reqXML = XElement.Parse(dr["request_content"] + "");
                            XElement rspXML = XElement.Parse(dr["response_content"] + "");
                            dgData.Rows[rowIdx].Cells["身分證號"].Value = GetElement(reqXML, "IDNumber");
                            dgData.Rows[rowIdx].Cells["姓名"].Value = GetElement(reqXML, "StudentName");
                            dgData.Rows[rowIdx].Cells["學號"].Value = GetElement(reqXML, "StudentNumber");
                            dgData.Rows[rowIdx].Cells["轉入學校"].Value = GetElement(reqXML, "SchoolName");
                            dgData.Rows[rowIdx].Cells["傳送1"].Value = GetElement(reqXML, "Req1");
                            dgData.Rows[rowIdx].Cells["傳送2"].Value = GetElement(reqXML, "Req2");
                            dgData.Rows[rowIdx].Cells["傳送3"].Value = GetElement(reqXML, "Req3");
                            dgData.Rows[rowIdx].Cells["回傳1"].Value = GetElement(rspXML, "Rsp1");
                            dgData.Rows[rowIdx].Cells["回傳2"].Value = GetElement(rspXML, "Rsp2");
                            dgData.Rows[rowIdx].Cells["回傳3"].Value = GetElement(rspXML, "Rsp3");
                            rowCount++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }

                    SetLblCount(rowCount);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private string GetElement(XElement elm, string name)
        {
            string value = "";

            if (elm.Element(name) != null)
                value = elm.Element(name).Value;

            return value;
        }

        private void LoadManSyncColumns()
        {
            dgData.Columns.Clear();

            DataGridViewTextBoxColumn tbDate = new DataGridViewTextBoxColumn();
            tbDate.Name = "執行時間";
            tbDate.Width = 140;
            tbDate.HeaderText = "執行時間";
            tbDate.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbIDNumber = new DataGridViewTextBoxColumn();
            tbIDNumber.Name = "身分證號";
            tbIDNumber.Width = 100;
            tbIDNumber.HeaderText = "身分證號";
            tbIDNumber.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbName = new DataGridViewTextBoxColumn();
            tbName.Name = "姓名";
            tbName.Width = 100;
            tbName.HeaderText = "姓名";
            tbName.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbStudentNumber = new DataGridViewTextBoxColumn();
            tbStudentNumber.Name = "學號";
            tbStudentNumber.Width = 80;
            tbStudentNumber.HeaderText = "學號";
            tbStudentNumber.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbImportSchool = new DataGridViewTextBoxColumn();
            tbImportSchool.Name = "轉入學校";
            tbImportSchool.Width = 100;
            tbImportSchool.HeaderText = "轉入學校";
            tbImportSchool.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbReq1 = new DataGridViewTextBoxColumn();
            tbReq1.Name = "傳送1";
            tbReq1.Width = 100;
            tbReq1.HeaderText = "傳送1";
            tbReq1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbReq2 = new DataGridViewTextBoxColumn();
            tbReq2.Name = "傳送2";
            tbReq2.Width = 100;
            tbReq2.HeaderText = "傳送2";
            tbReq2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbReq3 = new DataGridViewTextBoxColumn();
            tbReq3.Name = "傳送3";
            tbReq3.Width = 100;
            tbReq3.HeaderText = "傳送3";
            tbReq3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbRsp1 = new DataGridViewTextBoxColumn();
            tbRsp1.Name = "回傳1";
            tbRsp1.Width = 100;
            tbRsp1.HeaderText = "回傳1";
            tbRsp1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbRsp2 = new DataGridViewTextBoxColumn();
            tbRsp2.Name = "回傳2";
            tbRsp2.Width = 100;
            tbRsp2.HeaderText = "回傳2";
            tbRsp2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DataGridViewTextBoxColumn tbRsp3 = new DataGridViewTextBoxColumn();
            tbRsp3.Name = "回傳3";
            tbRsp3.Width = 100;
            tbRsp3.HeaderText = "回傳3";
            tbRsp3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgData.Columns.Add(tbDate);
            dgData.Columns.Add(tbIDNumber);
            dgData.Columns.Add(tbName);
            dgData.Columns.Add(tbStudentNumber);
            dgData.Columns.Add(tbImportSchool);
            dgData.Columns.Add(tbReq1);
            dgData.Columns.Add(tbReq2);
            dgData.Columns.Add(tbReq3);
            dgData.Columns.Add(tbRsp1);
            dgData.Columns.Add(tbRsp2);
            dgData.Columns.Add(tbRsp3);

        }

    }
}
