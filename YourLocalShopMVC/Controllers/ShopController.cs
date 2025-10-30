using Microsoft.AspNetCore.Mvc;

namespace YourLocalShopMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly Data.ShopInventoryContext _context;

        public IActionResult AddToCart()
        {
            return View("Items");
        }
    }
}
