using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHJHLog.DAO
{
    public class SchoolSysAdminInfo
    {
        // 學校名稱
        public string school_name { get; set; }

        // 系統管理師
        public string associated_with_name { get; set; }

        // 手機
        public string associated_with_cell_phone { get; set; }

        // Email
        public string associated_with_email { get; set; }

        // 修改日期
        public string server_time { get; set; }

    }
}
