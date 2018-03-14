using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Permissions
{
    public static class PermissionProvider
    {
        public static readonly List<Permissioner> PermissionList = new List<Permissioner>
        {
            new Permissioner{ Code = PermissionCodes.Total, DisplayName = "全部权限" },
            new Permissioner{ Code = PermissionCodes.App, DisplayName = "后台操作权限"},

            #region 员工

            new Permissioner{ Code = PermissionCodes.Employee, DisplayName = "员工"},
            new Permissioner{ Code = PermissionCodes.Employee_Create, DisplayName = "添加员工"},
            new Permissioner{ Code = PermissionCodes.Employee_Delete, DisplayName = "删除员工"},
            new Permissioner{ Code = PermissionCodes.Employee_Update, DisplayName = "编辑员工"},

            #endregion

            #region 用户

            new Permissioner{ Code = PermissionCodes.Member, DisplayName = "会员"},
            new Permissioner{ Code = PermissionCodes.Member_Delete, DisplayName = "删除会员"},

            #endregion
            #region 角色

            new Permissioner{ Code = PermissionCodes.Role, DisplayName = "角色"},
            new Permissioner{ Code = PermissionCodes.Role_Create, DisplayName = "添加角色"},
            new Permissioner{ Code = PermissionCodes.Role_Delete, DisplayName = "删除角色"},
            new Permissioner{ Code = PermissionCodes.Role_Update, DisplayName = "编辑角色"},

            #endregion

            #region 组织结构

		    new Permissioner{ Code = PermissionCodes.MenuManage,DisplayName = "菜单管理"},
            new Permissioner{ Code = PermissionCodes.MenuManage_Create, DisplayName = "添加菜单"},
            new Permissioner{ Code = PermissionCodes.MenuManage_Delete, DisplayName = "删除菜单"},
            new Permissioner{ Code = PermissionCodes.MenuManage_Update, DisplayName = "编辑菜单"},
            new Permissioner{ Code = PermissionCodes.MenuManage_Migrate, DisplayName = "迁移菜单"},
            new Permissioner{ Code = PermissionCodes.MenuManage_Rename, DisplayName = "重命名菜单"},

	        #endregion
        };
    }
}
