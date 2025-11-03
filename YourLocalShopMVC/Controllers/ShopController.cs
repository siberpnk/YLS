using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YourLocalShopMVC.Models;
using YourLocalShopMVC.Data;
using YourLocalShopMVC.Data.Accounts;
using YourLocalShopMVC.DataInventory;

namespace YourLocalShopMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopInventoryContext _shopContext;
        private readonly AccountsDbContext _accountsContext;
        private readonly UserManager<CustomerAccount> _userManager;

        public ShopController(ShopInventoryContext shopContext, AccountsDbContext accountsContext, UserManager<CustomerAccount> userManager)
        {
            _accountsContext = accountsContext;
            _shopContext = shopContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddToCart(int id)
        {
           //cart relates to a found user through Find(userId) in _shopContext 
            var cart = _shopContext.Find<ShoppingCart>(_userManager.GetUserAsync(HttpContext.User).Result.CartId);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid)
            {
                cart.ItemKeys.Add(id);

                if (user != null)
                {
                    await _userManager.UpdateAsync(user);
                }
                await _accountsContext.SaveChangesAsync();

                _accountsContext.Update(user);
                await _accountsContext.SaveChangesAsync();
                _shopContext.Add(cart);
                await _shopContext.SaveChangesAsync();
                return View(_accountsContext);
            }
            return View();
        }

        public async Task<IActionResult> ViewCart()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ShoppingCart cart = new ShoppingCart();
            if(user != null)
            {
                cart = _shopContext.Find<ShoppingCart>(user.CartId);
                user.Cart = cart;
            }
            if(user != null && user.Cart != null)
            {
                return View(user.Cart);
            }

            return View(cart);
        }
    }
}
