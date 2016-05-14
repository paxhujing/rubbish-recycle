using RubbishRecycle.Models.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RubbishRecycle.Models
{
    /// <summary>
    /// 账户。
    /// </summary>
    [Table("account")]
    public class Account : IConvertViewer<AccountViewer>
    {
        /// <summary>
        /// 账户表Id。
        /// </summary>
        [Key]
        [Column("id")]
        public String Id { get; set; }

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

        public virtual ICollection<Order> Orders { get; set; }

        public AccountViewer ToViewer()
        {
            AccountViewer viewer = new AccountViewer();
            viewer.Auction = this.Auction;
            viewer.BindingPhone = this.BindingPhone;
            viewer.Credit = this.Credit;
            viewer.IsFreezed = this.IsFreezed;
            viewer.LastLogin = this.LastLogin;
            viewer.Name = this.Name;
            return viewer;
        }
    }
}
