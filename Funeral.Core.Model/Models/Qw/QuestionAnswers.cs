

namespace Funeral.Core.Model.Models
{
    /// <summary>
    /// 问题答案关联表
    /// 作　　者:CY
    /// </summary>
    public class QuestionAnswers
    {
        public QuestionAnswers() { }

        public QuestionAnswers(int qid, int aid)
        {
            QId = qid;
            AId = aid;
        }

        /// <summary>
        /// 问题ID
        /// </summary>
        public int QId { get; set; }
        /// <summary>
        /// 答案ID
        /// </summary>
        public int AId { get; set; }
        

    }
}
