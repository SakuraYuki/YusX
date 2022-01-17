using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YusX.Entity.System
{
    /// <summary>
    /// 系统动作
    /// </summary>
    public class SysAction
    {
        /// <summary>
        /// 动作ID
        /// </summary>
        public int ActionId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// 动作名称
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 动作值
        /// </summary>
        public string Value { get; set; }
    }
}
