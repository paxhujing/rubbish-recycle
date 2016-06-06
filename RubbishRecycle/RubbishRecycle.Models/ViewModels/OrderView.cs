﻿using Newtonsoft.Json;
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
        [JsonProperty("name")]
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
        public Single Quantity { get; set; }

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
        /// 卖家的综合评分。
        /// </summary>
        [JsonProperty("comprehensive_score")]
        public Single ComprehensiveScore { get; set; }

    }
}