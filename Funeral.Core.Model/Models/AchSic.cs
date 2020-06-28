
using SqlSugar;
using System;

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class AchSic
    {

        public AchSic()
        {
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
        }

        /// <summary>
        /// SicId
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string SicId { get; set; }

        /// <summary>
        /// SicOrgid
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string SicOrgid { get; set; }


        /// <summary> 
        ///SicName
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string SicName { get; set; }


        /// <summary> 
        ///SicStatus
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string SicStatus { get; set; }


        /// <summary> 
        ///SicNum
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string SicNum { get; set; }

        /// <summary> 
        ///SicRemark
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string SicRemark { get; set; }

        

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
