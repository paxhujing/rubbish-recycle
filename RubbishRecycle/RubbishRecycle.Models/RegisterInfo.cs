﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    /// <summary>
    /// 注册信息。
    /// </summary>
    public struct RegisterInfo
    {
        /// <summary>
        /// 账户名称。
        /// </summary>
        [JsonProperty("name")]
        public String Name;

        /// <summary>
        /// 验证码。
        /// </summary>
        [JsonProperty("verify_code")]
        public String VerifyCode;

        /// <summary>
        /// 绑定的手机号。
        /// </summary>
        [JsonProperty("binding_phone")]
        public String BindingPhone;

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
