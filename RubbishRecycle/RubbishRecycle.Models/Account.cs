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
        public Int32 Id { get; set; }

        [MaxLength(20)]
        [Required(AllowEmptyStrings = false)]
        public String Name { get; set; }

        [MaxLength(20)]
        [Required(AllowEmptyStrings = false)]
        public String Password { get; set; }

        public AccountType Type { get; set; }

        public DateTime LastLogin { get; set; }
    }
}
