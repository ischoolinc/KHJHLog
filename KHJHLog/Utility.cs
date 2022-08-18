using Aspose.Cells;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using FISCA.Data;
using System.Data;
using System;
using System.Xml.Linq;

namespace KHJHLog
{
    /// <summary>
    /// 常用函式庫
    /// </summary>
    public class Utility
    {
        private static AccessHelper mHelper = null;

        /// <summary>
        /// 取得AccessHelper
        /// </summary>
        /// <returns></returns>
        public static AccessHelper AccessHelper
        {
            get
            {
                if (mHelper == null)
                    mHelper = new AccessHelper();

                return mHelper;
            }
        }

        /// <summary>
        /// 匯出 Excel
        /// </summary>
        /// <param name="inputReportName"></param>
        /// <param name="inputXls"></param>
        public static void ExprotXls(string ReportName, Workbook wbXls)
        {
            string reportName = ReportName;

            string path = Path.Combine(Application.StartupPath, "Reports");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, reportName + ".xls");

            Workbook wb = wbXls;

            if (File.Exists(path))
            {
                int i = 1;
                while (true)
                {
                    string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + (i++) + Path.GetExtension(path);
                    if (!File.Exists(newPath))
                    {
                        path = newPath;
                        break;
                    }
                }
            }

            try
            {
                wb.Save(path, FileFormatType.Excel2003);
                System.Diagnostics.Process.Start(path);
            }
            catch
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.Title = "另存新檔";
                sd.FileName = reportName + ".xls";
                sd.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd.FileName, FileFormatType.Excel2003);

                    }
                    catch
                    {
                        MsgBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        /// <summary>
        ///  取得 OpenID學校資訊
        /// </summary>
        /// <returns></returns>
        public static List<SchoolOpenIDInfo> GetSchoolOpenIDInfoList()
        {
            List<SchoolOpenIDInfo> value = new List<SchoolOpenIDInfo>();
            string qry = "SELECT school_name,school_id,dsns,school_code FROM $openid.school.list";
            QueryHelper qh = new QueryHelper();
            DataTable dt = qh.Select(qry);
            foreach (DataRow dr in dt.Rows)
            {
                SchoolOpenIDInfo si = new SchoolOpenIDInfo();
                si.SchoolName = dr["school_name"] + "";
                si.SchoolID = dr["school_id"] + "";
                si.SchoolCode = dr["school_code"] + "";
                si.DSNS = dr["dsns"] + "";
                value.Add(si);
            }

            return value;
        }

        public static string WriteOpenSendLog(string ActionName, string ReqContent, string RspContent)
        {
            string value = "";
            ReqContent = ReqContent.Replace("'", "''");
            RspContent = RspContent.Replace("'", "''");
            string query = "INSERT INTO $openid.send.log(action_name,request_content,response_content) VALUES('" + ActionName + "','" + ReqContent + "','" + RspContent + "');";

            try
            {
                UpdateHelper uh = new UpdateHelper();
                uh.Execute(query);
            }
            catch (Exception ex)
            {
                value = ex.Message;
            }

            return value;
        }


        public static string GetElementString(XElement elm, string name)
        {
            string value = "";
            if (elm.Element(name) != null)
                value = elm.Element(name).Value;

            return value;
        }
    }
}