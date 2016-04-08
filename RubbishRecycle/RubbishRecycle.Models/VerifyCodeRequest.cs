using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    /// <summary>
    /// 验证码请求。
    /// </summary>
    public struct VerifyCodeRequest
    {
        [JsonProperty("binding_phone")]
        public String BindingPhone;

        [JsonProperty("role_id")]
        public String RoleId;
    }
}
