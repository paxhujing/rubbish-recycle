using RubbishRecycle.Controllers.Assets;
using RubbishRecycle.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using RubbishRecycle.Models;
using System.Data.Entity;
using RubbishRecycle.Models.ViewModels;

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
            Order unfinishOrder = base.DbContext.Orders.SingleOrDefault(x => (x.SalerId == order.SalerId) && (x.State == OrderState.Trading));
            if (unfinishOrder != null)
            {
                if ((DateTime.Now.Date - unfinishOrder.Timestamp).Days > 7)
                {
                    unfinishOrder.State = OrderState.Finish;
                    base.DbContext.Entry(unfinishOrder).State = EntityState.Modified;

                    order.Id = Guid.NewGuid().ToString().Replace("-", String.Empty);
                    order.State = OrderState.NotPay;
                    order.Timestamp = DateTime.Now.Date;
                    base.DbContext.Orders.Add(order);
                    return base.DbContext.SaveChanges() > 1;
                }
            }
            return false;
        }

        public Boolean DeleteOrder(String salerId, String orderId)
        {
            Order order = base.DbContext.Orders.SingleOrDefault(x => (x.SalerId == salerId) && (x.Id == orderId));
            if (order == null) return true;
            if (order.State == OrderState.Trading)
            {
                if ((DateTime.Now.Date - order.Timestamp).Days > 7)
                {
                    order.State = OrderState.Finish;
                }
            }
            if (order.State == OrderState.Trading) return false;
            base.DbContext.Entry(order).State = EntityState.Deleted;
            return base.DbContext.SaveChanges() > 1;
        }

        public Boolean ModifyOrder(String salerId, Order order)
        {
            if (order.SalerId != salerId) return false;
            if (order.State == OrderState.Waiting)
            {
                if ((DateTime.Now.Date - order.Timestamp).Days > 3)
                {
                    order.State = OrderState.Expire;
                }
            }
            if ((order.State == OrderState.NotPay)
                || (order.State == OrderState.NotPass)
                ||(order.State == OrderState.Expire))
            {
                base.DbContext.Entry(order).State = EntityState.Modified;
                return base.DbContext.SaveChanges() > 1;
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
            IQueryable<OrderView> pageResult = (from v in base.DbContext.Orders
                                                where (v.State == OrderState.Waiting)
                                                && ((DateTime.Now.Date - v.Timestamp).Days <= 3)
                                                orderby v.Timestamp descending
                                                select v.ToView()).Skip((pageNo - 1) * pageSize).Take(pageSize);

            return pageResult;
        }

        #endregion

        #region Misc

        #endregion
    }
}
