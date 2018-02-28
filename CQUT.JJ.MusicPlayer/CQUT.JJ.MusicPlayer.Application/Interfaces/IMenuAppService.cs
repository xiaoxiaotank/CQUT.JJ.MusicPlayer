using CQUT.JJ.MusicPlayer.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Application.Interfaces
{
    public interface IMenuAppService
    {
        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        IEnumerable<MenuItemModel> GetMenus();

        /// <summary>
        /// 根据id获取菜单项信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MenuItemModel GetMenuItemById(int id);

        /// <summary>
        /// 根据id获取其子菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<MenuItemModel> GetChildMenuItemsById(int id);

        /// <summary>
        /// 添加菜单项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        MenuItemModel CreateMenuItem(MenuItemModel model);

        /// <summary>
        /// 根据id删除菜单项信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteMenuItem(int id);

        /// <summary>
        /// 更新菜单项信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        MenuItemModel UpdateMenuItem(MenuItemModel model);

        /// <summary>
        /// 迁移菜单项信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentId"></param>
        void MigrateMenuItem(int id, int? parentId);
    }
}
