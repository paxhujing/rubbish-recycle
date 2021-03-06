﻿using RubbishRecycle.Models.ViewModels;
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
    public abstract class Account : IConvertView<AccountView>
    {
        #region Fields

        public static readonly String Saler = "saler";

        public static readonly String Buyer = "buyer";

        #endregion

        #region Constructors

        protected Account()
        {

        }

        #endregion

        #region Properties

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
        [Column("membership_point")]
        public Single MembershipPoint { get; set; }

        /// <summary>
        /// 最近登录日期。
        /// </summary>
        [Column("last_login")]
        public DateTime LastLogin { get; set; }

        /// <summary>
        /// 信誉评分。
        /// </summary>
        [Column("credit_score")]
        public Single CreditScore { get; set; }

        /// <summary>
        /// 态度评分。
        /// </summary>
        [Column("attitude_score")]
        public Single AttitudeScore { get; set; }

        [Column("is_feezed")]
        public Boolean IsFreezed { get; set; }

        /// <summary>
        /// 角色Id。
        /// </summary>
        [StringLength(20)]
        [Column("role_id")]
        public abstract String RoleId { get; }

        #endregion

        #region Methods

        public AccountView ToView()
        {
            AccountView viewer = new AccountView();
            viewer.Auction = this.MembershipPoint;
            viewer.BindingPhone = this.BindingPhone;
            viewer.CreditScore = this.CreditScore;
            viewer.AttitudeScore = this.AttitudeScore;
            viewer.IsFreezed = this.IsFreezed;
            viewer.LastLogin = this.LastLogin.ToString("yyyy-MM-dd hh:mm:ss");
            viewer.Name = this.Name;
            return viewer;
        }

        #endregion
    }
}
