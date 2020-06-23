using SqlSugar;

namespace Funeral.Core.Model
{
    /// <summary>
    /// 公共实体ID
    /// 作　　者:CY
    /// </summary>
    public class RootEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

      
    }
}