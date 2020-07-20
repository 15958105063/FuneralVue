using SqlSugar;
using System;

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 字典类型
    /// 作　　者:CY
    /// </summary>
    public class DictionaryPermission : RootEntity
    {

        /// <summary>
        /// 自编码
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 256, IsNullable = true)]
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
        public string Name { get; set; }


        /// <summary>
        /// 值
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
        public string Value { get; set; }



        /// <summary>
        /// 上级Id
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
        public string Pid { get; set; }


        /// <summary>
        /// 备注
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
        public string Remark { get; set; }


        /// <summary>
        /// 排序
        /// 作　　者:CY
        /// </summary>
        public int OrderSort { get; set; }


        /// <summary>
        /// 是否激活
        /// 作　　者:CY
        /// </summary>
        public bool Enabled { get; set; }


        /// <summary>
        /// 创建ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? CreateId { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 修改ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? ModifyId { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ModifyBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? ModifyTime { get; set; } = DateTime.Now;


        /// <summary>
        /// 逻辑删除
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsDeleted { get; set; }


        /// <summary>
        /// 客户ID
        /// </summary>
        public int Tid { get; set; }
    }
}
