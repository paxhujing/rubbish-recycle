using Newtonsoft.Json;
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
        /// 客户端提供的密钥。
        /// </summary>
        [JsonProperty("secret_key")]
        public Byte[] SecretKey;

        /// <summary>
        /// 客户端提供的加密向量。
        /// </summary>
        [JsonProperty("iv")]
        public Byte[] IV;

        /// <summary>
        /// 账户名称。
        /// </summary>
        [JsonProperty("name")]
        public String Name;

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
    }
}
