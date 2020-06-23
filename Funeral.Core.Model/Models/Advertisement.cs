using SqlSugar;
using System;

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 
    /// 作　　者:CY
    /// </summary>
    public class Advertisement : RootEntity
    {

        /// <summary>
        /// 广告图片
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(Length = 512, IsNullable = true, ColumnDataType = "nvarchar")]
        public string ImgUrl { get; set; }

        /// <summary>
        /// 广告标题
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(Length = 64, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Title { get; set; }

        /// <summary>
        /// 广告链接
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(Length = 256, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Url { get; set; }

        /// <summary>
        /// 备注
        /// 作　　者:CY
        /// </summary>
        [SugarColumn(Length = int.MaxValue, IsNullable = true, ColumnDataType = "nvarchar")]
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// 作　　者:CY
        /// </summary>
        public DateTime Createdate { get; set; } = DateTime.Now;
    }
}
