﻿
namespace KHJHLog
{
    class Permissions
    {
        public static string 查詢紀錄 { get { return "44e329e1-ffb9-429f-9579-3d30dcb4f6b8"; } }

        public static bool 查詢紀錄權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[查詢紀錄].Executable;
            }
        }

        public static string 動作設定 { get { return "a348c6f6-a7d4-472b-aa60-752554d9dc77"; } }

        public static bool 動作設定權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[動作設定].Executable;
            }
        }

        public static string 傳送學生OpenID { get { return "A13BA3F7-03D4-468A-B59F-022694A45E61"; } }

        public static bool 傳送學生OpenID權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[傳送學生OpenID].Executable;
            }
        }

        public static string 批次傳送學生OpenID { get { return "475F0342-66C2-4F5B-A0D3-581C579F58D5"; } }

        public static bool 批次傳送學生OpenID權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[批次傳送學生OpenID].Executable;
            }
        }

        public static string 批次傳送班級OpenID { get { return "ABA761BE-8430-43E7-917D-47423E8075C7"; } }

        public static bool 批次傳送班級OpenID權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[批次傳送班級OpenID].Executable;
            }
        }
    }
}