using MySql.Data.MySqlClient;
using RubbishRecycle.Controllers.Assets;
using RubbishRecycle.Controllers.Repositories;
using RubbishRecycle.Models;
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
    public class OrderController : ApiController
    {
        #region Fields

        private readonly ISalerOrderRepository<RubbishRecycleContext> _orderRepository;

        #endregion

        #region Constructors

        public OrderController()
        {
            this._orderRepository = new OrderRepository(AppGlobal.DbContext);
        }

        #endregion

        #region Actions

        #region Saler

        [RubbishRecycleAuthorize(Roles = "saler")]
        public OperationResult CreatePublishOrder(Order order)
        {
            if (this._orderRepository.AddOrder(order))
            {
                return OperationResultHelper.GenerateSuccessResult(order);
            }
            return OperationResultHelper.GenerateErrorResult("发布订单失败。请检查是否存在未完成的订单，或无效的订单数据");
        }

        [RubbishRecycleAuthorize(Roles = "saler")]
        public OperationResult DeletePublishOrder(String orderId)
        {
            if(this._orderRepository.DeleteOrder(orderId))
            {
                return OperationResultHelper.GenerateSuccessResult();
            }
            return OperationResultHelper.GenerateErrorResult("无法删除订单。请检查是否存在未完成的订单");
        }

        #endregion

        #region Buyer

        #endregion

        #endregion
    }
}
