using RubbishRecycle.Controllers.Assets;
using RubbishRecycle.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using RubbishRecycle.Models;
using System.Data.Entity;
using RubbishRecycle.Models.ViewModels;
using System.Linq.Expressions;
using System.Data.Entity.SqlServer;

namespace RubbishRecycle.Controllers.Repositories
{
    public class SalerOrderRepository : RepositoryBase<RubbishRecycleContext>, ISalerOrderRepository<RubbishRecycleContext>
    {
        #region Constructors

        public SalerOrderRepository(RubbishRecycleContext dbContext)
            : base(dbContext)
        {


        }

        #endregion

        #region Saler

        public Boolean AddOrder(Order order)
        {
            TimeSpan ts = order.ExceptTradeDate.Date - DateTime.Now.Date;
            if ((ts.Days < 0) || (ts.Days > 3)) return false;
            Order unfinishOrder = base.DbContext.Orders.SingleOrDefault(x => (x.SalerId == order.SalerId) && (x.State == OrderState.Trading));
            if (unfinishOrder != null)
            {
                if (IsTradeExpire(unfinishOrder))
                {
                    unfinishOrder.State = OrderState.Finish;
                    base.DbContext.Entry(unfinishOrder).State = EntityState.Modified;
                }
                else
                {
                    return false;
                }
            }
            order.Id = Guid.NewGuid().ToString().Replace("-", String.Empty);
            order.State = OrderState.NotPay;
            order.Timestamp = DateTime.Now.Date;
            base.DbContext.Orders.Add(order);
            return base.DbContext.SaveChanges() > 0;
        }

        public Boolean DeleteOrder(String salerId, String orderId)
        {
            Order order = base.DbContext.Orders.SingleOrDefault(x => (x.SalerId == salerId) && (x.Id == orderId));
            if (order == null) return true;
            if(IsTradeExpire(order))
            {
                order.State = OrderState.Finish;
            }
            if (order.State == OrderState.Trading) return false;
            base.DbContext.Entry(order).State = EntityState.Deleted;
            return base.DbContext.SaveChanges() > 0;
        }

        public Boolean ModifyOrder(String salerId, Order order)
        {
            if (order.SalerId != salerId) return false;
            if (IsWaitExpire(order))
            {
                order.State = OrderState.Expire;
            }
            if ((order.State == OrderState.NotPay)
                || (order.State == OrderState.NotPass)
                ||(order.State == OrderState.Expire))
            {
                base.DbContext.Entry(order).State = EntityState.Modified;
                return base.DbContext.SaveChanges() > 0;
            }
            return false;
        }

        public IQueryable<Order> GetOrdersByPage(String salerId, Int32 pageNo, Int32 pageSize = 10) 
        {
            IQueryable<Order> pageResult = (from o in base.DbContext.Orders
                                            where o.SalerId == salerId
                                            orderby o.Timestamp descending
                                            select o).Skip((pageNo - 1) * pageSize).Take(pageSize);

            return pageResult;
        }

        public IEnumerable<OrderView> GetOrderViewsByPage(Int32 pageNo, Int32 pageSize = 10)
        {
            Expression<Func<Order, Boolean>> exp = o => (o.State == OrderState.Waiting) && !IsWaitExpire(o);
            Func<Order, Boolean> fun = exp.Compile();
            IEnumerable<OrderView> pageResult = base.DbContext.Orders.Where(fun).Select(x => new OrderView
            {
                OrderId = x.Id,
                Type = x.Type,
                RubbishDescription = x.RubbishDescription,
                Unit = x.Unit,
                Quantity = x.Quantity,
                Photo = x.Photo,
                Timestamp = x.Timestamp,
                ComprehensiveScore = x.ComprehensiveScore,
            }).ToArray();
            return pageResult;
        }

        #endregion

        #region Misc

        private Boolean IsTradeExpire(Order order)
        {
            if (order.State == OrderState.Trading)
            {
                return SqlFunctions.DateDiff("d", order.ExceptTradeDate, DateTime.Now) > 3;
            }
            return false;
        }

        private Boolean IsWaitExpire(Order order)
        {
            if (order.State == OrderState.Waiting)
            {
                return SqlFunctions.DateDiff("d", order.Timestamp, DateTime.Now) > 3;
            }
            return false;
        }

        #endregion
    }
}
