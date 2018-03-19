using CQUT.JJ.MusicPlayer.EntityFramework.Persistences.Permissions;
using CQUT.JJ.MusicPlayer.MS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUT.JJ.MusicPlayer.MS.Uitls.Extensions
{
    public static class PermissionExtension
    {
        #region 映射权限树

        /// <summary>
        /// 映射权限树
        /// </summary>
        /// <param name="permissionCodes">权限码</param>
        /// <param name="fixedPermissionCodes">固定的权限码，不可操作</param>
        /// <returns></returns>
        public static IEnumerable<ZTreeNode> MapToPermissionTree(this IEnumerable<string> permissionCodes, IEnumerable<string> fixedPermissionCodes = null)
        {
            var permissionerList = GetAllPermissionerList();

            var permissionNodeList = permissionerList.Select(p => 
            {
                var node = new ZTreeNode
                {
                    Id = p.Code,
                    ParentId = p.ParentCode,
                    Name = p.DisplayName,
                };
                if (permissionCodes.Contains(p.Code))
                {
                    node.Checked = node.Open = true;
                }
                return node;
            });
            
            if(fixedPermissionCodes != null)
            {
                permissionNodeList = permissionNodeList.Select(p =>
                {
                    if (fixedPermissionCodes.Contains(p.Id))
                        p.CheckDisabled = true;
                    return p;
                });
            }            

            return permissionNodeList;
        }

        private static List<Permissioner> GetAllPermissionerList()
        {
            var list = PermissionProvider.PermissionList;
            var permissionerList = new List<Permissioner>();
            foreach (var item in list)
            {
                var index = item.Code.LastIndexOf('.');
                var parentCode = index > 0 ? item.Code.Substring(0, index) : string.Empty;

                permissionerList.Add(new Permissioner
                {
                    ParentCode = parentCode,
                    Code = item.Code,
                    DisplayName = item.DisplayName
                });
            }

            return permissionerList;
        }
        #endregion


       
    }
}
