﻿using Newtonsoft.Json;
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
    public struct SmsResult
    {
        [JsonProperty("extend")]
        public String Extend;

        [JsonProperty("is_success")]
        public Boolean IsSuccess;

        [JsonProperty("error")]
        public String Error;
    }
}
