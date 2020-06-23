using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
    public class TopicDetailRepository : BaseRepository<TopicDetail>, ITopicDetailRepository
    {
        public TopicDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
