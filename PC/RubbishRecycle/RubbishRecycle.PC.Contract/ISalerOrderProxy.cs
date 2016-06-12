using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.PC.Contract
{
    public interface ISalerOrderProxy
    {
        OperationResult PublishOrder(Order order, String token);
        OperationResult DeleteOrder(String orderId, String token);
        OperationResult ModifyOrder(Order order, String token);
        OperationResult QueryOrdersByPage(String token, Int32 pageNo, Int32 pageSize);
        OperationResult QueryOrderViewsByPage(Int32 pageNo, Int32 pageSize);
    }
}
