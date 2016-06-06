﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models.ViewModels
{
    public class AccountView
    {
        /// <summary>
        /// 账户名称。
        /// </summary>
        [JsonProperty("name")]
        public String Name { get; set; }

        /// <summary>
        /// 绑定的手机号。
        /// </summary>
        [JsonProperty("binding_phone")]
        public String BindingPhone { get; set; }

        /// <summary>
        /// 积分。
        /// </summary>
        [JsonProperty("auction")]
        public Single Auction { get; set; }

        /// <summary>
        /// 最近登录日期。
        /// </summary>
        [JsonProperty("last_login")]
        public String LastLogin { get; set; }

        /// <summary>
        /// 信誉评分。
        /// </summary>
        [JsonProperty("credit_score")]
        public Single CreditScore { get; set; }

        /// <summary>
        /// 态度评分。
        /// </summary>
        [JsonProperty("attitude_score")]
        public Single AttitudeScore { get; set; }

        [JsonProperty("is_freezed")]
        public Boolean IsFreezed { get; set; }
    }
}