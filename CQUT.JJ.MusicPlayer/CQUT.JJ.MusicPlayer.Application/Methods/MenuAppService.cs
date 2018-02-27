using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Managers;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Application.Methods
{
    public class MenuAppService : IMenuAppService
    {
        private readonly MenuManager _menuManager;

        public MenuAppService(MenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        public MenuItemModel CreateMenuItem(MenuItemModel model)
        {
            var menuItem = _menuManager.Create(model);
            if (menuItem == null)
                throw new JMBasicException("创建菜单项失败!");

            return new MenuItemModel()
            {
                Id = menuItem.Id,
                Header = menuItem.Header,
                ParentId = menuItem.ParentId,
                Priority = menuItem.Priority,
                TargetUrl = menuItem.TargetUrl,
                RequiredAuthorizeCode = menuItem.RequiredAuthorizeCode
            };
        }

        public int DeleteMenuItem(int id)
        {
            return _menuManager.Remove(id);
        }

        public MenuItemModel GetMenuItemById(int id)
        {
            var menuItem = _menuManager.Find(id);

            if(menuItem != null)
            {
                return new MenuItemModel()
                {
                    Id = menuItem.Id,
                    Header = menuItem.Header,
                    ParentId = menuItem.ParentId,
                    Priority = menuItem.Priority,
                    TargetUrl = menuItem.TargetUrl,
                    RequiredAuthorizeCode = menuItem.RequiredAuthorizeCode
                };
            }

            return null;
        }

        public IEnumerable<MenuItemModel> GetMenus()
        {
            var menus = _menuManager.FindAll();            
            var menuItemList = new List<MenuItemModel>();
            foreach (var item in menus)
            {
                menuItemList.Add(new MenuItemModel()
                {
                    Id = item.Id,
                    Header = item.Header,
                    ParentId = item.ParentId,
                    Priority = item.Priority,
                    TargetUrl = item.TargetUrl,
                    RequiredAuthorizeCode = item.RequiredAuthorizeCode
                });
            }

            return menuItemList;
        }

        public void MigrateMenuItem(int id, int? parentId)
        {
            _menuManager.Migrate(id, parentId);
        }

        public MenuItemModel UpdateMenuItem(MenuItemModel model)
        {
            var menuItem = _menuManager.Update(model);
            if (menuItem == null)
                throw new JMBasicException("更新菜单项失败!");

            return new MenuItemModel()
            {
                Id = menuItem.Id,
                Header = menuItem.Header,
                ParentId = menuItem.ParentId,
                Priority = menuItem.Priority,
                TargetUrl = menuItem.TargetUrl,
                RequiredAuthorizeCode = menuItem.RequiredAuthorizeCode
            };
        }
    }
}
