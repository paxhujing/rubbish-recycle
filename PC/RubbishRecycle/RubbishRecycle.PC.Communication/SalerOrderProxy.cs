using RubbishRecycle.PC.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RubbishRecycle.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RubbishRecycle.PC.Communication
{
    public class SalerOrderProxy : WebApiProxy, ISalerOrderProxy
    {
        public OperationResult DeleteOrder(String orderId, String token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, "api/salerorder/DeleteOrder?orderId=" + orderId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
            return base.SendAsync(request).Result.Content.ReadAsAsync<OperationResult>().Result;
        }

        public OperationResult ModifyOrder(Order order, String token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, "api/salerorder/ModifyOrder");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
            request.Content = new ObjectContent<Order>(order, WebApiProxy.JsonFormatter);
            return base.SendAsync(request).Result.Content.ReadAsAsync<OperationResult>().Result;
        }

        public OperationResult PublishOrder(Order order, String token)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/salerorder/PublishOrder");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
            request.Content = new ObjectContent<Order>(order, WebApiProxy.JsonFormatter);
            return base.SendAsync(request).Result.Content.ReadAsAsync<OperationResult>().Result;
        }

        public OperationResult QueryOrdersByPage(String token, Int32 pageNo, Int32 pageSize = 10)
        {
            String @params = String.Format("pageNo={0}&pageSize={1}", pageNo, pageSize);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/salerorder/QueryOrdersByPage?" + @params);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
            return base.SendAsync(request).Result.Content.ReadAsAsync<OperationResult>().Result;
        }

        public OperationResult QueryOrderViewsByPage(Int32 pageNo, Int32 pageSize = 10)
        {
            String @params = String.Format("pageNo={0}&pageSize={1}", pageNo, pageSize);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/salerorder/QueryOrderViewsByPage?" + @params);
            return base.SendAsync(request).Result.Content.ReadAsAsync<OperationResult>().Result;
        }
    }
}
