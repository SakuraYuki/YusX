using System;
using System.Collections.Generic;
using System.Text;

namespace YusX.Entity.System
{
    /// <summary>
    /// 表格列信息
    /// </summary>
    public class TableColumnInfo
    {
        /// <summary>
        /// 精度，例如12,0
        /// </summary>
        public string PreciseScale { get; set; }

        /// <summary>
        /// 类型，例如varchar、int等
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
    }
}
