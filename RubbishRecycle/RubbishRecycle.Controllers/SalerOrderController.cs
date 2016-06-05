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
    [RubbishRecycleAuthorize(Roles = "saler")]
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

        public OperationResult PublishOrder(Order order)
        {
            if (this._orderRepository.AddOrder(order))
            {
                return OperationResultHelper.GenerateSuccessResult(order);
            }
            return OperationResultHelper.GenerateErrorResult("发布订单失败。请检查是否存在未完成的订单，或无效的订单数据");
        }

        public OperationResult DeleteOrder(String orderId)
        {
            if(this._orderRepository.DeleteOrder(orderId))
            {
                return OperationResultHelper.GenerateSuccessResult();
            }
            return OperationResultHelper.GenerateErrorResult("无法删除订单。请检查是否存在未完成的订单");
        }

        #endregion
    }
}
