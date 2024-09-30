using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Cells;
using Campus.Report;

namespace KHJHLog
{
    //匯出DataGridView內容的小工具
    internal static class DataGridViewExport
    {
        /// <summary>
        /// Export DataGridView Contents
        /// </summary>
        /// <param name="dgv">DataGridView Control</param>
        /// <param name="name">Export Report Name</param>
        internal static void Export(this DataGridView dgv, string name)
        {
            Workbook book = new Workbook();
            book.Worksheets.Clear();
            Worksheet ws = book.Worksheets[book.Worksheets.Add()];
            ws.Name = name;

            int index = 0;
            Dictionary<string, int> map = new Dictionary<string, int>();

            #region 建立標題
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                DataGridViewColumn col = dgv.Columns[i];
                ws.Cells[index, i].PutValue(col.HeaderText);
                map.Add(col.HeaderText, i);
            }
            index++;
            #endregion

            #region 填入內容
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    int column = map[cell.OwningColumn.HeaderText];
                    ws.Cells[index, column].PutValue("" + cell.Value);
                }
                index++;
            }
            #endregion

            book.Worksheets[0].AutoFitColumns();

            //ReportSaver.SaveWorkbook(book, name);
            Utility.ExportXls(name, book);
        }
    }
}