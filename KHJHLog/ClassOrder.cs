
namespace KHJHLog
{
    public class ClassOrder
    {
        /// <summary>
        /// 班級名稱
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 實際人數
        /// </summary>
        public string StudentCount { get; set; }

        /// <summary>
        /// 編班人數
        /// </summary>
        public string ClassStudentCount { get; set; }

        /// <summary>
        /// 編班人數數值
        /// </summary>
        public int ClassStudentCountValue { get; set; }

        /// <summary>
        /// 減免人數
        /// </summary>
        public string NumberReduceSum { get; set; }

        /// <summary>
        /// 特殊生人數
        /// </summary>
        public string NumberReduceCount { get; set; }

        /// <summary>
        /// 鎖定
        /// </summary>
        public string Lock { get; set; }

        /// <summary>
        /// 註記
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 編班順位
        /// </summary>
        public string ClassOrderNumber { get; set; }
    }
}