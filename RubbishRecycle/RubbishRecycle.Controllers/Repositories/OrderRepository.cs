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
    public class OrderRepository : RepositoryBase<RubbishRecycleContext>, IOrderRepository<RubbishRecycleContext>
    {
        #region Constructors

        public OrderRepository(RubbishRecycleContext dbContext)
            : base(dbContext)
        {


        }

        #endregion

        #region IOrderRepository<TKey>接口

        [RubbishRecycleAuthorize(Roles = "saler")]
        public Order PublishOrder(Order order, out String message)
        {
            Order existedOrder = base.DbContext.Orders.FirstOrDefault(x => x.SalerId == order.SalerId);
            if (existedOrder != null)
            {
                if (existedOrder.States.Any(x => (x.State != OrderState.Finish)))
                {
                    message = "存在未结束的交易.您可以删除该订单或修改后重新提交";
                    return null;
                }
            }
            order.Id = Guid.NewGuid().ToString().Replace("-", String.Empty);
            base.DbContext.Orders.Add(order);
            if (base.DbContext.SaveChanges() > 1)
            {
                OrderOperationStateTrace stateTrace = order.AddOrderStarteTrace(OrderState.NotPay);
                base.DbContext.OrderStates.Add(stateTrace);
                base.DbContext.SaveChanges();
                message = "创建订单成功";
                return order;
            }
            message = "创建订单失败";
            return null;
        }

        [RubbishRecycleAuthorize(Roles = "saler")]
        public Boolean DeletetPublishOrder(String id, String salerId)
        {
            Order order = base.DbContext.Orders.FirstOrDefault(x => (x.Id == id) && (x.SalerId == salerId));
            if (order == null) return true;
            base.DbContext.Orders.Remove(order);
            return base.DbContext.SaveChanges() != 0;
        }

        public Order GetMyPublishOrder(String id, String salerId)
        {
            return base.DbContext.Orders.FirstOrDefault(x => (x.Id == id) && (x.SalerId == salerId));
        }

        public IQueryable<Order> GetMyPublishOrders(String salerId, Int32 pageNo, Int32 pageSize)
        {
            return base.DbContext.Orders.Where(x => x.SalerId == salerId).OrderByDescending(y => y.CreateTime).Skip(pageNo * pageSize).Take(pageSize);

        }

        public IQueryable<Order> GetPublishOrders(Int32 pageNo, Int32 pageSize)
        {
            return base.DbContext.Orders.OrderByDescending(x => x.CreateTime).Skip(pageNo * pageSize).Take(pageSize);
        }

        #endregion
    }
}
