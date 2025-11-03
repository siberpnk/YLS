using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YourLocalShopMVC.Models;
using YourLocalShopMVC.Data.Accounts;
using YourLocalShopMVC.DataInventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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

        private async Task<ShoppingCart?> FindCart()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ShoppingCart cart;

            if (user == null)
            {
                return null;
            }

            if (ModelState.IsValid)
            {

                //if user has an associated cart dig up cart by its primary key
                if (user.CartId != null)
                {
                    //reseed contents based on CartId
                    cart = _shopContext.Find<ShoppingCart>(user.CartId);
                    foreach (int itemId in cart.ItemKeys)
                    {
                        cart.AddToContents(await _shopContext.Item.FindAsync(itemId));
                    }
                }
                else
                {
                    cart = new ShoppingCart();
                    _shopContext.Add(cart);
                    await _shopContext.SaveChangesAsync();
                }
                return cart;
            }
            return null;
        }

        public async Task<IActionResult> AddToCart(int id)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(user == null)
            {
                return NotFound(user);
            }

            if (ModelState.IsValid)
            {
                ShoppingCart cart;

                //if user has an associated cart dig up cart by its primary key
                if (user.CartId != null)
                {
                    //reseed contents
                    cart = _shopContext.Find<ShoppingCart>(user.CartId);
                    foreach(int itemId in cart.ItemKeys)
                    {
                        cart.AddToContents(await _shopContext.Item.FindAsync(itemId));
                    }
                }
                //else create an empty cart
                else
                {
                    cart = new ShoppingCart();
                    _shopContext.Add(cart);
                    await _shopContext.SaveChangesAsync();
                }

                //add item by it's key to the cart
                cart.AddItem(_shopContext.Find<Item>(id));
                //cart.CalculateTotal();

                //update shop context then sane the changes.
                _shopContext.Update(cart);
                await _shopContext.SaveChangesAsync();

                user.CartId = cart.Id;
                //update the user account and commit the change to the database
                _accountsContext.Update(user);
                await _accountsContext.SaveChangesAsync();

                return RedirectToAction(nameof(ViewCart));
            }
            return View();
        }

        public async Task<IActionResult> ViewCart()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(user == null)
            {
                return NotFound(user);
            }

            if (user.CartId != null)
            {
                //reseed contents
                var cart = _shopContext.Find<ShoppingCart>(user.CartId);
                foreach (int itemId in cart.ItemKeys)
                {
                    cart.AddToContents(await _shopContext.Item.FindAsync(itemId));
                }

                return View(cart);
            }

            return View();
        }

        public async Task<IActionResult> RemoveItem(int id)
        {
            var cart = await FindCart();
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(cart == null)
            {
                NotFound(cart);
            }
            cart.RemoveItem(_shopContext.Find<Item>(id));

            _shopContext.Update(cart);
            await _shopContext.SaveChangesAsync();


            return RedirectToAction(nameof(ViewCart));
        }

        public async Task<IActionResult> Checkout(int id)
        {
            var cart = await FindCart();
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if(cart == null)
            {
                return NotFound(cart);
            }

            if(user == null)
            {
                return NotFound(user);
            }



            return View();
        }
    }
}
