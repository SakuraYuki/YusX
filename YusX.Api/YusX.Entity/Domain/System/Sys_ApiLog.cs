/*
 * Author：SakuraYuki
 * Contact：MyronMi@outlook.com
 * ----------
 * 1.此代码为自动生成，自行修改将会导致不可预知的问题
 * 2.数据库更新时需重新生成此代码文件
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YusX.Entity.Attributes;
using YusX.Entity.System;

namespace VOL.Entity.DomainModels
{
    /// <summary>
    /// 接口日志
    /// </summary>
    [Table("Sys_ApiLog")]
    [EntityAttribute(TableCnName = "接口日志")]
    public class Sys_ApiLog : BaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [Display(Name = "Id")]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        public int Id { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        [Display(Name = "请求地址")]
        [MaxLength(10000)]
        [Column(TypeName = "varchar(10000)")]
        public string Url { get; set; }

        /// <summary>
        /// 请求内容
        /// </summary>
        [Display(Name = "请求内容")]
        [MaxLength(10000)]
        [Column(TypeName = "nvarchar(10000)")]
        public string Request { get; set; }

        /// <summary>
        /// 响应内容
        /// </summary>
        [Display(Name = "响应内容")]
        [MaxLength(10000)]
        [Column(TypeName = "nvarchar(10000)")]
        public string Response { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "类型")]
        [Column(TypeName = "int")]
        public int Type { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Display(Name = "开始时间")]
        [Column(TypeName = "datetime")]
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Display(Name = "结束时间")]
        [Column(TypeName = "datetime")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 时长
        /// </summary>
        [Display(Name = "时长")]
        [Column(TypeName = "int")]
        public int? ElapsedTime { get; set; }

        /// <summary>
        /// 响应状态
        /// </summary>
        [Display(Name = "响应状态")]
        [Column(TypeName = "int")]
        public int? ResponseStatus { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [Display(Name = "异常信息")]
        [MaxLength(10000)]
        [Column(TypeName = "nvarchar(10000)")]
        public string ExceptionInfo { get; set; }

        /// <summary>
        /// 用户代理
        /// </summary>
        [Display(Name = "用户代理")]
        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        public string UserAgent { get; set; }

        /// <summary>
        /// 用户IP
        /// </summary>
        [Display(Name = "用户IP")]
        [MaxLength(100)]
        [Column(TypeName = "varchar(39)")]
        public string UserAddress { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        [Column(TypeName = "int")]
        public int? UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [Display(Name = "用户名称")]
        [MaxLength(30)]
        [Column(TypeName = "nvarchar(30)")]
        public string UserName { get; set; }
    }
}
