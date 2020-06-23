using Funeral.Core.IServices.BASE;
using Funeral.Core.Model;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;

namespace Funeral.Core.IServices
{
    public partial interface IGuestbookServices : IBaseServices<Guestbook>
    {
        Task<MessageModel<string>> TestTranInRepository();
        Task<bool> TestTranInRepositoryAOP();
    }
}
