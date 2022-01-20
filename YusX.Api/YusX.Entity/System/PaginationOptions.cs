using System;
using System.Collections.Generic;
using System.Text;

namespace YusX.Entity.System
{
    /// <summary>
    /// 分页选项
    /// </summary>
    public class PaginationOptions
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页数据量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 数据表名或类型名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 排序字段名
        /// </summary>
        public string SortField { get; set; }

        /// <summary>
        /// 排序方法
        /// </summary>
        public string SortMethod { get; set; }

        /// <summary>
        /// 搜索选项集合
        /// </summary>
        public List<SearchOption> SearchOptions { get; set; }

        /// <summary>
        /// 是否导出数据
        /// </summary>
        public bool IsExport { get; set; }

        /// <summary>
        /// 额外数据
        /// </summary>
        public object Value { get; set; }
    }

    /// <summary>
    /// 搜索选项
    /// </summary>
    public class SearchOption
    {
        /// <summary>
        /// 名称，一般是字段名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数值，用户输入的值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 类型，例如select、checkbox、like等
        /// </summary>
        public string Type { get; set; }
    }
}
