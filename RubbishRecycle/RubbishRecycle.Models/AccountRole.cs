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
    /// 账户角色关联。
    /// </summary>
    public  class AccountRole
    {
        /// <summary>
        /// 账户角色关联表Id号。
        /// </summary>
        [Key]
        public Int32 Id { get; set; }

        /// <summary>
        /// 账户表Id号。
        /// </summary>
        [Required]
        public Int32 AccountId { get; set; }

        /// <summary>
        /// 角色Id号。
        /// </summary>
        [MaxLength(20)]
        [Required(AllowEmptyStrings = false)]
        public String RoleId { get; set; }

        [ForeignKey("AccountId")]
        /// <summary>
        /// 关联的账户。
        /// </summary>
        public virtual Account Account { get; set; }

        /// <summary>
        /// 关联的角色。
        /// </summary>
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
