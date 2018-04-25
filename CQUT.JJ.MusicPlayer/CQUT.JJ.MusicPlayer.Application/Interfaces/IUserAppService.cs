using CQUT.JJ.MusicPlayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Application.Interfaces
{
    public interface IUserAppService
    {
        /// <summary>
        /// 会员登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        UserModel LoginOfMember(string userName, string password);

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        UserModel LoginOfAdmin(string userName, string password);

        /// <summary>
        /// 通过id获取会员信息
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        UserModel GetMemberById(int id);

        /// <summary>
        /// 通过id获取管理员信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel GetAdminById(int id);

        /// <summary>
        /// 通过id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel GetUserById(int id);

        /// <summary>
        /// 获取所有会员
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserModel> GetAllMembers();

        /// <summary>
        /// 获取所有管理员
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserModel> GetAllAdmins();

        /// <summary>
        /// 获取特定会员
        /// </summary>
        /// <param name="skiper"></param>
        /// <param name="taker"></param>
        /// <returns></returns>
        IEnumerable<UserModel> GetMembersBySkiperAndTaker(int skiper,int taker);

        /// <summary>
        /// 获取特定管理员
        /// </summary>
        /// <param name="skiper"></param>
        /// <param name="taker"></param>
        /// <returns></returns>
        IEnumerable<UserModel> GetAdminsBySkiperAndTaker(int skiper,int taker);

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsSuperManager(int id, out UserModel userModel);

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

        /// <summary>
        /// 获取会员数
        /// </summary>
        /// <returns></returns>
        int GetMemberCount();

        /// <summary>
        /// 获取今日会员注册数
        /// </summary>
        /// <returns></returns>
        int GetTodayRegisterMemberCount();

        /// <summary>
        /// 获取员工数
        /// </summary>
        /// <returns></returns>
        int GetEmployeeCount();

        /// <summary>
        /// 获取今日员工创建数
        /// </summary>
        /// <returns></returns>
        int GetTodyCreateEmployeeCount();
    }
}
