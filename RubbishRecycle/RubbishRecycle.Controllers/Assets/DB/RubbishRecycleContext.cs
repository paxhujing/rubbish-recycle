using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Controllers.Assets.DB
{
    public class RubbishRecycleContext : DbContext
    {
        #region Constructors
        
        public RubbishRecycleContext()
            : base("name=RubbishRecycleContext")
        {
        }

        #endregion

        #region Properties

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Order> Orders { get; set; }

        #endregion
    }
}
