using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    [Table("account")]
    public class Account
    {
        [Key]
        [MaxLength(36)]
        [Column("id")]
        public String Id { get; set; }

        [MaxLength(20)]
        [Required(AllowEmptyStrings = false)]
        [Column("name")]
        public String Name { get; set; }

        [MaxLength(20)]
        [Required(AllowEmptyStrings = false)]
        [Column("password")]
        public String Password { get; set; }

        [MaxLength(10)]
        [Column("role")]
        public String Role { get; set; }

        [Column("last_login")]
        public DateTime LastLogin { get; set; }

        [Column("credit")]
        public Int32 Credit { get; set; }
    }
}
