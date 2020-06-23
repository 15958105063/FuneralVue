using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 部门/客户
    /// 作　　者:CY
    /// </summary>
    public class Tenan
    {

        public Tenan() { }


        public Tenan(string name)
        {
            TenanName = name;
            Description = "";
            Enabled = true;
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;

        }


        /// <summary>
        /// 部门ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
        public string TenanName { get; set; }


        /// <summary>
        ///描述
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Enabled { get; set; }


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
        /// 修改者
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ModifyBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? ModifyTime { get; set; } = DateTime.Now;

    }
}
