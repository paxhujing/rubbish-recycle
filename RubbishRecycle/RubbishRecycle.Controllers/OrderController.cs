﻿using MySql.Data.MySqlClient;
using RubbishRecycle.Controllers.Assets;
using RubbishRecycle.Controllers.Assets.DB;
using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace RubbishRecycle.Controllers
{
    [RoutePrefix("api/order")]
    public class OrderController : ApiController
    {
        #region Fields

        IList<Order> _orders = new List<Order>();

        #endregion

        #region Constructors

        public OrderController()
        {
        }

        #endregion

        #region Methods

        [RubbishRecycleAuthorize(Roles = "Saler")]
        [Route("GetOrders")]
        [HttpGet]
        public IEnumerable<Order> GetOrders()
        {
            using (RubbishRecycleContext context = new RubbishRecycleContext())
            {
                return context.Orders;
            }
        }

        [RubbishRecycleAuthorize(Roles = "Saler")]
        [Route("GetOrderById")]
        [HttpGet]
        public Order GetOrderById(Int32 id)
        {
            using (RubbishRecycleContext context = new RubbishRecycleContext())
            {
                Order order = context.Orders.FirstOrDefault(x => x.Id == id);
                if (order == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return order;
            }
        }

        [RubbishRecycleAuthorize(Roles = "Saler")]
        [Route("AddOrder")]
        [HttpPost]
        public Int32 AddOrder(Order order)
        {
            using (RubbishRecycleContext context = new RubbishRecycleContext())
            {
                Order added = context.Orders.Add(order);
                context.SaveChanges();
                return added.Id;
            }
        }

        [RubbishRecycleAuthorize(Roles = "Saler")]
        [Route("RemoveOrder")]
        [HttpDelete]
        public void RemoveOrder(Order order)
        {
            using (RubbishRecycleContext context = new RubbishRecycleContext())
            {
                context.Orders.Remove(order);
                context.SaveChanges();
            }
        }

        #endregion
    }
}