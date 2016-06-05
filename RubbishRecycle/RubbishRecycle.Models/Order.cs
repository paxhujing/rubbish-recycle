using RubbishRecycle.Models.ViewModels;
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
    public class Order : IConvertView<OrderView>
    {
        /// <summary>
        /// Id号。
        /// </summary>
        [Key]
        [MaxLength(36)]
        [Required(AllowEmptyStrings = false)]
        [Column("id")]
        public String Id { get; set; }
        
        /// <summary>
        /// 所属卖家的Id号。
        /// </summary>
        [MaxLength(36)]
        [Required(AllowEmptyStrings = false)]
        [Column("saler_id")]
        public String SalerId { get; set; }

        /// <summary>
        /// 所属卖家的账号。
        /// </summary>
        [ForeignKey("SalerId")]
        public virtual Saler Saler { get; set; }

        /// <summary>
        /// 类型。
        /// </summary>
        [MaxLength(100)]
        [Required(AllowEmptyStrings = false)]
        [Column("type")]
        public String Type { get; set; }

        /// <summary>
        /// 类型描述。
        /// </summary>
        [MaxLength(255)]
        [Required(AllowEmptyStrings = false)]
        [Column("rubbish_description")]
        public String RubbishDescription { get; set; }

        [MaxLength(10)]
        [Required]
        [Column("unit")]
        public String Unit { get; set; }

        /// <summary>
        /// 数量。
        /// </summary>
        [Required]
        [Column("quantity")]
        public Single Quantity { get; set; }

        /// <summary>
        /// 照片。
        /// </summary>
        [Column("photo")]
        public Byte[] Photo { get; set; }

        /// <summary>
        /// 地址（省市区）。
        /// </summary>
        [MaxLength(255)]
        [Column("address")]
        [Required(AllowEmptyStrings = false)]
        public String Address { get; set; }

        /// <summary>
        /// 详细地址。
        /// </summary>
        [MaxLength(255)]
        [Column("detail_address")]
        [Required(AllowEmptyStrings = false)]
        public String DetailAddress { get; set; }

        /// <summary>
        /// 发布日期。
        /// </summary>
        [Column("timestamp")]
        [Required]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 期望交易日期。
        /// </summary>
        [Column("except_trade_date")]
        [Required]
        public DateTime ExceptTradeDate { get; set; }

        /// <summary>
        /// 订单状态。
        /// </summary>
        [Column("state")]
        [Required]
        public OrderState State { get; set; }

        /// <summary>
        /// 状态描述（NotPass对应原因；Trading对应交易确认码）。
        /// </summary>
        [MaxLength(255)]
        [Column("state_description")]
        public String StateDescription { get; set; }

        /// <summary>
        /// 卖家的综合评分。
        /// </summary>
        [Column("comprehensive_score")]
        public Single ComprehensiveScore { get; set; }

        /// <summary>
        /// 竞价列表。
        /// </summary>
        public virtual ICollection<Auction> Auctions { get; set; }

        public OrderView ToView()
        {
            OrderView view = new OrderView();
            view.OrderId = this.Id;
            view.Type = this.Type;
            view.RubbishDescription = this.RubbishDescription;
            view.Unit = this.Unit;
            view.Quantity = this.Quantity;
            view.Photo = this.Photo;
            view.Timestamp = this.Timestamp;
            view.ComprehensiveScore = this.ComprehensiveScore;
            return view;
        }
    }
}
