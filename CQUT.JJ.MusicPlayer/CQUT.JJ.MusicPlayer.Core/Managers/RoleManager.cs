using CQUT.JJ.MusicPlayer.Core.Managers.AuthorizationManager;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Enums;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Managers
{
    public class RoleManager : BaseManager<Role>
    {
        private readonly IPermissionManager _permissionManager;
        public RoleManager(JMDbContext ctx,IPermissionManager permissionManager) : base(ctx)
        {
            _permissionManager = permissionManager;
        }

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
                DeleteOriginalDefualtRole(model.IsDefault);
                JMDbContext.Role.Add(role);
                Save();

                _permissionManager.SetPermissions(AuthorizationObjectType.Role, role.Id, permissionCodes);
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
            role.LastModificationTime 
                = role.DeletionTime 
                = DateTime.Now;
            Save();
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Role Update(RoleModel model)
        {
            ValidateForUpdate(model);

            var role = Find(model.Id);
            DeleteOriginalDefualtRole(model.IsDefault);
            role.Name = model.Name;
            role.IsDefault = model.IsDefault;
            role.LastModificationTime = DateTime.Now;
            Save();

            return role;
        }

        /// <summary>
        /// 切换默认角色
        /// </summary>
        /// <param name="id"></param>
        public void ToggleSetDefault(int id)
        {
            var role = Find(id);
            if (!role.IsDefault)
                DeleteOriginalDefualtRole(true);
            role.IsDefault = !role.IsDefault;
            role.LastModificationTime = DateTime.Now;
            Save();
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
        /// 删除之前的默认角色
        /// </summary>
        /// <param name="isDefault"></param>
        private void DeleteOriginalDefualtRole(bool isDefault)
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
            ValidateForUpdate(model);
            var role = JMDbContext.Role.SingleOrDefault(r => r.Name == model.Name && !r.IsDeleted);
            if (role != null)
                ThrowException("角色名已存在！");
        }

        private void ValidateForUpdate(RoleModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
                ThrowException("角色名不能为空！");
            if (model.Name.Length > 8)
                ThrowException("角色名太长了，不能大于8个字符！");
        }
    }
}
