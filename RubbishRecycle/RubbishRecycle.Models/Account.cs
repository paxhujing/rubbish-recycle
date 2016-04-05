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
    /// 账户。
    /// </summary>
    public class Account
    {
        /// <summary>
        /// 账户表Id号。
        /// </summary>
        [Key]
        public Int32 Id { get; set; }

        /// <summary>
        /// 账户名称。
        /// </summary>
        [MaxLength(20)]
        [Required(AllowEmptyStrings = false)]
        public String Name { get; set; }

        /// <summary>
        /// 密码。
        /// </summary>
        [MaxLength(20)]
        [Required(AllowEmptyStrings = false)]
        public String Password { get; set; }

        /// <summary>
        /// 绑定的手机号。
        /// </summary>
        [MaxLength(13)]
        [Required(AllowEmptyStrings = false)]
        public String BindingPhone { get; set; }

        /// <summary>
        /// 最近登录日期。
        /// </summary>
        public DateTime LastLogin { get; set; }

        /// <summary>
        /// 信誉。
        /// </summary>
        public Int32 Credit { get; set; }

        /// <summary>
        /// 账户角色关联列表。
        /// </summary>
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
    }
}
