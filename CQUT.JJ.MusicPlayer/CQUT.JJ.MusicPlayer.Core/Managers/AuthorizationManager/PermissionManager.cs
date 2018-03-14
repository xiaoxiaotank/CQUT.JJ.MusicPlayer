using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Managers.AuthorizationManager
{
    public class PermissionManager : IPermissionManager
    {
        private readonly JMDbContext _ctx;
        public PermissionManager(JMDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<Permissioner> GetAllPermissions()
        {
            return PermissionProvider.PermissionList;
        }

        public IEnumerable<string> GetAllPermissionsByUserId(int userId)
        {
            var user = FindAdminUser(userId);
            IEnumerable<string> permissionCodes = null;
            if (user != null)
            {
                //获取该用户所扮演的所有角色
                var roleIdList = _ctx.Role
                    .Where(r => r.UserRole.Any(ur => ur.UserId == user.Id))
                    .Select(r => r.Id)
                    .ToList();
                //获取该用户所有权限
                permissionCodes = _ctx.Permission
                    .Where(p => p.UserId == userId || roleIdList.Contains(p.RoleId ?? 0))
                    .Select(p => p.Code)
                    .Distinct();
            }
            else
                ThrowException("用户不存在");
            return permissionCodes;
        }

        public IEnumerable<string> GetAllPermissionsByRoleId(int roleId)
        {
            return _ctx.Permission
                .Where(p => p.RoleId == roleId)
                .Select(p => p.Code);
        }

        public IEnumerable<string> GetPermissionsByUserId(int userId)
        {
            var user = FindAdminUser(userId);
            IEnumerable<string> permissionCodes = null;
            if (user != null)
                permissionCodes = _ctx.Permission
                    .Where(p => p.UserId == userId)
                    .Select(p => p.Code).Distinct();
            else
                ThrowException("用户不存在");
            return permissionCodes;
        }

        public bool IsGranted(int userId, string permissionCode)
        {
            var permissionCodes = GetAllPermissionsByUserId(userId);
            return permissionCodes.Contains(permissionCode);
        }

        public void SetPermissionsToUser(int userId, string[] permissionCodes)
        {
            var originalPermissions = _ctx.Permission.Where(p => p.UserId == userId);
            _ctx.Permission.RemoveRange(originalPermissions);

            var permissions = permissionCodes.Select(code => new Permission
            {
                Code = code,
                UserId = userId,
                CreationTime = DateTime.Now
            });
            if (permissions != null)
                _ctx.Permission.AddRange(permissions);
            _ctx.SaveChanges();
        }

        public void SetRolesToUser(int userId, int[] roleIds)
        {           
            var user = FindAdminUser(userId);
            if (user != null)
            {
                var orgiRolesOfUser = _ctx.UserRole.Where(ur => ur.UserId == user.Id);
                _ctx.UserRole.RemoveRange(orgiRolesOfUser);

                _ctx.Role.Where(r => roleIds.Contains(r.Id))
                    .ToList()
                    .ForEach(role => user.UserRole.Add(new UserRole()
                    {
                        Role = role,
                        User = user
                    })); 
            }
            else
                ThrowException("用户不存在");
            _ctx.SaveChanges();
        }

        private void ThrowException(string message)
        {
            throw new JMBasicException(message, HttpStatusCode.BadRequest);
        }

        private User FindAdminUser(int userId)
        {
            return _ctx.User.SingleOrDefault(u => u.Id == userId && u.IsAdmin && !u.IsDeleted);
        }
    }
}
