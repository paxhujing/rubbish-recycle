using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    public struct OperationResult
    {
        [JsonProperty("is_success")]
        public Boolean IsSuccess;

        [JsonProperty("error_message")]
        public String ErrorMessage;

        [JsonProperty("data")]
        public Object Data;
    }
}
