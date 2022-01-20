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

namespace YusX.Entity.Domain
{
    /// <summary>
    /// 角色授权
    /// </summary>
    [Table("Sys_RoleAuth")]
    [Entity(TableCnName = "角色授权")]
    public class Sys_RoleAuth : BaseEntity
    {
        /// <summary>
        /// 授权ID
        /// </summary>
        [Key]
        [Display(Name = "授权ID")]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        public int AuthId { get; set; }

        /// <summary>
        /// 授权值
        /// </summary>
        [Display(Name = "授权值")]
        [MaxLength(10000)]
        [Column(TypeName = "nvarchar(10000)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Value { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        [Display(Name = "菜单ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int MenuId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [Display(Name = "角色ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int RoleId { get; set; }

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
