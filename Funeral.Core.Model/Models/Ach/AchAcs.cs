

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 系统用户角色表
    /// </summary>
    public class AchAcs
    {
        public AchAcs() { }
        public AchAcs(string uid, string rid,int tid)
        {
            AcsUsrid = uid;
            AcsRolid = rid;
            Tid = tid;
        }

        /// <summary>
        /// AcsUsrid
        /// </summary>
        public string AcsUsrid { get; set; }

        /// <summary>
        /// AcsRolid
        /// </summary>
        public string AcsRolid { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public int Tid { get; set; }
    }
}
