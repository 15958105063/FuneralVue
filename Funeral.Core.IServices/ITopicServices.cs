using Funeral.Core.IServices.BASE;
using Funeral.Core.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
    public interface ITopicServices : IBaseServices<Topic>
    {
        Task<List<Topic>> GetTopics();
    }
}
