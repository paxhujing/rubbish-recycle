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
        public Int32 Id { get; private set; }

        public Int32 SalerId { get; set; }

        public RubbishType Type { get; set; }

        public Double Quantity { get; set; }

        public QuantityUnit Unit { get; set; }

        public DateTime Expire { get; set; }

        public Int32 BuyerId { get; set; }

        public Double Price { get; set; }

        [MaxLength(255)]
        public String Address { get; set; }

        public DateTime Timestamp { get; set; }

        public Byte[] Photo { get; set; }
    }
}
