using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Managers;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Algorithms;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Application.Methods
{
    public class UserAppService : IUserAppService
    {
        private readonly JMDbContext _ctx;
        private readonly UserManager _userManager;

        public UserAppService(JMDbContext ctx,UserManager userManager)
        {
            _ctx = ctx;
            _userManager = userManager;
        }


        public void DeleteUserById(int id)
        {
            _userManager.Delete(id);
        }

        public UserModel GetAdminById(int id)
        {
            var admin = _ctx.User.SingleOrDefault(u => u.Id == id && u.IsAdmin && !u.IsDeleted);
            if (admin != null)
            {
                return new UserModel()
                {
                    Id = admin.Id,
                    UserName = admin.UserName,
                    NickName = admin.NickName,
                    IsAdmin = admin.IsAdmin,
                };                
            }
            else
                return null;
        }

        public IEnumerable<UserModel> GetAllAdmins()
        {
            var admins = _userManager.FindAllAdmins();
            if (admins != null)
            {
                var models = new List<UserModel>();
                admins.ToList().ForEach(a =>
                {
                    models.Add(new UserModel()
                    {
                        Id = a.Id,
                        UserName = a.UserName,
                        NickName = a.NickName,
                        IsAdmin = a.IsAdmin,
                    });
                });
                return models;
            }
            return null;
        }

        public IEnumerable<string> GetAllPermissionsByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserModel> GetAllCommonUsers()
        {
            var users = _userManager.FindAllCommonUsers();
            if (users != null)
            {
                var models = new List<UserModel>();
                users.ToList().ForEach(u =>
                {
                    models.Add(new UserModel()
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        NickName = u.NickName,
                    });
                });
                return models;
            }
            return null;
        }

        public UserModel GetUserById(int id)
        {
            var user = _ctx.User.SingleOrDefault(u => u.Id == id && !u.IsAdmin && !u.IsDeleted);
            if(user != null)
            {
                return new UserModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    NickName = user.NickName,
                    IsAdmin = user.IsAdmin,
                };
            }
            return null;
        }

        public UserModel GetUserOrAdminById(int id)
        {
            var user = _userManager.Find(id);
            if (user != null)
            {
                return new UserModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    NickName = user.NickName,
                    IsAdmin = user.IsAdmin,
                };
            }
            return null;
        }

        public UserModel Login(string userName, string password)
        {
            password = password.EncryptByMD5();
            var user = _ctx.User.SingleOrDefault(m => m.UserName.Equals(userName) && m.Password.Equals(password) && !m.IsDeleted);
            if (user == null)
                _userManager.ThrowException("用户名或密码错误！");
            return new UserModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                NickName = user.NickName,
                IsAdmin = user.IsAdmin,
            };
        }

        public UserModel Register(UserModel model)
        {
            var user =_userManager.Create(model);
            if (user == null)
                throw new JMBasicException("注册失败!");
            return new UserModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                NickName = user.NickName,
                IsAdmin = user.IsAdmin,
            };
        }

        public void SetPermissions(int userId, string[] permissionCodes)
        {
            throw new NotImplementedException();
        }

        public void SetRolesByUserId(int id, int[] roleIds)
        {
            throw new NotImplementedException();
        }

        public UserModel UpdateBasicInfo(UserModel model)
        {
            var user = _userManager.UpdateBasic(model);
            if(user != null)
            {
                return new UserModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    NickName = user.NickName,
                };
            }
            return null;
        }

        public UserModel UpdatePassword(int id, string originalPassword, string newPassword, string confirmPassword)
        {
            var user = _userManager.UpdatePassword(id,originalPassword,newPassword,confirmPassword);
            if (user != null)
            {
                return new UserModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    NickName = user.NickName,
                };
            }
            return null;
        }
    }
}
