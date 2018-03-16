using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Managers;
using CQUT.JJ.MusicPlayer.Core.Managers.AuthorizationManager;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Application.Methods
{
    public class RoleAppService : IRoleAppService
    {
       private readonly JMDbContext _ctx;
        private readonly RoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;

        public RoleAppService(JMDbContext ctx, RoleManager roleManager, IPermissionManager permissionManager)
        {
            _ctx = ctx;
            _roleManager = roleManager;
            _permissionManager = permissionManager;
        }

        public RoleModel CreateRole(RoleModel model, string[] permissionCodes)
        {
            var role = _roleManager.Create(model, permissionCodes);
            return new RoleModel()
            {
                Id = role.Id,
                Name = role.Name,
                IsDefault = role.IsDefault,
                CreationTime = role.CreationTime,
                LastModificationTime = role.LastModificationTime
            };
        }

        public void DeleteRoleById(int id)
        {
            _roleManager.Delete(id);
        }

        public IEnumerable<string> GetAllPermissionsByRoleId(int roleId)
        {
            return _permissionManager.GetAllPermissionsByRoleId(roleId);
        }

        public IEnumerable<RoleModel> GetAllRoles()
        {
            return _roleManager.FindAll()
                .Select(r => new RoleModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsDefault = r.IsDefault,
                    CreationTime = r.CreationTime,
                    LastModificationTime = r.LastModificationTime
                });
        }

        public IEnumerable<RoleModel> GetAllRolesByUserId(int userId)
        {
            var user = _ctx.User.SingleOrDefault(u => u.Id == userId && !u.IsDeleted);
            if (user == null)
                _roleManager.ThrowException("用户不存在");

            var result = _ctx.UserRole
                .Where(ur => ur.User.Id == user.Id && !ur.Role.IsDeleted)?
                .Select(m => new RoleModel()
                {
                    Id = m.Role.Id,
                    Name = m.Role.Name,
                    IsDefault = m.Role.IsDefault,
                    CreationTime = m.Role.CreationTime,
                    LastModificationTime = m.Role.LastModificationTime
                });               
            return result;
        }

        public RoleModel GetRoleById(int id)
        {
            var role = _roleManager.Find(id);
            return new RoleModel()
            {
                Id = role.Id,
                Name = role.Name,
                IsDefault = role.IsDefault,
                CreationTime = role.CreationTime,
                LastModificationTime = role.LastModificationTime
            };
        }

        public RoleModel UpdateRole(RoleModel model)
        {
            var role = _roleManager.Update(model);
            return new RoleModel()
            {
                Id = role.Id,
                Name = role.Name,
                IsDefault = role.IsDefault,
                CreationTime = role.CreationTime,
                LastModificationTime = role.LastModificationTime
            };
        }

        public void SetPermissions(int id, string[] permissionCodes)
        {
            _permissionManager.SetPermissions(AuthorizationObjectType.Role, id, permissionCodes);
        }

        public void ToggleSetDefault(int id)
        {
            _roleManager.ToggleSetDefault(id);
        }
    }
}
