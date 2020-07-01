
using SqlSugar;
using System;

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 列表管理
    /// </summary>
   public class AchDli: RootEntity
    {

        public AchDli()
        {
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
        }

        /// <summary>
        /// DliId
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string DliId { get; set; }

        /// <summary>
        /// DliOrgid
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DliOrgid { get; set; }


        /// <summary> 
        ///DliProname
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DliProname { get; set; }


        /// <summary> 
        ///DliProtype
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DliProtype { get; set; }


        /// <summary> 
        ///DliFunvalue
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DliFunvalue { get; set; }

        /// <summary> 
        ///DLIValue
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DLIValue { get; set; }

        /// <summary> 
        ///DliName
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DliName { get; set; }

        /// <summary> 
        ///DliNum
        /// </summary> 
        [SugarColumn(ColumnDataType = "int", IsNullable = true)]
        public int? DliNum { get; set; }

        /// <summary> 
        ///DliType
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DliType { get; set; }

        /// <summary> 
        ///DliSort
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DliSort { get; set; }


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
        /// 客户ID
        /// </summary>
        public int Tid { get; set; }

    }
}
