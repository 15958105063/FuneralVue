﻿using SqlSugar;
using System;

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 回复
    /// 作　　者:CY
    /// </summary>
    public class Answers : RootEntity
    {

        /// <summary>
        /// 标题
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 256, IsNullable = true)]
        public string Title { get; set; }

        /// <summary>
        /// 类别
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
        public string Category { get; set; }

        /// <summary>
        /// 内容
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
        public string Content { get; set; }

        /// <summary>
        /// 访问量
        /// 作　　者:CY
        /// </summary>
        public int Traffic { get; set; }

        /// <summary>
        /// 评论数量
        /// 作　　者:CY
        /// </summary>
        public int CommentNum { get; set; }


        /// <summary>
        /// 备注
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
        public string Remark { get; set; }


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

    }
}
