using System;

namespace YusX.Entity.Attributes
{
    /// <summary>
    /// 实体特性
    /// </summary>
    public class EntityAttribute : Attribute
    {
        /// <summary>
        /// 真实表名(数据库表名，若没有填写默认实体为表名)
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表中文名
        /// </summary>
        public string TableCnName { get; set; }

        /// <summary>
        /// 明细表类型
        /// </summary>
        public Type[] DetailTable { get; set; }

        /// <summary>
        /// 明细表名称
        /// </summary>
        public string DetailTableName { get; set; }
    }
}
