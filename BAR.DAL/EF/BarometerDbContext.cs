using BAR.BL.Domain.Data;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using System.Data.Entity;

namespace BAR.DAL.EF
{
    public class BarometerDbContext : DbContext
    {
        public BarometerDbContext() : base("BAR_DB")
        {
            Database.SetInitializer(new BarometerInitializer());
        }

        //Data package
        public DbSet<Information> Informations { get; set; }
        public DbSet<Property> Properties { get; set; }

        //User package
        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }

        //Item package
        public DbSet<Item> Items { get; set; }


    }
}
