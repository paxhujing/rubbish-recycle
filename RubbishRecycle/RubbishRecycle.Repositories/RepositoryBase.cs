using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Repositories
{
    public class RepositoryBase<TKey>
        where TKey : DbContext
    {
        #region Constructors

        protected RepositoryBase(TKey dbContext)
        {
            this._dbContext = dbContext;
        }

        #endregion

        #region Properties

        #region DbContext

        private readonly TKey _dbContext;
        /// <summary>
        /// 数据库上下文。
        /// </summary>
        public TKey DbContext
        {
            get { return this._dbContext; }
        }

        #endregion

        #endregion
    }
}
