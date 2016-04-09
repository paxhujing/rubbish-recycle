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
    [Table("role")]
    public class Role
    {
        /// <summary>
        /// 角色名称。
        /// </summary>
        [Key]
        [StringLength(20)]
        [Column("id")]
        public String Id { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        [StringLength(30)]
        [Column("description")]
        public String Description { get; set; }
    }
}
