using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using YusX.Core.DbAccessor;
using YusX.Core.Enums;
using YusX.Core.Extensions;
using YusX.Core.Providers;
using YusX.Core.Providers.Cache;
using YusX.Entity.Domain;
using YusX.Entity.System;

namespace YusX.Core.Managers
{
    /// <summary>
    /// 用户管理器
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// 角色权限的版本号
        /// </summary>
        private static readonly Dictionary<int, string> rolePermissionsVersion = new Dictionary<int, string>();

        /// <summary>
        /// 每个角色ID对应的菜单权限（已做静态化处理）
        /// 每次获取权限时用当前服务器的版本号与redis/memory缓存的版本比较,如果不同会重新刷新缓存
        /// </summary>
        private static readonly Dictionary<int, List<Permissions>> rolePermissions = new Dictionary<int, List<Permissions>>();

        /// <summary>
        /// 获取角色权限时通过安全字典锁定的角色id
        /// </summary>
        private static ConcurrentDictionary<string, object> objKeyValue = new ConcurrentDictionary<string, object>();

        private static ICacheService CacheService => App.GetService<ICacheService>();

        /// <summary>
        /// 为了尽量减少redis或Memory读取,保证执行效率,将UserContext注入到DI，
        /// 每个UserContext的属性至多读取一次redis或Memory缓存从而提高查询效率
        /// </summary>
        public static UserManager Current => App.GetService<UserManager>();

        private UserInfo _userInfo;
        public UserInfo UserInfo
        {
            get
            {
                if (_userInfo != null)
                {
                    return _userInfo;
                }
                return GetUserInfo(UserId);
            }
        }

        /// <summary>
        /// 角色ID为1的默认为超级管理员
        /// </summary>
        public bool IsSuperAdmin => IsRoleIdSuperAdmin(RoleId);

        /// <summary>
        /// 角色ID为1的默认为超级管理员
        /// </summary>
        public static bool IsRoleIdSuperAdmin(int roleId) => roleId == 1;

        public UserInfo GetUserInfo(int userId)
        {
            if (_userInfo != null) return _userInfo;
            if (userId <= 0)
            {
                _userInfo = new UserInfo();
                return _userInfo;
            }
            string key = userId.GetUserIdKey();
            _userInfo = CacheService.Get<UserInfo>(key);
            if (_userInfo != null && _userInfo.UserId > 0) return _userInfo;

            _userInfo = DbProvider.DbContext.Set<Sys_User>()
                .Where(x => x.UserId == userId).Select(s => new UserInfo()
                {
                    UserId = userId,
                    Roles = new Dictionary<int, string>(),
                    Token = s.Token,
                    Username = s.Username,
                    RealName = s.RealName,
                    Enable = s.Enable
                }).FirstOrDefault();

            if (_userInfo != null && _userInfo.UserId > 0)
            {
                CacheService.AddObject(key, _userInfo);
            }
            return _userInfo ?? new UserInfo();
        }

        /// <summary>
        /// 获取用户所有的菜单权限
        /// </summary>
        public List<Permissions> Permissions
        {
            get
            {
                return GetPermissions(RoleId);
            }
        }

        /// <summary>
        /// 菜单按钮变更时，同时刷新权限缓存2022.05.23
        /// </summary>
        /// <param name="menuId"></param>
        public void RefreshWithMenuActionChange(int menuId)
        {
            foreach (var roleId in rolePermissions.Where(c => c.Value.Any(x => x.MenuId == menuId)).Select(s => s.Key))
            {
                if (rolePermissionsVersion.ContainsKey(roleId))
                {
                    CacheService.Add(roleId.GetRoleIdKey(), DateTime.Now.ToString("yyyyMMddHHMMssfff"));
                }
            }

        }

        /// <summary>
        /// 获取单个表的权限
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Permissions GetPermissions(string tableName)
        {
            return GetPermissions(RoleId).Where(x => x.TableName == tableName).FirstOrDefault();
        }

        /// <summary>
        /// 2022.03.26
        /// 菜单类型1:移动端，0:PC端
        /// </summary>
        public static int MenuType => App.HttpContext.Request.Headers.ContainsKey("uapp") ? 1 : 0;

        /// <summary>
        /// 自定条件查询权限
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Permissions GetPermissions(Func<Permissions, bool> func)
        {
            // 2022.03.26增移动端加菜单类型判断
            return GetPermissions(RoleId).Where(func).Where(x => x.MenuType == MenuType).FirstOrDefault();
        }

        private List<Permissions> ActionToArray(List<Permissions> permissions)
        {
            permissions.ForEach(x =>
            {
                try
                {
                    var menuAuthArr = x.MenuAuth.DeserializeObject<List<MenuAction>>();
                    x.UserAuths = string.IsNullOrEmpty(x.UserAuth)
                    ? new string[0]
                    : x.UserAuth.Split(",").Where(c => menuAuthArr.Any(m => m.Value == c)).ToArray();

                }
                catch { }
                finally
                {
                    if (x.UserAuths == null)
                    {
                        x.UserAuths = new string[0];
                    }
                }
            });
            return permissions;
        }

        private List<Permissions> MenuActionToArray(List<Permissions> permissions)
        {
            permissions.ForEach(x =>
            {
                try
                {
                    x.UserAuths = string.IsNullOrEmpty(x.UserAuth)
                    ? new string[0]
                    : x.UserAuth.DeserializeObject<List<MenuAction>>().Select(s => s.Value).ToArray();
                }
                catch { }
                finally
                {
                    if (x.UserAuths == null)
                    {
                        x.UserAuths = new string[0];
                    }
                }
            });
            return permissions;
        }

        public List<Permissions> GetPermissions(int roleId)
        {
            if (IsRoleIdSuperAdmin(roleId))
            {
                //2020.12.27增加菜单界面上不显示，但可以分配权限
                var permissions = DbProvider.DbContext.Set<Sys_Menu>()
                    .Where(x => x.Enable)
                    .Select(a => new Permissions
                    {
                        MenuId = a.MenuId,
                        ParentId = a.ParentId,
                        //2020.05.06增加默认将表名转换成小写，权限验证时不再转换
                        TableName = (a.TableName ?? "").ToLower(),
                        //MenuAuth = a.Auth,
                        UserAuth = a.Auth,
                        // 2022.03.26增移动端加菜单类型
                        MenuType = a.MenuType
                    }).ToList();
                return MenuActionToArray(permissions);
            }
            ICacheService cacheService = CacheService;
            string roleKey = roleId.GetRoleIdKey();

            //角色有缓存，并且当前服务器的角色版本号与redis/memory缓存角色的版本号相同直接返回静态对象角色权限
            string currnetVeriosn = "";
            if (rolePermissionsVersion.TryGetValue(roleId, out currnetVeriosn)
                && currnetVeriosn == cacheService.Get(roleKey))
            {
                return rolePermissions.ContainsKey(roleId) ? rolePermissions[roleId] : new List<Permissions>();
            }

            //锁定每个角色，通过安全字典减少锁粒度，否则多个同时角色获取缓存会导致阻塞
            object objId = objKeyValue.GetOrAdd(roleId.ToString(), new object());
            //锁定每个角色
            lock (objId)
            {
                if (rolePermissionsVersion.TryGetValue(roleId, out currnetVeriosn)
                    && currnetVeriosn == cacheService.Get(roleKey))
                {
                    return rolePermissions.ContainsKey(roleId) ? rolePermissions[roleId] : new List<Permissions>();
                }

                //没有redis/memory缓存角色的版本号或与当前服务器的角色版本号不同时，刷新缓存
                var dbContext = DbProvider.DbContext;
                List<Permissions> _permissions = (from a in dbContext.Set<Sys_Menu>()
                                                  join b in dbContext.Set<Sys_RoleAuth>()
                                                  on a.MenuId equals b.MenuId
                                                  where b.RoleId == roleId //&& a.ParentId > 0
                                                  && b.Value != ""
                                                  orderby a.ParentId
                                                  select new Permissions
                                                  {
                                                      MenuId = a.MenuId,
                                                      ParentId = a.ParentId,
                                                      //2020.05.06增加默认将表名转换成小写，权限验证时不再转换
                                                      TableName = (a.TableName ?? "").ToLower(),
                                                      MenuAuth = a.Auth,
                                                      UserAuth = b.Value ?? "",
                                                      // 2022.03.26增移动端加菜单类型
                                                      MenuType = a.MenuType
                                                  }).ToList();
                ActionToArray(_permissions);
                string _version = cacheService.Get(roleKey);
                //生成一个唯一版本号标识
                if (_version == null)
                {
                    _version = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                    //将版本号写入缓存
                    cacheService.Add(roleKey, _version);
                }
                //刷新当前服务器角色的权限
                rolePermissions[roleId] = _permissions;

                //写入当前服务器的角色最新版本号
                rolePermissionsVersion[roleId] = _version;
                return _permissions;
            }

        }

        /// <summary>
        /// 判断是否有权限
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="authName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool ExistsPermissions(string tableName, string authName, int roleId = 0)
        {
            if (roleId <= 0) roleId = RoleId;
            tableName = tableName.ToLower();
            authName = authName.ToLower();
            return GetPermissions(roleId).Any(x => x.TableName == tableName && x.UserAuths.Contains(authName));
        }

        /// <summary>
        /// 判断是否有权限
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="authName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool ExistsPermissions(string tableName, ActionPermissionOptions actionPermission, int roleId = 0)
        {
            return ExistsPermissions(tableName, actionPermission.ToString(), roleId);
        }

        public int UserId => (App.User?.FindFirstValue(JwtRegisteredClaimNames.Jti) ?? App.User?.FindFirstValue(ClaimTypes.NameIdentifier)).GetInt();

        public string Username => UserInfo.Username;

        public string UserRealName => UserInfo.RealName;

        public string Token => UserInfo.Token;

        public int RoleId => UserInfo.Roles?.FirstOrDefault().Key ?? 0;

        /// <summary>
        /// 注销指定用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        public void Logout(int userId)
            => CacheService.Remove(userId.GetUserIdKey());
    }
}
