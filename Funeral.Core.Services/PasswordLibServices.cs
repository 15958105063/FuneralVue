using Funeral.Core.IRepository;
using Funeral.Core.IServices;
using Funeral.Core.Model.Models;
using Funeral.Core.Services.BASE;

namespace Funeral.Core.Services
{
    public partial class PasswordLibServices : BaseServices<PasswordLib>, IPasswordLibServices
    {
        IPasswordLibRepository _dal;
        public PasswordLibServices(IPasswordLibRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

    }
}
