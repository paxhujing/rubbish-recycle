using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    [Table("order")]
    public class Order
    {
        [Key]
        [Column("id")]
        public Int32 Id { get; private set; }

        [Column("saler_id")]
        public Int32 SalerId { get; set; }

        [Column("order_confirm_id")]
        public Int32? OrderConfirmId { get; set; }

        [Column("rubbish_type_id")]
        public Int32 RubbishTypeId { get; set; }

        [Column("quantity_unit_id")]
        public Int32 QuantityUnitId { get; set; }

        [Column("quantity")]
        public Double Quantity { get; set; }

        [Column("photo")]
        public Byte[] Photo { get; set; }

        [Column("create_time")]
        public DateTime CreateTime { get; set; }

        [Column("expire_time")]
        public DateTime ExpireTime { get; set; }

        [MaxLength(255)]
        [Column("trading_address")]
        public String TradingAddress { get; set; }

        [Column("order_state")]
        public OrderState State { get; set; }

        [Column("except_trading_time")]
        public DateTime ExceptTradingTime { get; set; }

        [Column("confirm_code")]
        public String ConfirmCode { get; set; }
    }
}
