using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Managers;
using CQUT.JJ.MusicPlayer.Core.Managers.AuthorizationManager;
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
        private readonly IPermissionManager _permissionManager;

        public UserAppService(JMDbContext ctx,UserManager userManager,IPermissionManager permissionManager)
        {
            _ctx = ctx;
            _userManager = userManager;
            _permissionManager = permissionManager;
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
                        CreationTime = a.CreationTime
                    });
                });
                return models;
            }
            return null;
        }

        public IEnumerable<string> GetAllPermissionsByUserId(int userId)
        {
            return _permissionManager.GetAllPermissionsByUserId(userId);
        }

        public IEnumerable<UserModel> GetAllMembers()
        {
            var users = _userManager.FindAllMembers();
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
                        CreationTime = u.CreationTime
                    });
                });
                return models;
            }
            return null;
        }

        public UserModel GetMemberById(int id)
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

        public UserModel GetUserById(int id)
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

        public UserModel LoginOfMember(string userName, string password)
        {
            return Login(userName, password, false);
        }


        public UserModel LoginOfAdmin(string userName, string password)
        {
            return Login(userName, password, true);
        }

        private UserModel Login(string userName,string password,bool isAdmin)
        {
            password = password.EncryptByMD5();
            var user = _ctx.User.SingleOrDefault(m => m.UserName.Equals(userName) && m.Password.Equals(password) && !m.IsDeleted && m.IsAdmin == isAdmin);
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
                CreationTime = user.CreationTime,
            };
        }

        public void SetPermissions(int userId, string[] permissionCodes)
        {
            _permissionManager.SetPermissionsToUser(userId, permissionCodes);
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

        public IEnumerable<UserModel> GetMembersBySkiperAndTaker(int skiper, int taker)
        {
            return GetUsersBySkiperAndTaker(skiper, taker, false);
        }

        public IEnumerable<UserModel> GetAdminsBySkiperAndTaker(int skiper, int taker)
        {
            return GetUsersBySkiperAndTaker(skiper, taker, true);
        }

        private IEnumerable<UserModel> GetUsersBySkiperAndTaker(int skiper,int taker,bool isAdmin)
        {
            return _ctx.User.Where(u => u.IsAdmin == isAdmin && !u.IsDeleted)
                .Skip(skiper)
                .Take(taker)
                .Select(m => new UserModel()
                {
                    Id = m.Id,
                    UserName = m.UserName,
                    NickName = m.NickName,
                    CreationTime = m.CreationTime
                });
        }

        public bool IsSuperManager(int id,out UserModel userModel)
        {
            var isSuperManager = _userManager.IsSuperManager(id, out User user);
            if (user == null)
                userModel = null;
            else
            {
                userModel = new UserModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    NickName = user.NickName,
                    IsAdmin = user.IsAdmin,
                    CreationTime = user.CreationTime
                };
            }

            return isSuperManager;
        }
    }
}
