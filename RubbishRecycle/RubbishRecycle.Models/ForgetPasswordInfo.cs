using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    public struct ForgetPasswordInfo
    {
        /// <summary>
        /// 手机号。
        /// </summary>
        [JsonProperty("phone")]
        public String Phone;

        /// <summary>
        /// 密码。
        /// </summary>
        [JsonProperty("password")]
        public String Password;

        /// <summary>
        /// 验证码。
        /// </summary>
        [JsonProperty("verify_code")]
        public String VerifyCode;

        /// <summary>
        /// 客户端的AppKey。
        /// </summary>
        [JsonProperty("app_key")]
        public String AppKey;

    }
}
