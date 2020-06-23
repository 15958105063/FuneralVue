using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Funeral.Core.Model.ViewModels
{
    /// <summary>
    /// 菜单路由和API权限视图模型
    /// </summary>
   public class RoleModulePermissionViewModel
    {


        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsDeleted { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// api ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? PermissionId { get; set; }



    }
}
