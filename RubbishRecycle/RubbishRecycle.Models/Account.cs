using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RubbishRecycle.Models
{
    /// <summary>
    /// 账户。
    /// </summary>
    [Table("account")]
    public class Account
    {
        /// <summary>
        /// 账户表Id。
        /// </summary>
        [Key]
        [Column("id")]
        public Int32 Id { get; set; }

        /// <summary>
        /// 账户名称。
        /// </summary>
        [StringLength(20)]
        [Required(AllowEmptyStrings = false)]
        [Column("name")]
        public String Name { get; set; }

        /// <summary>
        /// 密码。
        /// </summary>
        [StringLength(80)]
        [Required(AllowEmptyStrings = false)]
        [Column("password")]
        public String Password { get; set; }

        /// <summary>
        /// 绑定的手机号。
        /// </summary>
        [StringLength(13)]
        [Required(AllowEmptyStrings = false)]
        [Column("binding_phone")]
        public String BindingPhone { get; set; }

        /// <summary>
        /// 积分。
        /// </summary>
        [Column("auction")]
        public Single Auction { get; set; }

        /// <summary>
        /// 最近登录日期。
        /// </summary>
        [Column("last_login")]
        public DateTime LastLogin { get; set; }

        /// <summary>
        /// 信誉。
        /// </summary>
        [Column("credit")]
        public Int32 Credit { get; set; }

        /// <summary>
        /// 角色Id。
        /// </summary>
        [StringLength(20)]
        [Column("role_id")]
        public String RoleId { get; set; }

        [Column("is_feezed")]
        public Boolean IsFreezed { get; set; }
    }
}
