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
    /// 表格
    /// </summary>
    [Table("Sys_TableInfo")]
    [Entity(TableCnName = "表格", DetailTable = typeof(Sys_TableColumn), DetailTableName = "表格列")]
    public class Sys_TableInfo : BaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [Display(Name = "ID")]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        public int Id { get; set; }

        /// <summary>
        /// 表格名称
        /// </summary>
        [Display(Name = "表格名称")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
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
        /// 数据名称
        /// </summary>
        [Display(Name = "数据名称")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string TableName { get; set; }

        /// <summary>
        /// 表格别名
        /// </summary>
        [Display(Name = "表格别名")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string TableAlias { get; set; }

        /// <summary>
        /// 明细名称
        /// </summary>
        [Display(Name = "明细名称")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        public string DetailName { get; set; }

        /// <summary>
        /// 明细表名
        /// </summary>
        [Display(Name = "明细表名")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        public string DetailTable { get; set; }

        /// <summary>
        /// 视图文件夹
        /// </summary>
        [Display(Name = "视图文件夹")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string ViewFolder { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [Display(Name = "项目名称")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string ProjectName { get; set; }

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
        /// 排序字段
        /// </summary>
        [Display(Name = "排序字段")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string SortField { get; set; }

        /// <summary>
        /// 编辑字段
        /// </summary>
        [Display(Name = "排编辑字段")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string EditField { get; set; }

        /// <summary>
        /// 上传字段
        /// </summary>
        [Display(Name = "上传字段")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string UploadField { get; set; }

        /// <summary>
        /// 最大上传数
        /// </summary>
        [Display(Name = "最大上传数")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int UploadMaxCount { get; set; }

        /// <summary>
        /// 表格列
        /// </summary>
        [Display(Name = "表格列")]
        [ForeignKey("TableId")]
        public List<Sys_TableColumn> TableColumns { get; set; }
    }
}
