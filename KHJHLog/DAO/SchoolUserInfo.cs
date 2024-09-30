using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KHJHLog.DAO
{
    public class SchoolUserInfo
    {
        // 學校名稱
        public string SchoolName { get; set; }

        // 使用帳號
        public string Account { get; set; }

        // 最後使用日期
        public string LastUseDate { get; set; }

        // 登入IP
        public string LoginIP { get; set; }

        // 最高權限
        public string HighestPermission { get; set; }
        
    }
}
