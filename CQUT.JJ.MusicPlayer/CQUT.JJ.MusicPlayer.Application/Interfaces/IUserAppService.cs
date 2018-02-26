﻿using CQUT.JJ.MusicPlayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Application.Interfaces
{
    public interface IUserAppService
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        UserModel Login(string userName, string password);     

        /// <summary>
        /// 通过id获取用户信息
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        UserModel GetUserById(int id);

        /// <summary>
        /// 通过id获取管理员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel GetAdminById(int id);

        /// <summary>
        /// 通过id获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel GetUserOrAdminById(int id);

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserModel> GetAllCommonUsers();

        /// <summary>
        /// 获取所有管理员
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserModel> GetAllAdmins();

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model"></param>
        /// <param name="confirmPassword"></param>
        UserModel Register(UserModel model);

        /// <summary>
        /// 用户基本信息更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        UserModel UpdateBasicInfo(UserModel model);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="originalPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="confirmPassword">再次输入新密码</param>
        /// <returns></returns>
        UserModel UpdatePassword(int id, string originalPassword, string newPassword, string confirmPassword);

        /// <summary>
        /// 通过id删除用户
        /// </summary>
        /// <param name="id"></param>
        void DeleteUserById(int id);

        /// <summary>
        /// 通过用户编码获取所有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<string> GetAllPermissionsByUserId(int userId);

        /// <summary>
        /// 设置用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissions"></param>
        void SetPermissions(int userId, string[] permissionCodes);

        /// <summary>
        /// 通过用户id设置角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roleIds"></param>
        void SetRolesByUserId(int id, int[] roleIds);
    }
}