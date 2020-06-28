
using SqlSugar;
using System;

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 部门表
    /// </summary>
   public class AchDpt
    {

        public AchDpt()
        {
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
        }

        /// <summary>
        /// DptId
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public string DptId { get; set; }

        /// <summary>
        /// DptNAame
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DptNAame { get; set; }


        /// <summary> 
        ///DptNo
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DptNo { get; set; }


        /// <summary> 
        ///DptOrgid
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DptOrgid { get; set; }


        /// <summary> 
        ///DptTel
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DptTel { get; set; }

        /// <summary> 
        ///DptPsn
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DptPsn { get; set; }

        /// <summary> 
        ///DptStatus
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DptStatus { get; set; }

        /// <summary> 
        ///DptNum
        /// </summary> 
        [SugarColumn(ColumnDataType = "int", IsNullable = true)]
        public int? DptNum { get; set; }

        /// <summary> 
        ///序号
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string DptDtrcode { get; set; }


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
