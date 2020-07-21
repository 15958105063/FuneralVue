using SqlSugar;

namespace Funeral.Core.Model.ViewModels
{
    public class AnswersDto
    {


        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        public int Traffic { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentNum { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 父级回答
        /// 作　　者:CY
        /// </summary>
        public int Pid { get; set; }


        /// <summary>
        /// 题目ID
        /// </summary>
        public int Qid { get; set; }
    }
}
