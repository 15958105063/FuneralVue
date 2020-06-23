﻿using SqlSugar;
using System;

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// Tibug 博文
    /// 作　　者:CY
    /// </summary>
    public class TopicDetail : RootEntity
    {
        public TopicDetail()
        {
            this.tdUpdatetime = DateTime.Now;
        }

        public int TopicId { get; set; }

        [SugarColumn(ColumnDataType ="nvarchar",Length = 200, IsNullable = true)]
        public string tdLogo { get; set; } 

        [SugarColumn(ColumnDataType ="nvarchar",Length = 200, IsNullable = true)]
        public string tdName { get; set; }

        [SugarColumn(ColumnDataType ="nvarchar",Length = int.MaxValue , IsNullable = true)]
        public string tdContent { get; set; }

        [SugarColumn(ColumnDataType ="nvarchar",Length = 400, IsNullable = true)]
        public string tdDetail { get; set; }

        [SugarColumn(ColumnDataType ="nvarchar",Length = 200, IsNullable = true)]
        public string tdSectendDetail { get; set; }

        public bool tdIsDelete { get; set; } = false;
        public int tdRead { get; set; }
        public int tdCommend { get; set; }
        public int tdGood { get; set; }
        public DateTime tdCreatetime { get; set; }
        public DateTime tdUpdatetime { get; set; }
        public int tdTop { get; set; }

        [SugarColumn(ColumnDataType ="nvarchar",Length = 200, IsNullable = true)]
        public string tdAuthor { get; set; }


        [SugarColumn(IsIgnore = true)]
        public virtual Topic Topic { get; set; }

    }
}