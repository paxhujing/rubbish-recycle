﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    [Table("order_operation_state_trace")]
    public class OrderOperationStateTrace
    {
        public OrderOperationStateTrace(String orderId)
        {
            Timestamp = DateTime.Now;
            OrderId = orderId;
        }

        [Key]
        [MaxLength(36)]
        [Column("id")]
        public String Id { get; set; }

        [MaxLength(36)]
        [Column("order_id")]
        public String OrderId { get; private set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; private set; }

        [Column("state")]
        public OrderState State { get; set; }

        /// <summary>
        /// 如果为NotPass，则表示未通过原因。
        /// 如果为Trading，则表示“交易确认码”。
        /// </summary>
        [Column("description")]
        public String Description { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
    }
}
