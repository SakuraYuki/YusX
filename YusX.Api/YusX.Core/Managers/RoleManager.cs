using System;
using System.Collections.Generic;
using System.Linq;
using YusX.Core.DbAccessor;
using YusX.Core.Providers;
using YusX.Core.Providers.Cache;
using YusX.Core.Services;
using YusX.Entity.Domain;

namespace YusX.Core.Managers
{
    /// <summary>
    /// 角色管理器
    /// </summary>
    public static class RoleManager
    {
        public const string Key = "inernalRole";

        private static readonly object _locker = new object();

        private static string _roleVersion = string.Empty;

        private static List<RoleNodes> Roles { get; set; }

        public static List<RoleNodes> GetAllRoleId()
        {
            var cacheService = App.GetService<ICacheService>();
            //每次比较缓存是否更新过，如果更新则重新获取数据
            if (Roles != null && _roleVersion == cacheService.Get(Key))
            {
                return Roles;
            }
            lock (_locker)
            {
                if (_roleVersion != "" && Roles != null && _roleVersion == cacheService.Get(Key))
                {
                    return Roles;
                }

                Roles = DbProvider.DbContext
                    .Set<Sys_Role>()
                    .Where(x => x.Enable)
                    .Select(s => new RoleNodes()
                    {
                        Id = s.RoleId,
                        ParentId = s.ParentId,
                        RoleName = s.Name,
                    })
                    .ToList();

                string cacheVersion = cacheService.Get(Key);
                if (string.IsNullOrEmpty(cacheVersion))
                {
                    cacheVersion = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                    cacheService.Add(Key, cacheVersion);
                }
                else
                {
                    _roleVersion = cacheVersion;
                }
            }
            return Roles;
        }

        public static void Refresh()
        {
            App.GetService<ICacheService>().Remove(Key);
        }

        /// <summary>
        /// 
        /// 获取当前角色下的所有角色(不包括自己的角色)
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static List<RoleNodes> GetAllChildren(int roleId)
        {
            if (roleId <= 0)
            {
                return null;
            }

            var roles = GetAllRoleId();
            if (UserManager.IsRoleIdSuperAdmin(roleId))
            {
                return roles;
            }

            var completedRoles = new Dictionary<int, bool>();
            var rolesChildren = new List<RoleNodes>();
            var list = GetChildren(roles, rolesChildren, roleId, completedRoles);
            //2021.07.11增加无限递归异常数据移除当前节点
            if (list.Any(x => x.Id == roleId))
            {
                return list.Where(x => x.Id != roleId).ToList();
            }
            return list;
        }

        public static List<int> GetAllChildrenIds(int roleId)
        {
            return GetAllChildren(roleId)?.Select(x => x.Id)?.ToList();
        }

        /// <summary>
        /// 递归获取所有子节点权限
        /// </summary>
        /// <param name="roleId"></param>
        private static List<RoleNodes> GetChildren(List<RoleNodes> roles, List<RoleNodes> rolesChildren, int roleId, Dictionary<int, bool> completedRoles)
        {
            //2021.07.11修复不能获取三级以下角色的问题
            roles.ForEach(x =>
            {
                if (x.ParentId == roleId)
                {
                    if (completedRoles.TryGetValue(x.Id, out bool isWrite))
                    {
                        if (!isWrite)
                        {
                            roles.Where(x => x.Id == roleId).FirstOrDefault().ParentId = 0;
                            LogProvider.Error($"获取子角色异常RoleContext,角色id:{x.Id}");
                            Console.WriteLine($"获取子角色异常RoleContext,角色id:{x.Id}");
                            completedRoles[x.Id] = true;
                        }
                        return;
                    }
                    if (!rolesChildren.Any(c => c.Id == x.Id))
                    {
                        rolesChildren.Add(x);
                    }
                    completedRoles.Add(x.Id, false);
                    GetChildren(roles, rolesChildren, x.Id, completedRoles);
                }
            });
            return rolesChildren;
        }

        /// <summary>
        /// 获取当前角色下的所有用户
        /// </summary>
        /// <returns></returns>
        public static IQueryable<int> GetCurrentAllChildUser()
        {
            var roles = GetAllChildrenIds(UserManager.Current.RoleId);
            if (roles == null)
            {
                throw new Exception("未获取到当前角色");
            }
            return DbProvider.DbContext
                .Set<Sys_User>()
                .Where(u => roles.Contains(u.Role_Id)).Select(s => s.UserId);
        }
    }

    public class RoleNodes
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public string RoleName { get; set; }
    }
}
