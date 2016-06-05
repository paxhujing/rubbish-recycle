using RubbishRecycle.Controllers.Assets;
using RubbishRecycle.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RubbishRecycle.Models;

namespace RubbishRecycle.Controllers.Repositories
{
    internal class AppKeyRepository : RepositoryBase<RubbishRecycleContext>, IAppKeyRepository<RubbishRecycleContext>
    {
        #region Fields

        private static readonly IList<AppKeyInfo> AppKeys = new List<AppKeyInfo>();

        #endregion

        #region Constructors

        public AppKeyRepository(RubbishRecycleContext dbContext)
            : base(dbContext)
        {

        }

        #endregion

        #region Methods

        public AppKeyInfo GetAppKeyInfo(String appKey)
        {
            AppKeyInfo info = AppKeys.SingleOrDefault(x => x.AppKey == appKey);
            if (info == null)
            {
                info = base.DbContext.AppKeyInfos.SingleOrDefault(x => x.AppKey == appKey);
                if (info != null)
                {
                    AppKeys.Add(info);
                }
            }
            return info;
        }

        #endregion
    }
}
