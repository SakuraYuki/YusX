using System;

namespace YusX.Core.DbAccessor
{
    /// <summary>
    /// 指定单独使用的数据连接信息
    /// </summary>
    public class DBConnectionAttribute : Attribute
    {
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DbName { get; set; }
    }
}
