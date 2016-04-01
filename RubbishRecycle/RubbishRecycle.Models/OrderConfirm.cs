using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    [Table("order_confirm")]
    public class OrderConfirm
    {
        [MaxLength(36)]
        [Column("id")]
        public String Id { get; set; }

        [MaxLength(36)]
        [Column("order_id")]
        public String OrderId { get; set; }

        [MaxLength(36)]
        [Column("buyer_id")]
        public String BuyerId { get; set; }

        [Column("price")]
        public Double Price { get; set; }

        [Column("predict_trading_time")]
        public DateTime PredictTradingTime { get; set; }

        [Column("confirm_code")]
        public String ConfirmCode { get; set; }
    }
}
