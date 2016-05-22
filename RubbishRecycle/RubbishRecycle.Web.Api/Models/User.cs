using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Web.Api.Models
{
    /// <summary>
    /// 用户数据模型。
    /// </summary>
    public class User
    {
        /// <summary>
        /// 账户表Id。
        /// </summary>
        public String Id { get; set; }
        /// <summary>
        /// 绑定的手机号。
        /// </summary>
        public String Phone { get; set; }
        /// <summary>
        /// 积分。
        /// </summary>
        public Single Auction { get; set; }
        /// <summary>
        /// 最近登录日期。
        /// </summary>
        public DateTime LastLogin { get; set; }
        /// <summary>
        /// 信誉。
        /// </summary>
        public Int32 Credit { get; set; }
        /// <summary>
        /// 用户状态。
        /// </summary>
        public UserStatus Status { get; set; }
        /// <summary>
        /// 角色Id。
        /// </summary>
        public String RoleId { get; set; }
    }

    public enum UserStatus
    {
        /// <summary>
        /// 正常。
        /// </summary>
        Normal,
        /// <summary>
        /// 冻结。
        /// </summary>
        Freezed,
    }
}
