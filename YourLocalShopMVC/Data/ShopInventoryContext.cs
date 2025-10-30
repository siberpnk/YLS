using Microsoft.EntityFrameworkCore;

namespace YourLocalShopMVC.Data
{

    public class ShopInventoryContext : DbContext
    {
        public ShopInventoryContext(DbContextOptions<ShopInventoryContext> options)
            : base(options)
        {
        }
        public DbSet<YourLocalShopMVC.Models.Item> Item { get; set; } = default!;
    }
}
