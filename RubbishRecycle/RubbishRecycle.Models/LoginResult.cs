using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    /// <summary>
    /// 登陆结果。
    /// </summary>
    public struct LoginResult
    {
        /// <summary>
        /// 对称加密向量。
        /// </summary>
        [JsonProperty("iv")]
        public Byte[] IV;

        /// <summary>
        /// 会话Token。
        /// </summary>
        [JsonProperty("token")]
        public String Token;
    }
}
