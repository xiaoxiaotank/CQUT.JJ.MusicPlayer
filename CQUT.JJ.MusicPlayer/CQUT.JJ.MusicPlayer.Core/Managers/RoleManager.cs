using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Managers
{
    public class RoleManager : BaseManager<Role>
    {
        public RoleManager(JMDbContext ctx) : base(ctx) { }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="model"></param>
        /// <param name="permissionCodes"></param>
        /// <returns></returns>
        public Role Create(RoleModel model,string[] permissionCodes)
        {
            ValidateForCreate(model);
            Role role;
            using (var tran = JMDbContext.Database.BeginTransaction())
            {
                role = new Role()
                {
                    Id = model.Id,
                    Name = model.Name,
                    CreationTime = DateTime.Now,
                    IsDefault = model.IsDefault,
                    IsDeleted = false,
                };
                SetForDefualt(model.IsDefault);
                JMDbContext.Role.Add(role);
                Save();
                SetRolePermissions(role.Id, permissionCodes);
                tran.Commit();
            }
            Save();
            return role;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var role = base.Find(id);

            role.IsDeleted = true;
            var permissionCodes = JMDbContext.Permission.Where(m => m.RoleId == id);
            JMDbContext.Permission.RemoveRange(permissionCodes);
            Save();
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleName"></param>
        /// <param name="isDefault"></param>
        /// <param name="permissionCodes"></param>
        public Role Update(RoleModel model, string[] permissionCodes)
        {
            ValidateForUpdate(model);

            var role = Find(model.Id);

            var origPermissionCodes = JMDbContext.Permission.Where(m => m.RoleId == model.Id);
            JMDbContext.Permission.RemoveRange(origPermissionCodes);
            SetRolePermissions(model.Id, permissionCodes);
                
            role.Name = model.Name;
            role.IsDefault = model.IsDefault;
            role.LastModificationTime = DateTime.Now;
            SetForDefualt(model.IsDefault);           
            Save();

            return role;
        }

        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <returns></returns>
        public override IQueryable<Role> FindAll()
        {
            return base.FindAll().Where(r => !r.IsDeleted);
        }

        /// <summary>
        /// 通过id查询角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Role Find(int id)
        {
            var role = base.Find(id);
            if (role.IsDeleted)
                ThrowException("角色不存在！");
            return role;
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionCodes"></param>
        private void SetRolePermissions(int? roleId, string[] permissionCodes)
        {
            if (permissionCodes == null)
                return;
            var permissions = permissionCodes.Select(p => new EntityFramework.Models.Permission
            {
                RoleId = roleId,
                Code = p,
                CreationTime = DateTime.Now,
            });
            JMDbContext.Permission.AddRange(permissions);
        }

        /// <summary>
        /// 设置为默认角色
        /// </summary>
        /// <param name="isDefault"></param>
        private void SetForDefualt(bool isDefault)
        {
            if (isDefault)
            {
                var defaultRole = JMDbContext.Role.SingleOrDefault(r => r.IsDefault);
                if (defaultRole != null)
                    defaultRole.IsDefault = false;
            }          
        }

        private void ValidateForCreate(RoleModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                ThrowException("角色名不能为空！");
            if (model.Name.Length > 8)
                ThrowException("角色名太长了，不能大于8个字符！");
            var role = JMDbContext.Role.SingleOrDefault(r => r.Name == model.Name && !r.IsDeleted);
            if (role != null)
                ThrowException("角色名已存在！");
        }

        private void ValidateForUpdate(RoleModel model)
        {
            ValidateForCreate(model);
        }
    }
}
