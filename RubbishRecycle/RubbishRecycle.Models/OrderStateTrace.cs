using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    [Table("order_state_trace")]
    public class OrderStateTrace
    {
        [Key]
        [MaxLength(36)]
        [Column("id")]
        public String Id { get; set; }

        [MaxLength(36)]
        [Column("order_id")]
        public String OrderId { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("state")]
        public OrderState State { get; set; }

        /// <summary>
        /// 如果为NotPass，则表示未通过原因。
        /// 如果为Trading，则表示“交易确认码”。
        /// </summary>
        [Column("extend")]
        public String Extend { get; set; }

        public virtual Order Order { get; set; }
    }
}
