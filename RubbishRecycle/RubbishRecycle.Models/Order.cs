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
        [MaxLength(36)]
        [Column("id")]
        public String Id { get; set; }

        [MaxLength(36)]
        [Column("saler_id")]
        public String SalerId { get; set; }

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

        [MaxLength(255)]
        [Column("trading_address")]
        public String TradingAddress { get; set; }

        [MaxLength(100)]
        [Required(AllowEmptyStrings = false)]
        [Column("description")]
        public String Description { get; set; }

        public virtual ICollection<OrderStateTrace> States { get; set; }

        [ForeignKey("SalerId")]
        public virtual Account Saler { get; set; }
    }
}
