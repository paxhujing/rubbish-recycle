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

        public Order CreateOrder(Order order, out String message)
        {
            Order existedOrder = base.DbContext.Orders.FirstOrDefault(x => x.SalerId == order.SalerId);
            if (existedOrder != null)
            {
                if (existedOrder.States.Any(x => (x.State == OrderState.Reviewing)
                  || (x.State == OrderState.Waiting)
                  || (x.State == OrderState.Confirming)
                  || (x.State == OrderState.Trading)))
                {
                    message = "存在未结束的交易";
                    return null;
                }
            }
            order.Id = Guid.NewGuid().ToString().Replace("-", String.Empty);
            OrderStateTrace orderState = new OrderStateTrace();
            orderState.Timestamp = DateTime.Now;
            orderState.State = OrderState.Reviewing;
            base.DbContext.Orders.Add(order);
            if (base.DbContext.SaveChanges() > 1)
            {
                message = "创建订单成功";
                return order;
            }
            message = "创建订单失败";
            return null;
        }

        public Boolean DeleteOrder(String id, String salerId)
        {
            Order order = base.DbContext.Orders.FirstOrDefault(x => (x.Id == id) && (x.SalerId == salerId));
            if (order == null) return true;
            base.DbContext.Orders.Remove(order);
            return base.DbContext.SaveChanges() != 0;
        }

        public Order GetOrder(String id, String salerId)
        {
            return base.DbContext.Orders.FirstOrDefault(x => (x.Id == id) && (x.SalerId == salerId));
        }

        public IQueryable<Order> GetOrders(String salerId, Int32 pageNo, Int32 pageSize)
        {
            return base.DbContext.Orders.Where(x => x.SalerId == salerId).OrderByDescending(y => y.CreateTime).Skip(pageNo * pageSize).Take(pageSize);

        }

        public IQueryable<Order> GetOrders(Int32 pageNo, Int32 pageSize)
        {
            return base.DbContext.Orders.OrderByDescending(x => x.CreateTime).Skip(pageNo * pageSize).Take(pageSize);
        }

        #endregion
    }
}
