using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    [Table("buyer")]
    public class Buyer : Account
    {
        public override String RoleId
        {
            get
            {
                return Account.Buyer;
            }
        }

        public virtual ICollection<OrderSummary> OrderSummaries { get; set; }
    }
}
