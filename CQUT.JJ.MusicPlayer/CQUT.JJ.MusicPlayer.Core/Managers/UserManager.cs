using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Algorithms;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Managers
{
    public class UserManager : BaseManager<User>
    {
        private static readonly string _adminDefaultPassword = "123456";

        public UserManager(JMDbContext ctx) : base(ctx) { }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="model"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        public User Create(UserModel model)
        {
            ValidateForCreate(model);

            User user = null;
            var now = DateTime.Now;           
            using(var trans = JMDbContext.Database.BeginTransaction())
            {
                user = new User()
                {
                    UserName = model.UserName,
                    Password = model.Password.EncryptByMD5(),
                    IsAdmin = model.IsAdmin,
                    CreationTime = now,
                    LastModificationTime = now,
                    IsDeleted = false
                };
                user = base.Create(user);
                SetDefaultRole(user);
                trans.Commit();
            }

            return user;
        }

        /// <summary>
        /// 更新用户基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public User UpdateBasic(UserModel model)
        {
            var user = base.Find(model.Id);
            if (user != null && !user.IsDeleted)
            {
                ValidateForUpdate(model);

                user.NickName = model.NickName;
                user.UserName = model.UserName;
                user.LastModificationTime = DateTime.Now;
                Save();
            }
            else
                ThrowException("用户不存在！");

            return user;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="originalPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="confirmPassword"></param>
        /// <returns></returns>
        public User UpdatePassword(int id, string originalPassword, string newPassword, string confirmPassword)
        {
            ValidateForPassword(newPassword, confirmPassword);
            var user = base.Find(id);
            if (!user.IsDeleted)
            {
                if (user.Password.EncryptByMD5() == originalPassword)
                {
                    user.Password = newPassword.EncryptByMD5();
                    user.LastModificationTime = DateTime.Now;
                    Save();
                }
                else
                    ThrowException("密码不正确！");
            }
            else
                ThrowException("用户不存在！");
            return user;
        }

        /// <summary>
        /// 通过id查询用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override User Find(int id)
        {
            var user = base.Find(id);
            if (user.IsDeleted)
                ThrowException("用户不存在！");
            return user;
        }

        /// <summary>
        /// 查询所有用户
        /// </summary>
        /// <returns></returns>
        public IQueryable<User> FindAllCommonUsers()
        {
            return base.FindAll().Where(u => !u.IsDeleted && !u.IsAdmin);
        }

        /// <summary>
        /// 查询所有管理员
        /// </summary>
        /// <returns></returns>
        public IQueryable<User> FindAllAdmins()
        {
            return base.FindAll().Where(u => !u.IsDeleted && u.IsAdmin);
        }

        /// <summary>
        /// 根据id删除对象
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var user = base.Find(id);
            if (user != null && !user.IsDeleted)
            {
                user.IsDeleted = true;
                user.LastModificationTime = DateTime.Now;
                Save();
            }
            else
                ThrowException("用户不存在！");
        }

        /// <summary>
        /// 设置默认角色
        /// </summary>
        /// <param name="user"></param>
        private void SetDefaultRole(User user)
        {
            //var defaultRole = JMDbContext.Role.SingleOrDefault(r => r.IsDefault);
            //if (defaultRole != null)
            //{
            //    user.UserRole.Add(new UserRole
            //    {
            //        User = user,
            //        Role = defaultRole
            //    });
            //    Save();
            //}
        }

        /// <summary>
        /// 私有方法，验证数据有效性
        /// </summary>
        /// <param name="model"></param>
        /// <param name="confirmPassword"></param>
        private void ValidateForCreate(UserModel model)
        {
            var user = JMDbContext.User.SingleOrDefault(u => u.UserName == model.UserName && !u.IsDeleted);
            if (user != null)
                ThrowException("用户名已存在！");
            if (string.IsNullOrWhiteSpace(model.NickName))
                ThrowException("用户昵称不能为空！");
            if (model.NickName.Length > 8)
                ThrowException("用户昵称太长了！");
            if (string.IsNullOrWhiteSpace(model.UserName))
                ThrowException("用户名不能为空！");
            if (model.UserName.Length < 3)
                ThrowException("用户名太短了！");
            if (model.UserName.Length > 32)
                ThrowException("用户名太长了！");

            if(!model.IsAdmin)
                ValidateForPassword(model.Password, model.ConfirmPassword);
            else
                model.Password = _adminDefaultPassword;
        }

        private void ValidateForUpdate(UserModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName))
                ThrowException("用户名不能为空！");
            if (model.UserName.Length < 3)
                ThrowException("用户名太短了！");
            if (model.UserName.Length > 32)
                ThrowException("用户名太长了!");
        }

        private void ValidateForPassword(string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(password))
                ThrowException("密码不能为空！");
            if (!password.Equals(confirmPassword))
                ThrowException("两次密码输入不一致!");
            if (password.Length < 6)
                ThrowException("密码不能少于6位");
            if (password.Length > 32)
                ThrowException("密码太长了！");
        }

        /// <summary>
        /// 验证邮箱是否有效并且不重复
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        //private bool IsValidForEmailAddress(string emailAddress)
        //{
        //    if (string.IsNullOrWhiteSpace(emailAddress) && Regex.IsMatch(emailAddress, @"^([\w-]+\.)*?[\w-]+@[\w-]+\.([\w-]+\.)*?[\w]+$"))
        //        return !ctx.User.Any(u => u.EmailAddress.Equals(emailAddress) && !u.IsDeleted);
        //    return false;
        //}
    }
}
