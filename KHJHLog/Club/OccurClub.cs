using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace KHJHLog
{
    public class OccurClub
    {
        public OccurClub(XElement elmClub)
        {
            //年級
            GradeYear = elmClub.ElementText("GradeYear");

            //上課日期
            DateTime dt;
            if (DateTime.TryParse(elmClub.ElementText("OccurDate"), out dt))
            {
                OccurDate = dt;
            }

            //節次
            Period = elmClub.ElementText("Period");

            //星期
            Week = elmClub.ElementText("Week");

            //True為隔周上課/False為每周上課
            IsSingleDoubleWeek = elmClub.ElementText("IsSingleDoubleWeek") == "t" ? true : false;

        }
        public string GradeYear { get; set; }
        public DateTime OccurDate { get; set; }
        public string Period { get; set; }
        public string Week { get; set; }

        public bool IsSingleDoubleWeek { get; set; }
    }
}
