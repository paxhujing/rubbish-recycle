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

        private readonly IOrderRepository<RubbishRecycleContext> _orderRepository;

        #endregion

        #region Constructors

        public OrderController()
        {
            this._orderRepository = new OrderRepository(AppGlobal.DbContext);
        }

        #endregion

        //#region Actions

        //[RubbishRecycleAuthorize(Roles = "admin")]
        //[Route("GetOrders")]
        //[HttpGet]
        //public IQueryable<Order> GetOrders(Int32 pageNo, Int32 pageSize =10)
        //{
        //    return this._orderRepository.GetOrders(pageSize, pageSize);
        //}

        //[RubbishRecycleAuthorize(Roles = "saler")]
        //[Route("GetSalerOrders")]
        //[HttpGet]
        //public IQueryable<Order> GetSalerOrders(String salerId, Int32 pageNo, Int32 pageSize = 10)
        //{
        //    return this._orderRepository.GetOrders(salerId, pageNo, pageSize);
        //}

        //[RubbishRecycleAuthorize(Roles = "buyer")]
        //[Route("GetBuyerOrders")]
        //[HttpGet]
        //public IQueryable<Order> GetBuyerOrders(String buyerId, Int32 pageNo, Int32 pageSize = 10)
        //{
        //    return this._orderRepository.GetOrders(buyerId, pageNo, pageSize);
        //}

        //[RubbishRecycleAuthorize(Roles = "saler")]
        //[Route("GetOrder")]
        //[HttpGet]
        //public Order GetOrder(String id, String salerId)
        //{
        //    return this._orderRepository.GetOrder(id, salerId);
        //}

        //[RubbishRecycleAuthorize(Roles = "saler")]
        //[Route("RemoveOrder")]
        //[HttpDelete]
        //public Boolean RemoveOrder(String id, String salerId)
        //{
        //    return this._orderRepository.DeleteOrder(id, salerId);
        //}

        //[RubbishRecycleAuthorize(Roles = "saler")]
        //[Route("CreateOrder")]
        //[HttpPost]
        //public Order CreateOrder(Order order, out String message)
        //{
        //    return this._orderRepository.CreateOrder(order, out message);
        //}

        //#endregion
    }
}
