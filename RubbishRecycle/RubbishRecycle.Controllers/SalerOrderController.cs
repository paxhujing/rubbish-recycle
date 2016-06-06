using MySql.Data.MySqlClient;
using RubbishRecycle.Controllers.Assets;
using RubbishRecycle.Controllers.Repositories;
using RubbishRecycle.Models;
using RubbishRecycle.Models.ViewModels;
using RubbishRecycle.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RubbishRecycle.Controllers
{
    public class SalerOrderController : ApiController
    {
        #region Fields

        private readonly ISalerOrderRepository<RubbishRecycleContext> _orderRepository;

        #endregion

        #region Constructors

        public SalerOrderController()
        {
            this._orderRepository = new SalerOrderRepository(AppGlobal.DbContext);
        }

        #endregion

        #region Actions

        [RubbishRecycleAuthorize(Roles = "saler")]
        public OperationResult PublishOrder(Order order)
        {
            AccountToken at = base.ActionContext.GetAccountTokenFromActionContext();
            order.SalerId = at.AccountId;
            if (this._orderRepository.AddOrder(order))
            {
                return OperationResultHelper.GenerateSuccessResult(order);
            }
            return OperationResultHelper.GenerateErrorResult("发布订单失败。请检查是否存在未完成的订单，或无效的订单数据");
        }

        [RubbishRecycleAuthorize(Roles = "saler")]
        public OperationResult DeleteOrder(String orderId)
        {
            AccountToken at = base.ActionContext.GetAccountTokenFromActionContext();
            if (this._orderRepository.DeleteOrder(at.AccountId, orderId))
            {
                return OperationResultHelper.GenerateSuccessResult();
            }
            return OperationResultHelper.GenerateErrorResult("无法删除订单。请检查订单是否仍在交易中");
        }

        [RubbishRecycleAuthorize(Roles = "saler")]
        public OperationResult ModifyOrder(Order order)
        {
            AccountToken at = base.ActionContext.GetAccountTokenFromActionContext();
            if (this._orderRepository.ModifyOrder(at.AccountId, order))
            {
                return OperationResultHelper.GenerateSuccessResult();
            }
            return OperationResultHelper.GenerateErrorResult("无法修改订单。请检查订单数据是否正确");

        }

        [RubbishRecycleAuthorize(Roles = "saler")]
        public OperationResult QueryOrderByPage(Int32 pageNo, Int32 pageSize = 10)
        {
            AccountToken at = base.ActionContext.GetAccountTokenFromActionContext();
            IQueryable<Order> orders = this._orderRepository.GetOrdersByPage(at.AccountId, pageNo, pageSize);
            return OperationResultHelper.GenerateSuccessResult(orders);
        }

        [AllowAnonymous]
        public OperationResult QueryOrderViewsByPage(Int32 pageNo, Int32 pageSize = 10)
        {
            IEnumerable<OrderView> orderViews = this._orderRepository.GetOrderViewsByPage(pageNo, pageSize);
            return OperationResultHelper.GenerateSuccessResult(orderViews);

        }

        #endregion
    }
}
