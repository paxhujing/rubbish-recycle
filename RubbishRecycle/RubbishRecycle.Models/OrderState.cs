using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    /// <summary>
    /// 订单状态。
    /// </summary>
    public enum OrderState
    {
        /// <summary>
        /// 审核中。
        /// </summary>
        Reviewing,
        /// <summary>
        /// 未通过（交易流程结束）。
        /// </summary>
        NotPass,

        /// <summary>
        /// 待确认。
        /// </summary>
        Waiting,
        /// <summary>
        /// 等待过期（交易流程结束）。
        /// </summary>
        WaitExpire,

        /// <summary>
        /// 确认中。
        /// </summary>
        Confirming,
        /// <summary>
        /// 确认过期（交易流程结束）。
        /// </summary>
        ConfirmExpire,

        /// <summary>
        /// 线下交易中。
        /// </summary>
        Trading,
        /// <summary>
        /// 线下交易过期（交易流程结束）。
        /// </summary>
        TradeExpire,

        /// <summary>
        /// <summary>
        /// 完成（交易流程结束）。
        /// </summary>
        Finish,
    }
}
