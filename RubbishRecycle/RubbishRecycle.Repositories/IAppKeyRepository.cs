using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Repositories
{
    public interface IAppKeyRepository<TDbContext> : IRepository<TDbContext>
        where TDbContext : DbContext
    {
        /// <summary>
        /// 获取AppKey的信息。
        /// </summary>
        /// <param name="appKey">应用程序的AppKey字符串。</param>
        /// <returns></returns>
        AppKeyInfo GetAppKeyInfo(String appKey);
    }
}
