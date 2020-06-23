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
    public class RoleTenanServices : BaseServices<RoleTenan>, IRoleTenanServices
    {
	
        IRoleTenanRepository _dal;
        public RoleTenanServices(IRoleTenanRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rid"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        public async Task<RoleTenan> SaveRoleTenan(int rid, int tid)
        {
            RoleTenan roleTenan = new RoleTenan(rid, tid);

            RoleTenan model = new RoleTenan();
            var tenanList = await base.Query(a => a.RoleId == roleTenan.RoleId && a.TenanId == roleTenan.TenanId);
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
        public async Task<int> GetTenanIdByRid(int rid)
        {
            return ((await base.Query(d => d.RoleId == rid)).OrderByDescending(d => d.TenanId).LastOrDefault()?.RoleId).ObjToInt();
        }
    }
}
