using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YourLocalShopMVC.Models;
using YourLocalShopMVC.Data;

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

        public async Task<IActionResult> AddToCart(int id, Item item)
        {
            //var item = await _shopContext.Item.FindAsync(itemId);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            //item.Name = "Smiths";
            var _item = item;
            user.Cart.Contents.Add(_item);
            //_shopContext.Update(user.Cart);
            //await _userManager.UpdateAsync(user);
            //await _accountsContext.SaveChangesAsync();
            await _shopContext.SaveChangesAsync();
            return View(user.Cart.Contents);
        }

        public async Task<IActionResult> ViewCart()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(user.Cart.Contents);
        }
    }
}
