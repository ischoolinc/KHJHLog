using FISCA.Presentation.Controls;
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
    public partial class frmClubDetail : BaseForm
    {
        SchoolClubRecord _club;
        public frmClubDetail(SchoolClubRecord club)
        {
            InitializeComponent();

            _club = club;

            labelX1.Text = string.Format("第{0}學年度 第{1}學期 社團清單", _club.SchoolYear, _club.Semester);

            //社團清單
            _club.clubNameList.Sort();
            foreach (string each in _club.clubNameList)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridViewX1);
                row.Cells[0].Value = each;

                dataGridViewX1.Rows.Add(row);
            }


            _club.clubList.Sort(GDsort);
            foreach (OccurClub each in _club.clubList)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridViewX2);
                row.Cells[0].Value = each.GradeYear;

                if (each.OccurDate != null)
                    row.Cells[1].Value = each.OccurDate.ToString("yyyy/MM/dd");

                row.Cells[2].Value = each.Week;
                row.Cells[3].Value = each.Period;
                dataGridViewX2.Rows.Add(row);
            }

        }

        private int GDsort(OccurClub x, OccurClub y)
        {
            string geName = x.GradeYear + x.OccurDate.ToString("yyyy/MM/dd");
            string chName = y.GradeYear + y.OccurDate.ToString("yyyy/MM/dd");

            return geName.CompareTo(chName);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
