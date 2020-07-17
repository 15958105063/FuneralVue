
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class AchUsr: RootEntity
    {

        public AchUsr()
        {
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
        }

        /// <summary>
        /// UsrId
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string UsrId { get; set; }

        /// <summary>
        /// UsrOrgid
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string UsrOrgid { get; set; }


        /// <summary> 
        ///UsrDptid
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string UsrDptid { get; set; }


        /// <summary> 
        ///UsrLoginname
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string UsrLoginname { get; set; }


        /// <summary> 
        ///UsrPasswd
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string UsrPasswd { get; set; }

        /// <summary> 
        ///UsrName
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string UsrName { get; set; }

        /// <summary> 
        ///UsrTel
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string UsrTel { get; set; }

        /// <summary> 
        ///UsrPhone
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string UsrPhone { get; set; }

        /// <summary> 
        ///UsrStatus
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string UsrStatus { get; set; }


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


        [SugarColumn(IsIgnore = true)]
        public List<string> RIDs { get; set; }


    }
}
