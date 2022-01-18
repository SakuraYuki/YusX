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

namespace YusX.Entity.Domain
{
    /// <summary>
    /// 字典
    /// </summary>
    [Table("Sys_Dictionary")]
    [Entity(TableCnName = "字典", DetailTable = typeof(Sys_DictionaryList), DetailTableName = "字典明细")]
    public partial class Sys_Dictionary : BaseEntity
    {
        /// <summary>
        /// 字典ID
        /// </summary>
        [Key]
        [Display(Name = "字典ID")]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        public int DictID { get; set; }

        /// <summary>
        /// 字典编号
        /// </summary>
        [Display(Name = "字典编号")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string DictNo { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        [Display(Name = "字典名称")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        [Display(Name = "父级ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int ParentId { get; set; }

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
        [MaxLength(4000)]
        [Column(TypeName = "nvarchar(4000)")]
        [Editable(true)]
        public string Remark { get; set; }

        /// <summary>
        ///sql语句
        /// </summary>
        [Display(Name = "sql语句")]
        [MaxLength(10000)]
        [Column(TypeName = "nvarchar(10000)")]
        [Editable(true)]
        public string DbSql { get; set; }

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

        /// <summary>
        /// 字典明细
        /// </summary>
        [Display(Name = "字典明细")]
        [ForeignKey("DictID")]
        public List<Sys_DictionaryList> Sys_DictionaryList { get; set; }
    }
}