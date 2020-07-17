
using SqlSugar;
using System;

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 机构表
    /// </summary>
   public class AchOrg: RootEntity
    {

        public AchOrg()
        {
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
        }


        /// <summary>
        /// 机构编号
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string OrgId { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string OrgName { get; set; }


        /// <summary> 
        ///机构简称
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string OrgShortName { get; set; }


        /// <summary> 
        ///联系电话
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 11, IsNullable = true)]
        public string OrgTel { get; set; }


        /// <summary> 
        ///主要负责人
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string OrgPsn { get; set; }

        /// <summary> 
        ///行政区划编号
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string OrgDtrCode { get; set; }

        /// <summary> 
        ///所属客户类型
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string OrgGroup { get; set; }

        /// <summary> 
        ///数据级别
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string OrgOrigin { get; set; }

        /// <summary> 
        ///序号
        /// </summary> 
        [SugarColumn(ColumnDataType = "int", IsNullable = true)]
        public int? OrgNum { get; set; }


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
