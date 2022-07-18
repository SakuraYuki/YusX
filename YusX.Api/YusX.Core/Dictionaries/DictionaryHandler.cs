using System.Collections.Generic;
using YusX.Core.Constracts;
using YusX.Core.Enums;
using YusX.Core.ManageUser;
using YusX.Core.UserManager;

namespace YusX.Core.Dictionaries
{
    public static class DictionaryHandler
    {
        /// <summary>
        /// 获取自定义数据源sql
        /// </summary>
        /// <param name="dicNo"></param>
        /// <param name="originalSql"></param>
        /// <returns></returns>
        public static string GetCustomDBSql(string dicNo, string originalSql)
        {
            switch (dicNo)
            {
                case "roles":
                    originalSql = GetRolesSql(originalSql);
                    break;
                //2020.05.24增加绑定table表时，获取所有的角色列表
                //注意，如果是2020.05.24之前获取的数据库脚本
                //请在菜单【下拉框绑定设置】添加一个字典编号【t_roles】,除了字典编号，其他内容随便填写
                case "t_roles":
                    originalSql = GetRolesSql();
                    break;
                default:
                    break;
            }
            return originalSql;
        }

        /// <summary>
        /// 2020.05.24增加绑定table表时，获取所有的角色列表
        /// </summary>
        /// <param name="context"></param>
        /// <param name="originalSql"></param>
        /// <returns></returns>
        public static string GetRolesSql()
        {
            if (DbInfo.Name == DbCurrentType.PgSql.ToString())
            {
                return "SELECT \"RoleId\" as key,\"RoleName\" as value FROM Sys_Role";
            }
            return $@"SELECT RoleId as 'key',RoleName as 'value' FROM Sys_Role WHERE Enable=1 ";
        }

        /// <summary>
        /// 获取解决的数据源，只能看到自己与下级所有角色
        /// </summary>
        /// <param name="context"></param>
        /// <param name="originalSql"></param>
        /// <returns></returns>
        public static string GetRolesSql(string originalSql)
        {
            if (UserContext.Current.IsSuperAdmin)
            {
                return originalSql;
            }
            int currnetRoleId = UserContext.Current.RoleId;
            List<int> roleIds = RoleContext.GetAllChildrenIds(currnetRoleId);
            roleIds.Add(currnetRoleId);
            string sql = $@"SELECT Role_Id as 'key',RoleName as 'value' FROM Sys_Role 
                           WHERE Enable=1  and Role_Id in ({string.Join(',', roleIds)})";
            return sql;
        }
    }
}
