using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    /// <summary>
    /// AnswersRepository
    /// </summary>
    public class AnswersRepository : BaseRepository<Answers>, IAnswersRepository
    {
        public AnswersRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}