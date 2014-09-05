using FISCA;
using FISCA.Permission;
using FISCA.Presentation;
using FISCA.UDT;

namespace KHJHLog
{
    public static class Program
    {
        public static NLDPanel MainPanel { get; private set; }

        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [MainMethod]
        public static void Main()
        {
            Campus.Configuration.Config.Initialize(
                new Campus.Configuration.UserConfigManager(new Campus.Configuration.ConfigProvider_User(), FISCA.Authentication.DSAServices.UserAccount),
                new Campus.Configuration.ConfigurationManager(new Campus.Configuration.ConfigProvider_App()),
                new Campus.Configuration.ConfigurationManager(new Campus.Configuration.ConfigProvider_Global())
            );

            InitMainPanel();

            SchemaManager Manager = new SchemaManager(FISCA.Authentication.DSAServices.DefaultConnection);

            Manager.SyncSchema(new SchoolLog());
            Manager.SyncSchema(new Action());

            MainPanel.RibbonBarItems["自動編班"]["動作設定"].Image = Properties.Resources.achievement_config_128;
            MainPanel.RibbonBarItems["自動編班"]["動作設定"].Size = RibbonBarButton.MenuButtonSize.Medium;
            MainPanel.RibbonBarItems["自動編班"]["動作設定"].Click += (sender, e) => new frmActionList().ShowDialog();
            MainPanel.RibbonBarItems["自動編班"]["動作設定"].Enable = Permissions.動作設定權限;

            MainPanel.RibbonBarItems["自動編班"]["查詢紀錄"].Image = Properties.Resources.admissions_search_128;
            MainPanel.RibbonBarItems["自動編班"]["查詢紀錄"].Size = RibbonBarButton.MenuButtonSize.Medium;
            MainPanel.RibbonBarItems["自動編班"]["查詢紀錄"].Click += (sender, e) => new QueryLog().ShowDialog();
            MainPanel.RibbonBarItems["自動編班"]["查詢紀錄"].Enable = Permissions.查詢紀錄權限;

            MainPanel.RibbonBarItems["自動編班"]["查詢編班"].Click += (sender, e) => new frmClassOrder().ShowDialog();
            MainPanel.RibbonBarItems["自動編班"]["查詢編班"].Image = Properties.Resources.classmate_128;
            MainPanel.RibbonBarItems["自動編班"]["查詢編班"].Size = RibbonBarButton.MenuButtonSize.Medium;

            MainPanel.RibbonBarItems["自動編班"]["查詢高關懷學生"].Click += (sender, e) => new frmSpecial().ShowDialog();
            MainPanel.RibbonBarItems["自動編班"]["查詢高關懷學生"].Image = Properties.Resources.student_a_128;
            MainPanel.RibbonBarItems["自動編班"]["查詢高關懷學生"].Size = RibbonBarButton.MenuButtonSize.Medium;

            FISCA.Permission.Catalog AdminCatalog = FISCA.Permission.RoleAclSource.Instance["自動編班"]["功能按鈕"];
            AdminCatalog.Add(new RibbonFeature(Permissions.查詢紀錄, "查詢紀錄"));
            AdminCatalog.Add(new RibbonFeature(Permissions.動作設定, "動作設定"));
        }

        private static void InitMainPanel()
        {

           bool addPanel=true;
            foreach (NLDPanel Panel in MotherForm.Panels)
            {
                if (Panel.Group.Equals("學校"))
                {
                    MainPanel = Panel;
                    addPanel=false;
                }
            }

            if (addPanel)
            {
                MainPanel = new NLDPanel();
                MainPanel.Group = "學校";
            }
        }
    }
}