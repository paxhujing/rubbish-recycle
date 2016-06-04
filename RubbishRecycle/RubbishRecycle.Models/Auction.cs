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
    /// 描述买家的竞价信息。
    /// </summary>
    [Table("auction")]
    public class Auction
    {
        /// <summary>
        /// Id号。
        /// </summary>
        [Key]
        [MaxLength(36)]
        [Column("id")]
        public String Id { get; set; }

        /// <summary>
        /// 所属订单Id号。
        /// </summary>
        [MaxLength(36)]
        [Required(AllowEmptyStrings = false)]
        [Column("saler_id")]
        public String OrderId { get; set; }

        /// <summary>
        /// 所属订单。
        /// </summary>
        [MaxLength(36)]
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

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
        public virtual Account Buyer { get; set; }

        /// <summary>
        /// 单价。
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [Column("price")]
        public Single Price { get; set; }

        /// <summary>
        /// 预期交易日期。
        /// </summary>
        [Column("predict_trade_date")]
        public DateTime PredictTradeDate { get; set; }

        /// <summary>
        /// 综合评分。
        /// </summary>
        [Column("comprehensive_score")]
        public Single ComprehensiveScore { get; set; }
    }
}
