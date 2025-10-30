using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YourLocalShopMVC.Models;

namespace YourLocalShopMVC.Data
{
    public class AccountsDbContext : IdentityDbContext
    {
        public AccountsDbContext(DbContextOptions<AccountsDbContext> options)
            : base(options)
        {
        }
    }
}
