using CQUT.JJ.MusicPlayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Application.Interfaces
{
    public interface IRoleAppService
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="model"></param>
        /// <param name="permissionNames"></param>
        RoleModel CreateRole(RoleModel model, string[] permissionCodes);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        void DeleteRoleById(int id);

        /// <summary>
        /// 通过角色Id获取角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RoleModel GetRoleById(int id);

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="model"></param>
        RoleModel UpdateRole(RoleModel model);

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        IEnumerable<RoleModel> GetAllRoles();

        /// <summary>
        /// 通过角色id获取权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        IEnumerable<string> GetAllPermissionsByRoleId(int roleId);

        /// <summary>
        /// 通过用户id获取角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<RoleModel> GetAllRolesByUserId(int userId);

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="permissions"></param>
        void SetPermissions(int id, string[] permissionCodes);

        /// <summary>
        /// 切换默认角色
        /// </summary>
        /// <param name="id"></param>
        void ToggleSetDefault(int id);
    }
}
