using RubbishRecycle.Models;
using RubbishRecycle.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RubbishRecycle.Repositories
{
    public interface ISalerOrderRepository<TKey> : IRepository<TKey>
        where TKey : DbContext
    {
        /// <summary>
        /// 添加订单。
        /// </summary>
        /// <param name="salerId">发布者的Id。</param>
        /// <param name="order">新建的订单信息。</param>
        /// <returns>成功返回true；否则返回false。</returns>
        Boolean AddOrder(String salerId, PublishOrderData order);

        /// <summary>
        /// 删除订单。
        /// </summary>
        /// <param name="orderId">要删除的订单Id。</param>
        /// <param name="salerId">发布者的Id。</param>
        /// <returns>成功返回true；否则返回false。</returns>
        Boolean DeleteOrder(String salerId, String orderId);

        /// <summary>
        /// 修改订单。
        /// </summary>
        /// <param name="salerId">发布者的Id。</param>
        /// <param name="order">要修改的订单。</param>
        /// <returns>成功返回true；否则返回false。</returns>
        Boolean ModifyOrder(String salerId, Order order);

        /// <summary>
        /// 分页获取指定发布者的订单列表。
        /// </summary>
        /// <param name="salerId">发布者的Id。</param>
        /// <param name="pageNo">从1开始的页号。</param>
        /// <param name="pageSize">每页大小。</param>
        /// <returns>订单列表。</returns>
        IQueryable<Order> GetOrdersByPage(String salerId, Int32 pageNo, Int32 pageSize);

        /// <summary>
        /// 分页获取所有订单的视图列表。
        /// </summary>
        /// <param name="pageNo">从1开始的页号。</param>
        /// <param name="pageSize">每页大小。</param>
        /// <returns>订单视图列表。</returns>
        IEnumerable<OrderView> GetOrderViewsByPage(Int32 pageNo, Int32 pageSize);
    }
}
