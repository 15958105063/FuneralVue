using Funeral.Core.IServices;
using Funeral.Core.IRepository;
using Funeral.Core.Services.BASE;
using Funeral.Core.Model.Models;
using System.Threading.Tasks;
using System.Linq;
using Funeral.Core.Common;
using System.Collections.Generic;
using Funeral.Core.Common.HttpContextUser;

namespace Funeral.Core.Services
{
    /// <summary>
    /// TenanServices
    /// </summary>	
    public class TenanServices : BaseServices<Tenan>, ITenanServices
    {

        ITenanRepository _dal;
      
        public TenanServices(IUser user, ITenanRepository dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="roleName"></param>
       /// <returns></returns>
        public async Task<Tenan> SaveTenan(string tenanName)
        {
            Tenan tenan = new Tenan(tenanName);
            Tenan model = new Tenan();
            var tenanList = await base.Query(a => a.TenanName == tenan.TenanName && a.Enabled);
            if (tenanList.Count > 0)
            {
                model = tenanList.FirstOrDefault();
            }
            else
            {
                var id = await base.Add(tenan);
                model = await base.QueryById(id);
            }

            return model;

        }

        [Caching(AbsoluteExpiration = 30)]
        public async Task<string> GetTenanNameByTid(int tid)
        {
            return ((await base.QueryById(tid))?.TenanName);
        }


        /// <summary>
        /// 获取所有部门信息（缓存）
        /// </summary>
        /// <returns></returns>
        //[Caching(AbsoluteExpiration = 60)]
        public async Task<List<Tenan>> GetAll()
        {
            return await base.Query(a => a.Enabled);
        }



    }
}
