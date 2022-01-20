using System;
using System.Collections.Generic;
using System.Text;

namespace YusX.Entity.System
{
    /// <summary>
    /// 权限信息
    /// </summary>
    public class Permissions
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// 父菜单ID
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 数据库表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 菜单授权
        /// </summary>
        public string MenuAuth { get; set; }

        /// <summary>
        /// 用户授权
        /// </summary>
        public string UserAuth { get; set; }

        /// <summary>
        /// 用户在当前菜单的全部权限值，如Add、Search等
        /// </summary>
        public string[] UserAuths { get; set; }
    }
}
