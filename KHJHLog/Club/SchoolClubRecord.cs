using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KHJHLog
{
    public class SchoolClubRecord
    {
        //專門整理學校內的社團清單資訊物件

        public List<OccurClub> clubList = new List<OccurClub>();

        public List<string> clubNameList = new List<string>();

        public string SchoolYear { get; set; }
        public string Semester { get; set; }

        public School school { get; set; }

        public int 社團數 { get; set; }

        public int 一年級節數 { get; set; }
        public bool 一年級隔周上課 { get; set; }

        public int 二年級節數 { get; set; }
        public bool 二年級隔周上課 { get; set; }

        public int 三年級節數 { get; set; }
        public bool 三年級隔周上課 { get; set; }

        /// <summary>
        /// 開始統計
        /// </summary>
        public void Sum()
        {
            List<string> clubNameList = new List<string>();
            foreach (OccurClub club in clubList)
            {
                if (club.GradeYear == "1" || club.GradeYear == "7")
                {
                    一年級節數++;
                    一年級隔周上課 = club.IsSingleDoubleWeek;
                }
                else if (club.GradeYear == "2" || club.GradeYear == "8")
                {
                    二年級節數++;
                    二年級隔周上課 = club.IsSingleDoubleWeek;
                }
                else if (club.GradeYear == "3" || club.GradeYear == "9")
                {
                    三年級節數++;
                    三年級隔周上課 = club.IsSingleDoubleWeek;
                }

            }
        }

    }
}
