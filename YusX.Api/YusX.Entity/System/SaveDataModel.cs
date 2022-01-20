using System;
using System.Collections.Generic;
using System.Text;

namespace YusX.Entity.System
{
    /// <summary>
    /// 保存数据模型
    /// </summary>
    public class SaveDataModel
    {
        /// <summary>
        /// 主表数据
        /// </summary>
        public Dictionary<string, object> MainData { get; set; }

        /// <summary>
        /// 明细表数据
        /// </summary>
        public List<Dictionary<string, object>> DetailData { get; set; }

        /// <summary>
        /// 要删除的主键数据
        /// </summary>
        public List<object> DelKeys { get; set; }

        /// <summary>
        /// 额外扩展数据
        /// </summary>
        public object Extra { get; set; }
    }
}
