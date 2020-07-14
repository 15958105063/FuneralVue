using SqlSugar;

namespace Funeral.Core.Model.ViewModels
{
    public class AchPermissionInputViewModels
    {

        public int TId { get; set; }
        public int Id { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string AchId { get; set; }
        /// <summary>
        /// 机构编号
        /// </summary>
        public string OrgId { get; set; }

        /// <summary>
        /// 菜单路由地址
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否是按钮
        /// </summary>
        public bool IsButton { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public string Func { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProName { get; set; }

        /// <summary>
        /// 产品类型
        /// </summary>
        public string ProType { get; set; }

        /// <summary>
        /// 所属客户类型
        /// </summary>
        public string Pruid { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        public string Imageurl { get; set; }
        /// <summary>
        /// 排序名
        /// </summary>

        public int? Num { get; set; }

        /// <summary>
        /// 展示名称
        /// </summary>
        public string ShowName { get; set; }

        
        /// <summary>
        /// 上一级菜单（0表示上一级无菜单）
        /// </summary>
        public string Pid { get; set; }


    }
}
