using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQUT.JJ.MusicPlayer.Core.Managers
{
    public class MenuManager : BaseManager<Menu>
    {
        public MenuManager(JMDbContext ctx) : base(ctx)
        {
        }

        /// <summary>
        /// 创建菜单项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Menu Create(MenuItemModel model)
        {
            ValidateForCreate(model);

            var siblings = JMDbContext.Menu.Where(m => m.ParentId == model.ParentId);
            var lowestPriority = siblings.Any() ? siblings.Max(i => i.Priority) : (short)0;

            var menu = new Menu()
            {
                Header = model.Header,
                ParentId = model.ParentId ?? 0,
                TargetUrl = model.TargetUrl,
                Priority = ++lowestPriority,
                RequiredAuthorizeCode = model.RequiredAuthorizeCode
            };
            return Create(menu);
        }       

        /// <summary>
        /// 更新菜单项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Menu Update(MenuItemModel model)
        {
            var menuItem = Find(model.Id);
            if (menuItem != null)
            {
                ValidateForUpdate(model);
                menuItem.Header = model.Header;
                menuItem.TargetUrl = model.TargetUrl;
                menuItem.RequiredAuthorizeCode = model.RequiredAuthorizeCode;               
                Save();
            }
            else
                ThrowException("栏目不存在！");

            return menuItem;
        }

        public int Delete(int id)
        {
            if (JMDbContext.Menu.Any(m => m.ParentId == id))
                ThrowException("该菜单下含有子菜单，请先删除子菜单！");
            return Remove(id);
        }

        public Menu Rename(int id,string header)
        {
            var menuItem = Find(id);
            if(menuItem != null)
            {
                ValidateForHeader(header);

                menuItem.Header = header;
                Save();
            }
            else
                ThrowException("栏目不存在！");
            return menuItem;
        }

        /// <summary>
        /// 迁移菜单项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parentId"></param>
        public void Migrate(int id, int? parentId)
        {
            var column = Find(id);
            if (column != null)
                column.ParentId = parentId ?? 0;
            else
                ThrowException("菜单项不存在！");
            Save();
        }


        private void ValidateForUpdate(MenuItemModel model)
        {
            ValidateForCreate(model);
            if (model.Priority < 1)
                ThrowException("菜单优先级最高为1");
            if (model.Priority > 99)
                ThrowException("菜单优先级最低为99");
        }

        private void ValidateForCreate(MenuItemModel model)
        {
            if (!(string.IsNullOrWhiteSpace(model.TargetUrl) || model.TargetUrl.StartsWith("/")))
                model.TargetUrl = model.TargetUrl.Insert(0, "/");
            ValidateForHeader(model.Header);
        }

        private void ValidateForHeader(string header)
        {
            if (string.IsNullOrWhiteSpace(header))
                ThrowException("菜单名不能为空！");
            if (header.Length < 2)
                ThrowException("菜单名长度至少为2个字符");
            if (header.Length > 8)
                ThrowException("菜单名长度不能超过8个字符");
        }
    }
}
