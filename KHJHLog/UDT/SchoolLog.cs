using System;
using FISCA.UDT;

namespace KHJHLog
{
    /// <summary>
    /// 學校歷程
    /// </summary>
    [TableName("school_log")]
    public class SchoolLog : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 學校端DSNS
        /// </summary>
        [Field(Field = "dsns")]
        public string DSNS { get; set;}

        /// <summary>
        /// 動作
        /// </summary>
        [Field(Field = "action")]
        public string Action { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        [Field(Field = "content")]
        public string Content { get; set; }

        /// <summary>
        /// 是否審核
        /// </summary>
        [Field(Field = "is_verify")]
        public string IsVerify { get; set; }

        /// <summary>
        /// 註解
        /// </summary>
        [Field(Field = "comment")]
        public string Comment { get; set; }

        /// <summary>
        /// 新增時間
        /// </summary>
        [Field(Field = "date_time")]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 明細
        /// </summary>
        [Field(Field = "detail")]
        public string Detail { get; set; }

    }
}