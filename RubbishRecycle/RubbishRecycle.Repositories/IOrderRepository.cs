using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Repositories
{
    public interface IOrderRepository<TKey> : IRepository<TKey>
        where TKey : DbContext
    {
        /// <summary>
        /// 发布订单。
        /// </summary>
        /// <param name="order"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Order PublishOrder(Order order,out String message);

        /// <summary>
        /// 删除发布的订单。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="salerId"></param>
        /// <returns></returns>
        Boolean DeletetPublishOrder(String id, String salerId);

        /// <summary>
        /// 根据指定Id获取我发布订单。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="salerId"></param>
        /// <returns></returns>
        Order GetMyPublishOrder(String id, String salerId);

        /// <summary>
        /// 分页获取指定卖家的发布订单列表。
        /// </summary>
        /// <param name="salerId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<Order> GetMyPublishOrders(String salerId, Int32 pageNo, Int32 pageSize);

        /// <summary>
        /// 分页获取所有卖家的订单列表。
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<Order> GetPublishOrders(Int32 pageNo, Int32 pageSize);
    }
}
