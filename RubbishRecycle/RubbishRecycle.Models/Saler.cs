using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    [Table("saler")]
    public class Saler : Account
    {
        public override String RoleId
        {
            get
            {
                return Account.Saler;
            }
        }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
