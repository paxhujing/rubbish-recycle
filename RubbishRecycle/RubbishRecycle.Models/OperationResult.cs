using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    [JsonObject("result")]
    public struct OperationResult<T>
    {
        [JsonProperty("is_sussess")]
        public Boolean IsSussess { get { return String.IsNullOrWhiteSpace(this.ErrorMessage); } }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("error_message")]
        public String ErrorMessage { get; set; }
    }
}
