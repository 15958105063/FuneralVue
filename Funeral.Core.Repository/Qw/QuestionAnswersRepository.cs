using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// QuestionAnswersRepository
    /// </summary>
    public class QuestionAnswersRepository : BaseRepository<QuestionAnswers>, IQuestionAnswersRepository
    {
        public QuestionAnswersRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}