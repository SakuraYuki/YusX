using YusX.Core.Enums;

namespace YusX.Core.Extensions
{
    /// <summary>
    /// 缓存键扩展
    /// </summary>
    public static class CacheKeyExtensions
    {
        /// <summary>
        /// 获取缓存前缀的对应缓存键
        /// </summary>
        /// <param name="prefix">缓存前缀</param>
        /// <param name="value">缓存键</param>
        /// <returns></returns>
        public static string GetKey(this CachePrefix prefix, object value)
        {
            return $"{prefix}_{value}";
        }

        /// <summary>
        /// 获取指定用户ID的缓存键
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public static string GetUserIdKey(this int userId)
        {
            return GetKey(CachePrefix.UID, userId);
        }

        /// <summary>
        /// 获取指定角色ID的缓存键
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public static string GetRoleIdKey(this int roleId)
        {
            return GetKey(CachePrefix.Role, roleId);
        }
    }
}
