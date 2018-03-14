using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Permissions
{
    public static class PermissionCodes
    {
        public const string Total = "Total";
        public const string App = Total + ".App";

        #region 员工

        public const string Employee = App + ".Employee";
        public const string Employee_Create = Employee + ".Create";
        public const string Employee_Delete = Employee + ".Delete";
        public const string Employee_Update = Employee + ".Update";

        #endregion

        #region 会员

        public const string Member = App + ".Member";
        public const string Member_Delete = Member + ".Delete";

        #endregion

        #region 角色

        public const string Role = App + ".Role";
        public const string Role_Create = Role + ".Create";
        public const string Role_Delete = Role + ".Delete";
        public const string Role_Update = Role + ".Update";

        #endregion

        #region 菜单管理

        public const string MenuManage = App + ".MenuManage";
        public const string MenuManage_Create = MenuManage + ".Create";
        public const string MenuManage_Delete = MenuManage + ".Delete";
        public const string MenuManage_Rename = MenuManage + ".Rename";
        public const string MenuManage_Migrate = MenuManage + ".Migrate";
        public const string MenuManage_Update = MenuManage + ".Update";

        #endregion
    }
}
