using RubbishRecycle.Models;
using RubbishRecycle.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Repositories
{
    public interface ISalerOrderRepository<TKey> : IRepository<TKey>
        where TKey : DbContext
    {
        /// <summary>
        /// 添加订单。
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Boolean AddOrder(Order order);

        /// <summary>
        /// 删除订单。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Boolean DeleteOrder(String id);

        /// <summary>
        /// 修改订单。
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Boolean ModifyOrder(Order order);

        /// <summary>
        /// 分页获取订单视图列表。
        /// </summary>
        /// <param name="salerId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IEnumerable<OrderView> GetOrderViewsByPage(String salerId, Int32 pageNo, Int32 pageSize);
    }
}
