using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    /// <summary>
    /// 描述买家确认后订单的摘要信息。
    /// </summary>
    [Table("order_summary")]
    public class OrderSummary
    {
        /// <summary>
        /// Id号。
        /// </summary>
        [Key]
        [MaxLength(36)]
        [Column("id")]
        public String Id { get; set; }

        /// <summary>
        /// 所属买家Id号。
        /// </summary>
        [MaxLength(36)]
        [Required(AllowEmptyStrings = false)]
        [Column("buyer_id")]
        public String BuyerId { get; set; }

        /// <summary>
        /// 所属买家账号。
        /// </summary>
        [ForeignKey("BuyerId")]
        public virtual Buyer Buyer { get; set; }

        /// <summary>
        /// 废品所属卖家Id号。
        /// </summary>
        [MaxLength(36)]
        [Required(AllowEmptyStrings = false)]
        [Column("saler_id")]
        public String SalerId { get; set; }

        /// <summary>
        /// 废品所属卖家账号。
        /// </summary>
        [ForeignKey("SalerId")]
        public virtual Account Saler { get; set; }

        /// <summary>
        /// 类型。
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [Column("type")]
        public String Type { get; set; }

        /// <summary>
        /// 类型描述。
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [Column("name")]
        public String RubbishDescription { get; set; }

        /// <summary>
        /// 单位。
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [Column("unit")]
        public String Unit { get; set; }

        /// <summary>
        /// 数量。
        /// </summary>
        [Column("quantity")]
        public Single Quantity { get; set; }

        /// <summary>
        /// 照片。
        /// </summary>
        [Column("photo")]
        public Byte[] Photo { get; set; }

        /// <summary>
        /// 确认时间戳。
        /// </summary>
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
