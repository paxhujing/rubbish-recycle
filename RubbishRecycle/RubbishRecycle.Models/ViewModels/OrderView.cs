using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models.ViewModels
{
    /// <summary>
    /// 订单视图。
    /// </summary>
    public class OrderView
    {
        /// <summary>
        /// 订单Id号。
        /// </summary>
        [JsonProperty("order_id")]
        public String OrderId { get; set; }

        /// <summary>
        /// 类型。
        /// </summary>
        [JsonProperty("type")]
        public String Type { get; set; }

        /// <summary>
        /// 类型描述。
        /// </summary>
        [JsonProperty("rubbish_description")]
        public String RubbishDescription { get; set; }

        /// <summary>
        /// 单位。
        /// </summary>
        [JsonProperty("unit")]
        public String Unit { get; set; }

        /// <summary>
        /// 数量。
        /// </summary>
        [JsonProperty("quantity")]
        public Double Quantity { get; set; }

        /// <summary>
        /// 照片。
        /// </summary>
        [JsonProperty("photo")]
        public Byte[] Photo { get; set; }

        /// <summary>
        /// 发布日期。
        /// </summary>
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 期望交易日期。
        /// </summary>
        [JsonProperty("except_trade_date")]
        public DateTime ExceptTradeDate { get; set; }

        /// <summary>
        /// 卖家的综合评分。
        /// </summary>
        [JsonProperty("comprehensive_score")]
        public Double ComprehensiveScore { get; set; }

    }
}
