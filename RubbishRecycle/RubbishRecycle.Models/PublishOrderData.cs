using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    public class PublishOrderData
    {
        [JsonProperty("type")]
        public String Type { get; set; }

        [JsonProperty("rubbish_description")]
        public String RubbishDescription { get; set; }

        [JsonProperty("unit")]
        public String Unit { get; set; }

        [JsonProperty("quantity")]
        public Single Quantity { get; set; }

        [JsonProperty("photo")]
        public Byte[] Photo { get; set; }

        [JsonProperty("address")]
        public String Address { get; set; }

        [JsonProperty("detail_address")]
        public String DetailAddress { get; set; }

        [JsonProperty("except_trade_date")]
        public DateTime ExceptTradeDate { get; set; }
    }
}
