
using SqlSugar;
using System;

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 按钮表
    /// </summary>
   public class AchBtn: RootEntity
    {

        public AchBtn()
        {
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
        }

        /// <summary>
        /// FunId
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string BtnId { get; set; }

        /// <summary>
        /// FunFupid
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string FunFupid { get; set; }


        /// <summary> 
        ///FunValue
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string FunValue { get; set; }


        /// <summary> 
        ///FunName
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string FunName { get; set; }


        /// <summary> 
        ///FgpValue
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string FunShowname { get; set; }

        /// <summary> 
        ///FunUrl
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string FunUrl { get; set; }

        /// <summary> 
        ///FunImageurl
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string FunImageurl { get; set; }

        /// <summary> 
        ///FunNum
        /// </summary> 
        [SugarColumn(ColumnDataType = "int", IsNullable = true)]
        public int? FunNum { get; set; }

        /// <summary> 
        ///FunController
        /// </summary> 
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string FunController { get; set; }


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
