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
    public enum OrderState : byte
    {
        /// <summary>
        /// 审核中。
        /// </summary>
        Reviewing,
        /// <summary>
        /// 待确认。
        /// </summary>
        Waiting,
        /// <summary>
        /// 过期。
        /// </summary>
        Expire,
        /// <summary>
        /// 确定。
        /// </summary>
        Confirm,
        /// <summary>
        /// 完成。
        /// </summary>
        Finish,
        /// <summary>
        /// 取消。
        /// </summary>
        Cancel,
    }
}
