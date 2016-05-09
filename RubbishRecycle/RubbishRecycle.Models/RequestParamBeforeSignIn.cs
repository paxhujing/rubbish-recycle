using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    /// <summary>
    /// 登陆前的请求参数。
    /// </summary>
    /// <typeparam name="T">数据类型。</typeparam>
    public struct RequestParamBeforeSignIn<T>
    {
        /// <summary>
        /// 数据。
        /// </summary>
        [JsonProperty("data")]
        public T Data;

        /// <summary>
        /// 客户端的AppKey。
        /// </summary>
        [JsonProperty("app_key")]
        public String AppKey;
    }
}
