using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.MS.Entities;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Uitls.Extensions
{
    public static class MenuExtension
    {
        #region 映射菜单树
        public static List<MenuItemViewModel> MapToMenuTree(this IEnumerable<MenuItemModel> menuItem)
        {
            var roots = menuItem.Where(m => m.ParentId == 0)
                .OrderBy(n => n.Priority)
                .Select(c => new MenuItemViewModel
                {
                    Id = c.Id,
                    Header = c.Header,
                    TargetUrl = c.TargetUrl,
                    Priority = c.Priority,
                    RequiredAuthorizeCode = c.RequiredAuthorizeCode
                }).ToList();
            foreach (var r in roots)
            {
                r.Children.AddRange(GetChildren(r, menuItem));
            }
            return roots;
        }

        public static List<MenuItemViewModel> GetChildren(MenuItemViewModel root, IEnumerable<MenuItemModel> menuItem)
        {
            var children = menuItem.Where(m => m.ParentId == root.Id)
                .OrderBy(n => n.Priority)
                .Select(c => new MenuItemViewModel
                {
                    Id = c.Id,
                    Header = c.Header,
                    TargetUrl = c.TargetUrl,
                    Priority = c.Priority,
                    RequiredAuthorizeCode = c.RequiredAuthorizeCode
                }).ToList();
            if (children.Any())
            {
                foreach (var c in children)
                {
                    c.Children.AddRange(GetChildren(c, menuItem));
                }
            }
            return children;
        }
        #endregion

        #region 映射菜单到ZTree
        public static List<ZTreeNode> MapMenuToZTree(this IEnumerable<MenuItemModel> list)
        {
            var menuTreeList = new List<ZTreeNode>();
            var groupList = list.GroupBy(n => n.ParentId ?? 0);
            foreach (var items in groupList)
            {
                foreach (var menuItem in items.OrderBy(i => i.Priority))
                {
                    menuTreeList.Add(new ZTreeNode()
                    {
                        Id = menuItem.Id.ToString(),
                        ParentId = (menuItem.ParentId ?? 0).ToString(),
                        Name = menuItem.Header
                    });

                }
            }

            return menuTreeList;
        }
        #endregion
    }
}
