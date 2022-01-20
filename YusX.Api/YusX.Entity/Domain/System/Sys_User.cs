/*
 * Author：SakuraYuki
 * Contact：MyronMi@outlook.com
 * ----------
 * 1.此代码为自动生成，自行修改将会导致不可预知的问题
 * 2.数据库更新时需重新生成此代码文件
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YusX.Entity.Attributes;
using YusX.Entity.System;
using Newtonsoft.Json;

namespace YusX.Entity.Domain
{
    /// <summary>
    /// 用户
    /// </summary>
    [Table("Sys_User")]
    [Entity(TableCnName = "用户")]
    public class Sys_User : BaseEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key]
        [Display(Name = "用户ID")]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [MaxLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "昵称")]
        [MaxLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Nickname { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [Display(Name = "部门ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? DeptId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Display(Name = "部门名称")]
        [MaxLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Editable(true)]
        public string DeptName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        public byte Gender { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        public string Address { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Display(Name = "邮箱")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string Email { get; set; }

        /// <summary>
        /// 真实名称
        /// </summary>
        [Display(Name = "真实名称")]
        [MaxLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Editable(true)]
        public string RealName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "电话")]
        [MaxLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Editable(true)]
        public string Telephone { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        [MaxLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Editable(true)]
        public string Mobile { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [Display(Name = "传真")]
        [MaxLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Editable(true)]
        public string Fax { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        [Display(Name = "头像")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public bool Enable { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        [Display(Name = "排序号")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int SortNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        public string Remark { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [JsonIgnore]
        [Display(Name = "密码")]
        [MaxLength(32)]
        [Column(TypeName = "nvarchar(32)")]
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }

        /// <summary>
        /// 加盐值
        /// </summary>
        [JsonIgnore]
        [Display(Name = "加盐值")]
        [MaxLength(32)]
        [Column(TypeName = "nvarchar(32)")]
        [Required(AllowEmptyStrings = false)]
        public string Salt { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        [Display(Name = "审核状态")]
        [Column(TypeName = "tinyint")]
        public byte AuditStatus { get; set; }

        /// <summary>
        /// 审核人ID
        /// </summary>
        [Display(Name = "审核人ID")]
        [Column(TypeName = "int")]
        public int? AuditId { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        [Display(Name = "审核人")]
        [MaxLength(30)]
        [Column(TypeName = "nvarchar(30)")]
        public string Auditor { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Display(Name = "审核时间")]
        [Column(TypeName = "datetime")]
        public DateTime? AuditDate { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [Display(Name = "创建人ID")]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        public int CreateId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Display(Name = "创建人")]
        [MaxLength(30)]
        [Column(TypeName = "nvarchar(30)")]
        [Required(AllowEmptyStrings = false)]
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Column(TypeName = "datetime")]
        [Required(AllowEmptyStrings = false)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        [Display(Name = "修改人ID")]
        [Column(TypeName = "int")]
        public int? ModifyId { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [Display(Name = "修改人")]
        [MaxLength(30)]
        [Column(TypeName = "nvarchar(30)")]
        public string Modifier { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }
    }
}
