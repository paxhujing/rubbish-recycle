using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Controllers.Assets
{
    public class RubbishRecycleContext : DbContext
    {
        #region Constructors

        public RubbishRecycleContext()
            : base("name=RubbishRecycleContext")
        {
            Database.SetInitializer(new RubbishRecycleDBInitializer());
        }

        #endregion

        #region Nested class

        private class RubbishRecycleDBInitializer : DropCreateDatabaseAlways<RubbishRecycleContext>
        {
            protected override void Seed(RubbishRecycleContext context)
            {
                context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_account_name on account (name)");
                context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_account_phone on account (binding_phone)");
                context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_account_roleid on account (role_id)");

                Role saler = new Role();
                saler.Id = "saler";
                saler.Description = "卖家";
                context.Roles.Add(saler);

                Role buyer = new Role();
                buyer.Id = "buyer";
                buyer.Description = "买家";
                context.Roles.Add(buyer);

                context.SaveChanges();
            }
        }

        #endregion

        #region Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Properties

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Role> Roles { get; set; }

        #endregion
    }
}
