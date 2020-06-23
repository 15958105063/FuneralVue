
using Funeral.Core.IRepository;
using Funeral.Core.IRepository.UnitOfWork;
using Funeral.Core.Model.Models;
using Funeral.Core.Repository.Base;

namespace Funeral.Core.Repository
{
	/// <summary>
	/// TasksQzRepository
	/// </summary>
    public class TasksQzRepository : BaseRepository<TasksQz>, ITasksQzRepository
    {
        public TasksQzRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
                    