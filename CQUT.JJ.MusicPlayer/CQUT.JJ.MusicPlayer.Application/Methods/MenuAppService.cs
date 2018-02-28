using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Managers;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Exceptions;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Application.Methods
{
    public class MenuAppService : IMenuAppService
    {
        private readonly JMDbContext _ctx;
        private readonly MenuManager _menuManager;

        public MenuAppService(JMDbContext ctx,MenuManager menuManager)
        {
            _ctx = ctx;
            _menuManager = menuManager;
        }

        public MenuItemModel CreateMenuItem(MenuItemModel model)
        {
            var menuItem = _menuManager.Create(model);
            if (menuItem == null)
                throw new JMBasicException("创建菜单项失败!");

            return new MenuItemModel()
            {
                Header = menuItem.Header,
                ParentId = menuItem.ParentId,
                TargetUrl = menuItem.TargetUrl,
                RequiredAuthorizeCode = menuItem.RequiredAuthorizeCode
            };
        }

        public int DeleteMenuItem(int id)
        {
            return _menuManager.Remove(id);
        }

        public IEnumerable<MenuItemModel> GetChildMenuItemsById(int id)
        {
            return _ctx.Menu.Where(m => m.ParentId == id)?
                .Select(i => new MenuItemModel()
                {
                    Id = i.Id,
                    Header = i.Header,
                    ParentId = i.ParentId,
                    Priority = i.Priority,
                    TargetUrl = i.TargetUrl,
                    RequiredAuthorizeCode = i.RequiredAuthorizeCode
                });
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
            return _menuManager.FindAll()?
                .Select(i => new MenuItemModel()
                {
                    Id = i.Id,
                    Header = i.Header,
                    ParentId = i.ParentId,
                    Priority = i.Priority,
                    TargetUrl = i.TargetUrl,
                    RequiredAuthorizeCode = i.RequiredAuthorizeCode
                });                       
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
