using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using YourLocalShopMVC.Models;

namespace YourLocalShopMVC.DataInventory
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
            var converter = new ValueConverter<IEnumerable<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
        }

        public DbSet<Item> Item { get; set; } = default!;
        public DbSet<ShoppingCart> ShoppingCart { get; set; } = default!;
        public DbSet<Order> Order { get; set; } = default!;
        public DbSet<PaymentDetails> PaymentDetails { get; set; } = default!;
    }
}
