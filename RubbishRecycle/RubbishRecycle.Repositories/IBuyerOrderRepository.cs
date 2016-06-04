using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Repositories
{
    public interface IBuyerOrderRepository<TKey> : IRepository<TKey>
        where TKey : DbContext
    {
        /// <summary>
        /// 添加竞价信息。
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="acuction"></param>
        /// <returns></returns>
        Boolean AddAuction(String orderId, Auction acuction);

        /// <summary>
        /// 添加订单摘要信息。
        /// </summary>
        /// <param name="summary"></param>
        /// <returns></returns>
        Boolean AddOrderSummary(OrderSummary summary);

        /// <summary>
        /// 删除订单摘要信息。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Boolean DeleteOrderSummary(String id);

        /// <summary>
        /// 分页获取订单摘要列表。
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IEnumerable<OrderSummary> GetOrderSummariesByPage(Int32 pageNo, Int32 pageSize);
    }
}
