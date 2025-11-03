using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using YourLocalShopMVC.Data;
using YourLocalShopMVC.Data.Accounts;
using YourLocalShopMVC.Models;

namespace YourLocalShopMVC.Controllers
{
    //[Authorize(Roles = "Staff")]
    public class AccountsController : Controller
    {
        private readonly AccountsDbContext _accountsContext;
        private readonly UserManager<CustomerAccount> _userManager;

        public AccountsController(AccountsDbContext accountsContext, UserManager<CustomerAccount> userManager)
        {
            _accountsContext = accountsContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string email)
        {

            if (email == null)
                return NotFound(email);

            var user = _userManager.FindByEmailAsync(email).Result;

            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Staff");
                await _userManager.UpdateAsync(user);
                _accountsContext.SaveChanges();
                return View(user);
            }

            return NotFound(user);
        }
    }
}
