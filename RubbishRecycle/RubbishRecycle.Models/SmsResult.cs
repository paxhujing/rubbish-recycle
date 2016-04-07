using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    /// <summary>
    /// 短信请求结果。
    /// </summary>
    public class SmsResult
    {
        [JsonProperty("code")]
        public String Code { get; set; }

        [JsonProperty("extend")]
        public String Extend { get; set; }

        [JsonProperty("is_success")]
        public Boolean IsSuccess { get; set; }

        [JsonProperty("error")]
        public String Error { get; set; }
    }
}
