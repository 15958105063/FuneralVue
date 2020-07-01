﻿using SqlSugar;
using System;

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 客户菜单关联表
    /// 作　　者:CY
    /// </summary>
    public class PermissionTenan:RootEntity
    {

        public PermissionTenan() { }


        public PermissionTenan(int pid, int tid)
        {
            PermissionId = pid;
            TenanId = tid;
            CreateTime = DateTime.Now;
            IsDeleted = false;
            CreateId = pid;
            CreateTime = DateTime.Now;
        }


        /// <summary>
        ///获取或设置是否禁用，逻辑上的删除，非物理删除
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsDeleted { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        public int TenanId { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int PermissionId { get; set; }
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
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 修改ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? ModifyId { get; set; }
        /// <summary>
        /// 修改者
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string ModifyBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? ModifyTime { get; set; }


    }
}
