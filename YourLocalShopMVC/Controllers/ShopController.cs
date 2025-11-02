using Microsoft.AspNetCore.Mvc;

namespace YourLocalShopMVC.Controllers
{
    public class ShopController : Controller
    {
        private readonly Data.ShopInventoryContext _context;

        public async Task<IActionResult> AddToCart(int itemId)
        {
            return View("Items");
        }
    }
}
