using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Permissions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Managers.AuthorizationManager
{
    public interface IPermissionManager
    {
        /// <summary>
        /// 判断用户是否有权限
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="permissionCode">权限码</param>
        /// <returns></returns>
        bool IsGranted(int userId, string permissionCode);

        /// <summary>
        /// 根据用户编号获取仅与该用户相关的权限
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        IEnumerable<string> GetPermissionsByUserId(int userId);

        /// <summary>
        /// 根据用户编号获取该用户所有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<string> GetAllPermissionsByUserId(int userId);

        /// <summary>
        /// 获取所有的权限
        /// </summary>
        /// <returns></returns>
        IEnumerable<Permissioner> GetAllPermissions();

        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="id"></param>
        /// <param name="permissionCodes"></param>
        void SetPermissions(AuthorizationObjectType objType, int id, string[] permissionCodes);

        /// <summary>
        /// 根据角色编号获取权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <returns></returns>
        IEnumerable<string> GetAllPermissionsByRoleId(int roleId);

        /// <summary>
        /// 给用户设置角色
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="roleId">角色编号</param>
        void SetRolesToUser(int userId, int[] roleIds);
    }
}
