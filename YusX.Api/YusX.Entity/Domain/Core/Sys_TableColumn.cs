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
    /// 表格列
    /// </summary>
    [Table("Sys_TableColumn")]
    [Entity(TableCnName = "表格列")]
    public partial class Sys_TableColumn : BaseEntity
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
        /// 表格ID
        /// <summary>
        [Display(Name = "表格ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int TableId { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [Display(Name = "显示名称")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        /// <summary>
        /// 数据字段
        /// </summary>
        [Display(Name = "数据字段")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Field { get; set; }

        /// <summary>
        /// 数据字段
        /// </summary>
        [Display(Name = "数据字段")]
        [MaxLength(10000)]
        [Column(TypeName = "nvarchar(10000)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string DataType { get; set; }

        /// <summary>
        /// 显示宽度
        /// </summary>
        [Display(Name = "显示宽度")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int Width { get; set; }

        /// <summary>
        /// 显示尺寸
        /// </summary>
        [Display(Name = "显示尺寸")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? ColSize { get; set; }

        /// <summary>
        /// 选择器数据源
        /// </summary>
        [Display(Name = "选择器数据源")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string DataSource { get; set; }

        /// <summary>
        /// 是否数据列
        /// </summary>
        [Display(Name = "是否数据列")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public bool IsDataColumn { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [Display(Name = "是否显示")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public bool IsShow { get; set; }

        /// <summary>
        /// 是否显示为图片
        /// </summary>
        [Display(Name = "是否显示为图片")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public bool IsImage { get; set; }

        /// <summary>
        /// 是否为主键
        /// </summary>
        [Display(Name = "是否为主键")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public bool IsKey { get; set; }

        /// <summary>
        /// 是否只读
        /// </summary>
        [Display(Name = "是否只读")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public bool IsReadonly { get; set; }

        /// <summary>
        /// 是否可空
        /// </summary>
        [Display(Name = "是否可空")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public bool Nullable { get; set; }

        /// <summary>
        /// 是否可排序
        /// </summary>
        [Display(Name = "是否可排序")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public bool Sortable { get; set; }

        /// <summary>
        /// 编辑列号
        /// </summary>
        [Display(Name = "编辑列号")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? EditColNo { get; set; }

        /// <summary>
        /// 编辑行号
        /// </summary>
        [Display(Name = "编辑行号")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? EditRowNo { get; set; }

        /// <summary>
        /// 编辑类型
        /// </summary>
        [Display(Name = "编辑类型")]
        [MaxLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Editable(true)]
        public string EditType { get; set; }

        /// <summary>
        /// 搜索列号
        /// </summary>
        [Display(Name = "搜索列号")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? SearchColNo { get; set; }

        /// <summary>
        /// 搜索行号
        /// </summary>
        [Display(Name = "搜索行号")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? SearchRowNo { get; set; }

        /// <summary>
        /// 搜索类型
        /// </summary>
        [Display(Name = "搜索类型")]
        [MaxLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        [Editable(true)]
        public string SearchType { get; set; }

        /// <summary>
        /// 数据最大长度
        /// </summary>
        [Display(Name = "数据最大长度")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? MaxLength { get; set; }

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
