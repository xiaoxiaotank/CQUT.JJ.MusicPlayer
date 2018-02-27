using CQUT.JJ.MusicPlayer.Application.Interfaces;
using CQUT.JJ.MusicPlayer.Core.Models;
using CQUT.JJ.MusicPlayer.MS.Areas.Admin.Models.Shared;
using CQUT.JJ.MusicPlayer.MS.Uitls.Extensions;
using CQUT.JJ.MusicPlayer.MS.Utils.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JJJ.CQLG.MIS.Web.Areas.Admin.Models.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly IMenuAppService _menuAppService;

        public MenuViewComponent(IMenuAppService menuAppService)
        {
            _menuAppService = menuAppService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keywords">搜索关键字</param>
        /// <returns></returns>
        public IViewComponentResult Invoke(string keywords = null)
        {
            var user = HttpContext.Session.GetCurrentUser();
            var menus = new List<MenuItemViewModel>();
            if (user != null)
                menus = GetMenuByUserId(user.Id, keywords);
            return View(menus);
        }

     
        public List<MenuItemViewModel> GetMenuByUserId(int userId, string keywords)
        {
            List<MenuItemViewModel> result = new List<MenuItemViewModel>();
            var menus = _menuAppService.GetMenus().MapToMenuTree();
            result = LoadMenu(menus, keywords);
            return result;
        }

        public List<MenuItemViewModel> LoadMenu(List<MenuItemViewModel> menus, string keywords)
        {
            var root = new MenuItemViewModel() { Children = menus };
            List<MenuItemViewModel> menuList = new List<MenuItemViewModel>();
            if (root.HasChild)
            {
                root.Children.ForEach(c =>
                {
                    var childrenList = LoadMenu(c.Children, keywords);
                    if (!string.IsNullOrWhiteSpace(keywords) && !childrenList.Any())
                    {
                        if (c.Header.Contains(keywords) && !string.IsNullOrWhiteSpace(c.TargetUrl))
                        {
                            c.Children = childrenList;
                            menuList.Add(c);
                        }
                    }
                    else
                    {
                        c.Children = childrenList;
                        menuList.Add(c);
                    }
                });
                return menuList;
            }
            return menuList;
        }
    }
}

