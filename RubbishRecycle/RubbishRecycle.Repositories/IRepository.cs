using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Repositories
{
    /// <summary>
    /// 描述与数据库操作相关的功能。
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepository<TKey>
        where TKey : DbContext
    {
        /// <summary>
        /// 数据库上下文。
        /// </summary>
        TKey DbContext { get; }
    }
}
