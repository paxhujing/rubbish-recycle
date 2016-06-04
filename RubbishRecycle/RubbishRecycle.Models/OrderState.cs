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
        /// 未支付（此状态由系统生成，是订单初始状态，会保存在数据库中）。
        /// 只能在支付后才会被审核。
        /// 此时可以修改、删除订单信息。
        /// </summary>
        NotPay,
        /// <summary>
        /// 审核中（此状态由管理设置，会保存在数据库中）。
        /// 已支付成功。
        /// 此时不能修改，但可以删除订单信息。
        /// </summary>
        Reviewing,
        /// <summary>
        /// 未通过（此状态由管理设置，会保存在数据库中）。
        /// 由Reviewing状态转换而来。
        /// 此时可以修改、删除订单信息。
        /// </summary>
        NotPass,
        /// <summary>
        /// 待确认（此状态由系统生成，会保存在数据库中）。
        /// 由Reviewing状态转换而来。
        /// 此时不能修改，但可以删除订单信息。
        /// </summary>
        Waiting,
        /// <summary>
        /// 等待过期（此状态由系统根据超时策略动态生成，不会保存在数据库中）。
        /// 可以由NotPay、NotPass、Waiting状态转到该状态。
        /// 此时可以修改、删除订单信息。
        /// </summary>
        Expire,
        /// <summary>
        /// 线下交易中（此状态由买家操作得到，会保存在数据库中）。
        /// 由Waiting状态转换而来。
        /// 此时不能修改、删除订单信息。
        /// </summary>
        Trading,
        /// <summary>
        /// <summary>
        /// 完成（此状态由买家操作得到-会保存在数据库中，或由系统生成-不会保存在数据库中）。
        /// 由Trading状态转换而来。
        /// 此时不能修改，但可以删除订单信息。
        /// </summary>
        Finish,
    }
}
