using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    [Table("order_confirm")]
    public class OrderConfirm
    {
        [Column("id")]
        public Int32 Id { get; set; }

        [Column("order_id")]
        public Int32 OrderId { get; set; }

        [Column("buyer_id")]
        public Int32 BuyerId { get; set; }

        [Column("price")]
        public Double Price { get; set; }

        [Column("predict_trading_time")]
        public DateTime PredictTradingTime { get; set; }

        [Column("confirm_code")]
        public String ConfirmCode { get; set; }
    }
}
