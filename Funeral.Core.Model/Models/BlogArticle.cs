using SqlSugar;
using System;

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 博客文章
    /// 作　　者:CY
    /// </summary>
    public class BlogArticle
    {
        /// <summary>
        /// 主键
        /// 作　　者:CY
        /// </summary>
        /// 这里之所以没用RootEntity，是想保持和之前的数据库一致，主键是bID，不是Id
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int bID { get; set; }
        /// <summary>
        /// 创建人
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 600, IsNullable = true)]
        public string bsubmitter { get; set; }

        /// <summary>
        /// 标题blog
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 256, IsNullable = true)]
        public string btitle { get; set; }

        /// <summary>
        /// 类别
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
        public string bcategory { get; set; }

        /// <summary>
        /// 内容
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
        public string bcontent { get; set; }

        /// <summary>
        /// 访问量
        /// 作　　者:CY
        /// </summary>
        public int btraffic { get; set; }

        /// <summary>
        /// 评论数量
        /// 作　　者:CY
        /// </summary>
        public int bcommentNum { get; set; }

        /// <summary> 
        /// 修改时间
        /// 作　　者:CY
        /// </summary>
        public DateTime bUpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// 作　　者:CY
        /// </summary>
        public System.DateTime bCreateTime { get; set; }
        /// <summary>
        /// 备注
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = int.MaxValue, IsNullable = true)]
        public string bRemark { get; set; }

        /// <summary>
        /// 逻辑删除
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsDeleted { get; set; }

    }
}
