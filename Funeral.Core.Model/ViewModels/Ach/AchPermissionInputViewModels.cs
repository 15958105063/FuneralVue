using SqlSugar;

namespace Funeral.Core.Model.ViewModels
{
    public class AchPermissionInputViewModels
    {

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
        /// 菜单显示名（如用户页、编辑(按钮)、删除(按钮)）
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string Name { get; set; }
        /// <summary>
        /// 是否是按钮
        /// </summary>
        public bool IsButton { get; set; } = false;

        /// <summary>
        /// ProName
        /// </summary>
        public string ProName { get; set; }

        /// <summary>
        /// FgpProtype
        /// </summary>
        public string FgpProtype { get; set; }

        /// <summary>
        /// FgpPruid
        /// </summary>
        public string FgpPruid { get; set; }

        /// <summary>
        /// FgpValue
        /// </summary>
        public string FgpValue { get; set; }


        /// <summary>
        /// FgpImageurl
        /// </summary>
        public string FgpImageurl { get; set; }
        /// <summary>
        /// FgpNum
        /// </summary>

        public int? FgpNum { get; set; }


        /// <summary>
        /// 上一级菜单（0表示上一级无菜单）
        /// </summary>
        public string Pid { get; set; }


    }
}
