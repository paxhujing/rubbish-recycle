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
    public class OrderRepository : RepositoryBase<RubbishRecycleContext>, ISalerOrderRepository<RubbishRecycleContext>
    {
        #region Constructors

        public OrderRepository(RubbishRecycleContext dbContext)
            : base(dbContext)
        {


        }

        #endregion

        #region Saler

        public Boolean AddOrder(Order order)
        {
            if (base.DbContext.Orders.Any(x => IsOrderFinished(x)))
            {
                order.Id = Guid.NewGuid().ToString().Replace("-", String.Empty);
                order.State = OrderState.NotPay;
                order.Timestamp = DateTime.Now.Date;
                base.DbContext.Orders.Add(order);
                if (base.DbContext.SaveChanges() > 1)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean DeleteOrder(String orderId)
        {
            Order order = base.DbContext.Orders.SingleOrDefault(x => x.Id == orderId);
            if (orderId == null) return false;
            if (IsOrderExpired(order) || IsOrderFinished(order))
            {
                base.DbContext.Entry(order).State = EntityState.Deleted;
                return base.DbContext.SaveChanges() > 1;
            }
            return false;
        }

        public bool ModifyOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrderView> GetOrderViewsByPage(Int32 pageNo, Int32 pageSize)
        {
            throw new NotImplementedException();
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
