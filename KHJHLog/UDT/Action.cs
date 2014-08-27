using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA.UDT;

namespace KHJHLog
{
    /// <summary>
    /// 學校歷程
    /// </summary>
    [TableName("action")]
    public class Action : FISCA.UDT.ActiveRecord
    {
        /// <summary>
        /// 學校端DSNS
        /// </summary>
        [Field(Field = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 動作
        /// </summary>
        [Field(Field = "verify")]
        public bool Verify { get; set; }
    }
}