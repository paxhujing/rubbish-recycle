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
#if DEBUG
            Database.SetInitializer(new RubbishRecycleDBInitializer());
#endif
        }

        #endregion

        #region Nested class
#if DEBUG
        private class RubbishRecycleDBInitializer :
#if REBUILD
            CreateDatabaseIfNotExists<RubbishRecycleContext>
#else
            DropCreateDatabaseIfModelChanges<RubbishRecycleContext>
#endif
        {
            protected override void Seed(RubbishRecycleContext context)
            {
                context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_account_name on account (name)");
                context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_account_phone on account (binding_phone)");

                Role saler = new Role();
                saler.Id = "saler";
                saler.Description = "卖家";
                context.Roles.Add(saler);

                Role buyer = new Role();
                buyer.Id = "buyer";
                buyer.Description = "买家";
                context.Roles.Add(buyer);

                AppKeyInfo testAppKey = new AppKeyInfo();
                testAppKey.AppKey = "EDF6D00C74DB486880835FD2AEE8CB71";
                testAppKey.CreateTime = DateTime.Now;
                context.AppKeyInfos.Add(testAppKey);

                context.SaveChanges();
            }
        }
#endif

#endregion

        #region Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //order_state_trace->order_id->order CASCADE
            modelBuilder.Entity<OrderStateTrace>().HasRequired(x => x.Order).WithMany(x=>x.States).HasForeignKey(x=>x.OrderId).WillCascadeOnDelete(true);
            //order->saler_id->account CASCADE
            modelBuilder.Entity<Order>().HasRequired(x => x.Saler).WithMany(x => x.Orders).HasForeignKey(x => x.SalerId).WillCascadeOnDelete(true);
            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Properties

        public DbSet<AppKeyInfo> AppKeyInfos { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderStateTrace> OrderStates { get; set; }

        #endregion
    }
}
