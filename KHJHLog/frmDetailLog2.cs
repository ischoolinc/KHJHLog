using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using FISCA.Data;
using FISCA.Presentation.Controls;

namespace KHJHLog
{
    public partial class frmDetailLog2 : BaseForm
    {
        private DataGridViewRow Row;

        public frmDetailLog2(DataGridViewRow Row)
        {
            InitializeComponent();

            this.Row = Row;
        }

        private void frmDetailLog_Load(object sender, EventArgs e)
        {
            txtDate.Text = "" + Row.Cells["colDate"].Value;
            txtSchool.Text = "" + Row.Cells["colSchool"].Value;
            txtAction.Text = "" + Row.Cells["colAction"].Value;
            txtContent.Text = "" + Row.Cells["colContent"].Value;
            txtComment.Text = "" + Row.Cells["colComment"].Value;
            string UID = "" + Row.Cells["colID"].Value;

            QueryHelper query = new QueryHelper();

            DataTable table = query.Select("select detail from $school_log where uid=" + UID);

            if (table.Rows.Count > 0)
            {
                string Detail = table.Rows[0].Field<string>("detail");

                try
                {
                    XElement elmContent = XElement.Load(new StringReader("<root>" + Detail + "</root>"));

                    if (txtAction.Text.Equals("匯入新增學生"))
                    {
                        //<Detail>
                        //  <Student>
                        //    <IDNumber>   </IDNumber>
                        //    <StudentNumber> </StudentNumber>
                        //   <StudentName>   </StudentName>
                        //               <GradeYear></GradeYear>
                        //   <ClassName> </ClassName>
                        //   <SeatNo> </SeatNo>
                        //  </Student>
                        //</Detail>

                        grdDetail.Columns.Add("身分證", "身分證");
                        grdDetail.Columns.Add("學號", "學號");
                        grdDetail.Columns.Add("學生姓名", "學生姓名");
                        grdDetail.Columns.Add("班級名稱", "班級名稱");
                        grdDetail.Columns.Add("座號", "座號");

                        foreach (XElement elmStudent in elmContent.Elements("Student"))
                        {
                            grdDetail.Rows.Add(
                                txtDate.Text,
                                txtSchool.Text,
                                txtAction.Text,
                                elmStudent.ElementText("IDNumber"),
                                elmStudent.ElementText("StudentNumber"),
                                elmStudent.ElementText("StudentName"),
                                elmStudent.ElementText("ClassName"),
                                elmStudent.ElementText("SeatNo"));
                        }                   
                    }
                    else if (txtAction.Text.Equals("匯入更新班級"))
                    {
                        grdDetail.Columns.Add("身分證", "身分證");
                        grdDetail.Columns.Add("學號", "學號");
                        grdDetail.Columns.Add("學生姓名", "學生姓名");
                        grdDetail.Columns.Add("原班級", "原班級");
                        grdDetail.Columns.Add("新班級", "新班級");

                        foreach (XElement elmStudent in elmContent.Elements("Student"))
                        {
                            grdDetail.Rows.Add(
                                txtDate.Text,
                                txtSchool.Text,
                                txtAction.Text,
                                elmStudent.ElementText("IDNumber"),
                                elmStudent.ElementText("StudentNumber"),
                                elmStudent.ElementText("StudentName"),
                                elmStudent.ElementText("ClassName"),
                                elmStudent.ElementText("NewClassName"));
                        }
                    }
                    else if (txtAction.Text.Equals("匯入更新"))
                    {
                        grdDetail.Columns.Add("身分證", "身分證");
                        grdDetail.Columns.Add("學號", "學號");
                        grdDetail.Columns.Add("學生姓名", "學生姓名");
                        grdDetail.Columns.Add("原班級", "原班級");
                        grdDetail.Columns.Add("新班級", "新班級");
                        grdDetail.Columns.Add("原狀態", "原狀態");
                        grdDetail.Columns.Add("新狀態", "新狀態");

                        foreach (XElement elmStudent in elmContent.Elements("Student"))
                        {
                            grdDetail.Rows.Add(
                                txtDate.Text,
                                txtSchool.Text,
                                txtAction.Text,
                                elmStudent.ElementText("IDNumber"),
                                elmStudent.ElementText("StudentNumber"),
                                elmStudent.ElementText("StudentName"),
                                elmStudent.ElementText("ClassName"),
                                elmStudent.ElementText("NewClassName"),
                                elmStudent.ElementText("StudentStatus"),
                                elmStudent.ElementText("NewStudentStatus")
                                );
                        }
                    }
                    else if (txtAction.Text.Equals("匯入特殊身分"))
                    {
                        //<Content>
                        //    <Summary>
                        //    匯入特殊身分學生共...筆。
                        //    </Summary>
                        //</Content>
                        //<Detail>
                        //    <Student>
                        //    <IDNumber>S125514930</IDNumber>
                        //    <StudentNumber>910029</StudentNumber>
                        //    <StudentName>吳昱志</StudentName>
                        //    <ClassName>704</ClassName>
                        //    <SeatNo>3</SeatNo>
                        //    <NumberReduce>0</NumberReduce>
                        //    <DocNo>測試用文號00001</DocNo>
                        //</Detail>

                        grdDetail.Columns.Add("身分證", "身分證");
                        grdDetail.Columns.Add("學號", "學號");
                        grdDetail.Columns.Add("學生姓名", "學生姓名");
                        grdDetail.Columns.Add("班級名稱", "班級名稱");
                        grdDetail.Columns.Add("座號", "座號");
                        grdDetail.Columns.Add("減免人數", "減免人數");
                        grdDetail.Columns.Add("文號", "文號");
                       
                        foreach (XElement elmStudent in elmContent.Elements("Student"))
                        {
                            grdDetail.Rows.Add(
                                txtDate.Text,
                                txtSchool.Text,
                                txtAction.Text,
                                elmStudent.ElementText("IDNumber"),
                                elmStudent.ElementText("StudentNumber"),
                                elmStudent.ElementText("StudentName"),
                                elmStudent.ElementText("ClassName"),
                                elmStudent.ElementText("SeatNo"),
                                elmStudent.ElementText("NumberReduce"),
                                elmStudent.ElementText("DocNo"));
                        }
                    }
                }
                catch
                {
                    
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            grdDetail.Export(txtAction.Text + "明細");
        }
    }
}