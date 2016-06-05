using RubbishRecycle.Controllers.Assets;
using RubbishRecycle.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            order.Id = Guid.NewGuid().ToString().Replace("-", String.Empty);
            order.State = OrderState.NotPay;
            order.Timestamp = DateTime.Now.Date;
            base.DbContext.Orders.Add(order);
            if (base.DbContext.SaveChanges() > 1)
            {
                return true;
            }
            return false;
        }

        public Boolean DeleteOrder(String orderId)
        {
            Order order = base.DbContext.Orders.SingleOrDefault(x => x.Id == orderId);
            if (orderId == null) return false;
            base.DbContext.Entry(order).State = EntityState.Deleted;
            return base.DbContext.SaveChanges() > 1;
        }

        public bool ModifyOrder(Order order)
        {
            base.DbContext.Entry(order).State = EntityState.Modified;
            return base.DbContext.SaveChanges() > 1;
        }

        public IEnumerable<OrderView> GetOrderViewsByPage(String salerId, Int32 pageNo, Int32 pageSize = 10)
        {
            IQueryable<OrderView> pageResult = (from v in base.DbContext.Orders
                                                where (v.SalerId == salerId) && (v.State == OrderState.Waiting)
                                                orderby v.Timestamp descending
                                                select v.ToView()).Skip((pageNo - 1) * pageSize).Take(pageSize);

            return pageResult;
        }

        #endregion

        #region Misc

        private Boolean IsOrderFinished(Order order)
        {
            if (order.State == OrderState.Finish) return true;
            if(order.State == OrderState.Trading)
            {
                if ((DateTime.Now.Date - order.Timestamp).Days > 7)
                {
                    order.State = OrderState.Finish;
                    return true;
                }
            }
            return false;
        }

        private Boolean IsOrderExpired(Order order)
        {
            if ((order.State != OrderState.NotPay)
                || (order.State != OrderState.NotPass)
                || (order.State != OrderState.Waiting))
            {
                if((DateTime.Now.Date - order.Timestamp).Days > 3)
                {
                    order.State = OrderState.Expire;
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
