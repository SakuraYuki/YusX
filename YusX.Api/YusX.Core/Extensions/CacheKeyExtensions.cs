using YusX.Core.Enums;

namespace YusX.Core.Extensions
{
    public static class CacheKeyExtensions
    {
        public static string GetKey(this CachePrefix prefix, object value)
        {
            return prefix.ToString() + value;
        }

        public static string GetUserIdKey(this int userId)
        {
            return CachePrefix.UID.ToString() + userId;
        }

        public static string GetRoleIdKey(this int roleId)
        {
            return CachePrefix.Role.ToString() + roleId;
        }
    }
}
