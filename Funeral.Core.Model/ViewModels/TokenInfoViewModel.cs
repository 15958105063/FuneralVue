namespace Funeral.Core.Model.ViewModels
{
    public  class TokenInfoViewModel
    {
        /// <summary>
        /// 返回标识
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int uid { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int rid { get; set; }


        /// <summary>
        /// 客户ID
        /// </summary>
        public int tid { get; set; }


        /// <summary>
        /// token
        /// </summary>
        public string token { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public double expires_in { get; set; }
        /// <summary>
        /// token类型
        /// </summary>
        public string token_type { get; set; }
    }
}
