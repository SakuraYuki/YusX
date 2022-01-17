using System;

namespace YusX.Entity.Attributes
{
    /// <summary>
    /// 权限表特性
    /// </summary>
    public class PermissionTableAttribute : Attribute
    {
        /// <summary>
        /// 控制器操作的数据库表名，需要与菜单(Sys_Menu)中数据表名称(TableName)一致
        /// </summary>
        public string Name { get; set; }
    }
}
