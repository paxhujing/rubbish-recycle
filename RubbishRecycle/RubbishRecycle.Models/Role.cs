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
    /// 角色。
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 角色名称。
        /// </summary>
        [Key]
        [MaxLength(20)]
        public String RoleName { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        [MaxLength(30)]
        public String Description { get; set; }
    }
}
