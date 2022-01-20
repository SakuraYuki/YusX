using System;
using System.Collections.Generic;
using System.Text;

namespace YusX.Entity.System
{
    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T">数据对象类型</typeparam>
    public class PaginationResult<T>
    {
        /// <summary>
        /// 状态码，0成功
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 消息，成功或错误的具体消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 分页数据集
        /// </summary>
        public List<T> Rows { get; set; }

        /// <summary>
        /// 统计相关数据
        /// </summary>
        public object Summary { get; set; }

        /// <summary>
        /// 额外扩展数据
        /// </summary>
        public object Extra { get; set; }
    }

}
