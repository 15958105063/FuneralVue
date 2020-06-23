using Funeral.Core.IServices;
using Funeral.Core.FrameWork.IRepository;
using Funeral.Core.Services.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;
using System.Linq;
using Funeral.Core.Common;
using Funeral.Core;

namespace Funeral.Core.Services
{
    /// <summary>
    /// RoleTenanServices
    /// </summary>	
    public class PermissionTenanServices : BaseServices<PermissionTenan>, IPermissionTenanServices
    {
	
        IPermissionTenanRepository _dal;
        public PermissionTenanServices(IPermissionTenanRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }


        /// <summary>
        /// 保存用户菜单关联信息
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        public async Task<PermissionTenan> SavePermissionTenan(int pid, int tid)
        {
            PermissionTenan roleTenan = new PermissionTenan(pid, tid);

            PermissionTenan model = new PermissionTenan();
            var tenanList = await base.Query(a => a.PermissionId == roleTenan.PermissionId && a.TenanId == roleTenan.TenanId);
            if (tenanList.Count > 0)
            {
                model = tenanList.FirstOrDefault();
            }
            else
            {
                var id = await base.Add(roleTenan);
                model = await base.QueryById(id);
            }

            return model;

        }

        [Caching(AbsoluteExpiration = 30)]
        public async Task<int> GetTenanIdByPid(int pid)
        {
            return ((await base.Query(d => d.PermissionId == pid)).OrderByDescending(d => d.TenanId).LastOrDefault()?.PermissionId).ObjToInt();
        }
    }
}
