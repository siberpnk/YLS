using Microsoft.EntityFrameworkCore;

namespace YourLocalShopMVC.Data
{

    public class ShopInventoryContext : DbContext
    {
        public ShopInventoryContext(DbContextOptions<ShopInventoryContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Models.Item> Item { get; set; } = default!;
        public DbSet<Models.ShoppingCart> ShoppingCart { get; set; } = default!;
        public DbSet<Models.Order> Order { get; set; } = default!;
        public DbSet<Models.PaymentDetails> PaymentDetails { get; set; } = default!;
    }
}
