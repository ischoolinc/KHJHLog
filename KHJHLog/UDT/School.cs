using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace KHJHLog
{
    [TableName("school")]
    public class School : ActiveRecord
    {
        [Field(Field = "title", Indexed = true)]
        public string Title { get; set; }

        [Field(Field = "dsns", Indexed = true)]
        public string DSNS { get; set; }

        [Field(Field = "group")]
        public string Group { get; set; }

        [Field(Field = "comment")]
        public string Comment { get; set; }
    }
}
