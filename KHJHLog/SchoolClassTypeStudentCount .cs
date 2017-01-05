using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KHJHLog
{
    public class SchoolClassTypeStudentCount
    {
        /// <summary>
        /// 學校名稱
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 班級名稱
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 編班人數
        /// </summary>
        public int ClassStudentCount { get; set; }

        /// <summary>
        /// 年級
        /// </summary>
        public string GradeYear { get; set; }


        /// <summary>
        /// 是否為 班級分類: 普通班
        /// </summary>
        public bool NormalClass { get; set; }

        /// <summary>
        /// 是否為 班級分類: 體育班
        /// </summary>
        public bool SportClass { get; set; }
                  
        /// <summary>
        /// 是否為 班級分類: 美術班
        /// </summary>
        public bool ArtClass { get; set; }
                
        /// <summary>
        /// 是否為 班級分類: 音樂班
        /// </summary>
        public bool MusicClass { get; set; }
                
        /// <summary>
        /// 是否為 班級分類: 舞蹈班
        /// </summary>
        public bool DanceClass { get; set; }
                
        /// <summary>
        /// 是否為 班級分類: 資優班
        /// </summary>
        public bool GiftedClass { get; set; }
                
        /// <summary>
        /// 是否為 班級分類: 資源班
        /// </summary>
        public bool ResourceClass { get; set; }
                
        /// <summary>
        /// 是否為 班級分類: 特教班
        /// </summary>
        public bool IepClass { get; set; }
                
        /// <summary>
        /// 是否為 班級分類: 技藝專班
        /// </summary>
        public bool SkillClass { get; set; }
                
        /// <summary>
        /// 是否為 班級分類: 機構式非學校自學班
        /// </summary>
        public bool NoSchoolClass { get; set; }





    }
}
