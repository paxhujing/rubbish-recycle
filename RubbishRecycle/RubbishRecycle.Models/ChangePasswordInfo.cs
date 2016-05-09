using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    public struct ChangePasswordInfo
    {
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
    }
}
