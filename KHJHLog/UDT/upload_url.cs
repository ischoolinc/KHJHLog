using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace KHJHLog
{
    /// <summary>
    /// 上傳位置    
    /// </summary>
    [TableName("upload_url")]
    public class upload_url:ActiveRecord
    {
        /// <summary>
        /// 學校DSNS
        /// </summary>
        [Field(Field = "dsns")]
        public string DSNS { get; set; }

        /// <summary>
        /// student/class
        /// </summary>
        [Field(Field = "type")]
        public string Type { get; set; }

        /// <summary>
        /// 系統編號
        /// </summary>
        [Field(Field = "id")]
        public int Id { get; set; }

        /// <summary>
        /// 網址
        /// </summary>
        [Field(Field = "url")]
        public string Url { get; set; }
    }
}
