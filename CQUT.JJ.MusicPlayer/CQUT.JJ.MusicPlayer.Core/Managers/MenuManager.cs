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
        private const short Lowest_Prioriy = 99;

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

            var menu = new Menu()
            {
                Header = model.Header,
                ParentId = model.ParentId ?? 0,
                TargetUrl = model.TargetUrl,
                Priority = CreateLowestPriorityOfSiblings(model.ParentId ?? 0),
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

                using (var trans = JMDbContext.Database.BeginTransaction())
                {
                    menuItem.TargetUrl = model.TargetUrl;
                    menuItem.RequiredAuthorizeCode = model.RequiredAuthorizeCode;

                    if (menuItem.Priority != model.Priority)
                    {
                        ResettingSiblingPriority(menuItem.Id, menuItem.ParentId, model.Priority);
                        menuItem.Priority = model.Priority;
                    }

                    Save();
                    trans.Commit();
                }
            }
            else
                ThrowException("菜单项不存在！");

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
            var menuItem = Find(id);
            if (menuItem != null)
            {
                menuItem.ParentId = parentId ?? 0;
                menuItem.Priority = CreateLowestPriorityOfSiblings(parentId ?? 0);
            }
                
            else
                ThrowException("菜单项不存在！");
            Save();
        }

        /// <summary>
        /// 创建新的最低优先级
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private short CreateLowestPriorityOfSiblings(int parentId)
        {
            var lowestPriority = GetLowestPriorityOfSiblings(parentId);
            var newLowestPriority = (short)(lowestPriority + 1);
            return newLowestPriority > Lowest_Prioriy - 1 ? Lowest_Prioriy : newLowestPriority;
                
        }

        /// <summary>
        /// 获取兄弟结点最低优先级
        /// </summary>
        /// <param name="parentId"></param>
        private short GetLowestPriorityOfSiblings(int parentId)
        {
            var siblings = JMDbContext.Menu.Where(m => m.ParentId == parentId);
            return siblings.Any() ? siblings.Max(i => i.Priority) : (short)0;
        }

        /// <summary>
        /// 由于优先级发生变化，需要重新设置同级菜单项优先级参数
        /// </summary>
        /// <param name="priority"></param>
        private void ResettingSiblingPriority(int id, int parentId, short priority)
        {
            var siblings = JMDbContext.Menu.Where(m => m.ParentId == parentId && m.Priority >= priority && m.Id != id);
            if (siblings.Any(s => s.Priority == priority))
            {
                foreach (var sibling in siblings)
                {
                    sibling.Priority++;
                }
            }
            Save();
        }



        private void ValidateForUpdate(MenuItemModel model)
        {
            if (!(string.IsNullOrWhiteSpace(model.TargetUrl) || model.TargetUrl.StartsWith("/")))
                model.TargetUrl = model.TargetUrl.Insert(0, "/");
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
