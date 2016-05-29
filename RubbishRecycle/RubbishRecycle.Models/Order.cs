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
        public Order()
        {
            CreateTime = DateTime.Now;
        }

        [Key]
        [MaxLength(36)]
        [Column("id")]
        public String Id { get; set; }

        [MaxLength(36)]
        [Column("saler_id")]
        public String SalerId { get; set; }

        [MaxLength(100)]
        [Column("type")]
        public String Type { get; set; }

        [MaxLength(10)]
        [Column("unit")]
        public String Unit { get; set; }

        [MaxLength(10)]
        [Column("quantity")]
        public Double Quantity { get; set; }

        [Column("photo")]
        public Byte[] Photo { get; set; }

        [Column("create_time")]
        public DateTime CreateTime { get; private set; }

        [MaxLength(255)]
        [Column("address")]
        public String Address { get; set; }

        [MaxLength(255)]
        [Column("detail_address")]
        public String DetailAddress { get; set; }

        [MaxLength(100)]
        [Required(AllowEmptyStrings = false)]
        [Column("description")]
        public String Description { get; set; }

        public virtual ICollection<OrderOperationStateTrace> States { get; set; }

        [ForeignKey("SalerId")]
        public virtual Account Saler { get; set; }

        public OrderOperationStateTrace AddOrderStarteTrace(OrderState state,String description = null)
        {
            if (States.Any(x => x.State == state)) return null;
            OrderOperationStateTrace stateTrace = new OrderOperationStateTrace(this.Id);
            stateTrace.State = state;
            stateTrace.Description = description;
            States.Add(stateTrace);
            return stateTrace;
        }
    }
}
