using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    public struct OperationResult<T>
    {
        [JsonProperty("is_success")]
        public Boolean IsSuccess;

        [JsonProperty("data")]
        public T Data;

        [JsonProperty("error_message")]
        public String ErrorMessage;
    }

    public struct OperationResult
    {
        [JsonProperty("is_success")]
        public Boolean IsSuccess;

        [JsonProperty("error_message")]
        public String ErrorMessage;

        [JsonProperty("data")]
        private Object Data;
    }
}
