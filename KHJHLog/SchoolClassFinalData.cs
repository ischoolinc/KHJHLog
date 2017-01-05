using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KHJHLog
{
    class SchoolClassFinalData
    {
        /// <summary>
        /// 學校名稱
        /// </summary>
        public string SchoolName { get; set; }


        //2017/1/5 穎驊註解，因應高雄局端 的需求，想要了解各班別的人數、班數統計，局端樣表示分成以下四類
        // 1. 普通班 2.體育班 3.藝才班 4.特教班  
        // 經過確認，其分別對應高雄國中系統 預設類別>> 班級分類 的規則如下:
        
        //普通班：普通班、資優班、資源班、機構式非學校自學班
        //體育班：體育班
        //藝才班：美術班、音樂班、舞蹈班、技藝專班
        //特教班：特教班

        //統計來源可參照class SchoolClassTypeStudentCount 



        /// <summary>
        /// 一年級普通班編班人數
        /// </summary>
        public int NormalClassStudentCount1 { get; set; }

        /// <summary>
        /// 一年級體育班編班人數
        /// </summary>
        public int SportClassStudentCount1 { get; set; }


        /// <summary>
        /// 一年級藝才班編班人數
        /// </summary>
        public int SkillClassStudentCount1 { get; set; }


        /// <summary>
        /// 一年級特教班編班人數
        /// </summary>
        public int IepClassStudentCount1 { get; set; }

        /// <summary>
        /// 二年級普通班編班人數
        /// </summary>
        public int NormalClassStudentCount2 { get; set; }

        /// <summary>
        /// 二年級體育班編班人數
        /// </summary>
        public int SportClassStudentCount2 { get; set; }


        /// <summary>
        /// 二年級藝才班編班人數
        /// </summary>
        public int SkillClassStudentCount2 { get; set; }


        /// <summary>
        /// 二年級特教班編班人數
        /// </summary>
        public int IepClassStudentCount2 { get; set; }

        /// <summary>
        /// 三年級普通班編班人數
        /// </summary>
        public int NormalClassStudentCount3 { get; set; }

        /// <summary>
        /// 三年級體育班編班人數
        /// </summary>
        public int SportClassStudentCount3 { get; set; }


        /// <summary>
        /// 三年級藝才班編班人數
        /// </summary>
        public int SkillClassStudentCount3 { get; set; }


        /// <summary>
        /// 三年級特教班編班人數
        /// </summary>
        public int IepClassStudentCount3 { get; set; }



        /// <summary>
        /// 一年級普通班班數
        /// </summary>
        public int NormalClassCount1 { get; set; }

        /// <summary>
        /// 一年級體育班班數
        /// </summary>
        public int SportClassCount1 { get; set; }


        /// <summary>
        /// 一年級藝才班班數
        /// </summary>
        public int SkillClassCount1 { get; set; }


        /// <summary>
        /// 一年級特教班班數
        /// </summary>
        public int IepClassCount1 { get; set; }

        /// <summary>
        /// 二年級普通班班數
        /// </summary>
        public int NormalClassCount2 { get; set; }

        /// <summary>
        /// 二年級體育班班數
        /// </summary>
        public int SportClassCount2 { get; set; }


        /// <summary>
        /// 二年級藝才班班數
        /// </summary>
        public int SkillClassCount2 { get; set; }


        /// <summary>
        /// 二年級特教班班數
        /// </summary>
        public int IepClassCount2 { get; set; }

        /// <summary>
        /// 三年級普通班班數
        /// </summary>
        public int NormalClassCount3 { get; set; }

        /// <summary>
        /// 三年級體育班班數
        /// </summary>
        public int SportClassCount3 { get; set; }


        /// <summary>
        /// 三年級藝才班班數
        /// </summary>
        public int SkillClassCount3 { get; set; }


        /// <summary>
        /// 三年級特教班班數
        /// </summary>
        public int IepClassCount3 { get; set; }

    }
}
