using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YourLocalShopMVC.Models;
using YourLocalShopMVC.Data.Accounts;
using YourLocalShopMVC.DataInventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Policy;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

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

            _accountsContext.CustomerAccount.Include(c => c.PaymentDetails);
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

        private async Task<CustomerAccount?> GetCurrentUser()
        {
                return await _userManager.GetUserAsync(HttpContext.User);
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

        [Authorize]
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

            return View(new ShoppingCart());
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

        public async Task<IActionResult> Payment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Payment(CustomerAccount customer)
        {
            if(customer == null)
            {
                return NotFound(customer);
            }

            if(ModelState.IsValid)
            {
                var cart = await FindCart();
                var user = await _userManager.GetUserAsync(HttpContext.User);
                customer.PaymentDetails.CreateCreditCardHash();


                if(cart == null)
                {
                    return NotFound(cart);
                }

                if(user == null)
                {
                    return NotFound(user);
                }

                user.PaymentDetails = customer.PaymentDetails;

                _accountsContext.Update(user);
                await _accountsContext.SaveChangesAsync();
                return RedirectToAction(nameof(Checkout));
            }
            return View(customer);
        }

        public async Task<IActionResult> Checkout()
        {
            var cart = await FindCart();
            if (cart == null)
            {
                NotFound(cart);
            }
            if (ModelState.IsValid)
            {

                var user = await GetCurrentUser();
                var order = new Order();
                order.BuildOrder(user, cart);
                order.Customer = user;
                _shopContext.Update(cart);
                user.CartId = null;

                foreach(var item in cart.Contents)
                {
                    _shopContext.Find<Item>(item.Key.Id).Stock -= item.Value;
                }


                _accountsContext.Update(user);
                _shopContext.Add(order);

                _accountsContext.SaveChanges();
                _shopContext.SaveChanges();

            return RedirectToAction(nameof(Order), order);
            }
            return View();
        }

        public async Task<IActionResult> Order(int id)
        {
            var order = _shopContext.Order.Where(x => x.Id == id).First();
            //reseed contents based on CartId
            foreach (int itemId in order.ItemIds)
            {
                order.AddToContents(await _shopContext.Item.FindAsync(itemId));
            }
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Order()
        {
            //Order order = _shopContext.Order.ToList().Where<CustomerAccount>(x => x.PurchaserId);
            var user = GetCurrentUser();
            var id = user.Result.Id;
            var order = _shopContext.Order.ToList().Where(x => x.PurchaserId == id).Last();
            //order.OrderedItems = null;


            //reseed contents based on CartId
            foreach (int itemId in order.ItemIds)
            {
                order.AddToContents(await _shopContext.Item.FindAsync(itemId));
            }

            return View(order);
        }

        [Authorize]
        public async Task<IActionResult> Orders()
        {
            if (HttpContext.User.IsInRole("Staff"))
            {
                var orders = _shopContext.Order.ToList();
                return View(orders);
            }
            else
            {
                var user = await GetCurrentUser();
                if (user == null)
                {
                    NotFound(user);
                }

                var orders = _shopContext.Order.ToList().Where(x => x.PurchaserId == user.Id);
                return View(orders);
            }
            return View();
        }
    }
}
