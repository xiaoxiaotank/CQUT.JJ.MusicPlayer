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
        public const string Employee_Authorize = Employee + ".Authorize";
        public const string Employee_SetRole = Employee + ".SetRole";

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
        public const string Role_Authorize = Role + ".Authorize";
        public const string Role_SetDefault = Role + ".SetDefault";

        #endregion

        #region 菜单管理

        public const string MenuManage = App + ".MenuManage";
        public const string MenuManage_Create = MenuManage + ".Create";
        public const string MenuManage_Delete = MenuManage + ".Delete";
        public const string MenuManage_Rename = MenuManage + ".Rename";
        public const string MenuManage_Migrate = MenuManage + ".Migrate";
        public const string MenuManage_Update = MenuManage + ".Update";

        #endregion

        #region 歌唱家管理

        public const string Singer = App + ".Singer";
        public const string Singer_Create = Singer + ".Create";
        public const string Singer_Delete = Singer + ".Delete";
        public const string Singer_Update = Singer + ".Update";
        public const string Singer_Publish = Singer + ".Publish";
        public const string Singer_Unpublish = Singer + ".Unpublish";

        #endregion

        #region 专辑管理

        public const string Album = App + ".Album";
        public const string Album_Create = Album + ".Create";
        public const string Album_Delete = Album + ".Delete";
        public const string Album_Update = Album + ".Update";
        public const string Album_Publish = Album + ".Publish";
        public const string Album_Unpublish = Album + ".Unpublish";

        #endregion

        #region 音乐管理

        public const string Music = App + ".Music";
        public const string Music_Create = Music + ".Create";
        public const string Music_Delete = Music + ".Delete";
        public const string Music_Update = Music + ".Update";
        public const string Music_Publish = Music + ".Publish";
        public const string Music_Unpublish = Music + ".Unpublish";

        #endregion
    }
}
