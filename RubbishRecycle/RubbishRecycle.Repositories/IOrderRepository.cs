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
        /// 创建订单。
        /// </summary>
        /// <param name="order"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Order CreateOrder(Order order,out String message);

        /// <summary>
        /// 删除订单。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="salerId"></param>
        /// <returns></returns>
        Boolean DeleteOrder(String id, String salerId);

        /// <summary>
        /// 获取指定Id的订单。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="salerId"></param>
        /// <returns></returns>
        Order GetOrder(String id, String salerId);

        /// <summary>
        /// 分页获取指定卖家的订单列表。
        /// </summary>
        /// <param name="salerId"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<Order> GetOrders(String salerId, Int32 pageNo, Int32 pageSize);

        /// <summary>
        /// 分页获取所有卖家的订单列表。
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IQueryable<Order> GetOrders(Int32 pageNo, Int32 pageSize);
    }
}
