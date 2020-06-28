
using SqlSugar;
using System;

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 部门表
    /// </summary>
   public class AchVdr
    {

        public AchVdr()
        {
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
        }

        /// <summary>
        /// VdrId
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string VdrId { get; set; }

        /// <summary>
        /// VdrType
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string VdrType { get; set; }


        /// <summary> 
        ///VdrCode
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string VdrCode { get; set; }


        /// <summary> 
        ///VdrName
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string VdrName { get; set; }


        /// <summary> 
        ///VdrAddress
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string VdrAddress { get; set; }

        /// <summary> 
        ///VdrLinkman
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string VdrLinkman{ get; set; }

        /// <summary> 
        ///VdrLinkphone
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string VdrLinkphone{ get; set; }


        /// <summary> 
        ///VdrStatus
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string VdrStatus { get; set; }


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
