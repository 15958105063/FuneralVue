using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// QuestionsRepository
    /// </summary>
    public class QuestionsRepository : BaseRepository<Questions>, IQuestionsRepository
    {
        public QuestionsRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}