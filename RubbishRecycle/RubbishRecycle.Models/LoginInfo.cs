﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    /// <summary>
    /// 登陆信息。
    /// </summary>
    public struct LoginInfo
    {
        /// <summary>
        /// 账户名称。
        /// </summary>
        [JsonProperty("name")]
        public String Name;

        /// <summary>
        /// 密码。
        /// </summary>
        [JsonProperty("password")]
        public String Password;

        /// <summary>
        /// 客户端的AppKey。
        /// </summary>
        [JsonProperty("app_key")]
        public String AppKey;
    }
}
