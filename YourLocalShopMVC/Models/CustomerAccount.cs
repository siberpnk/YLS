using Microsoft.AspNetCore.Identity;
using System.Security.Policy;

namespace YourLocalShopMVC.Models
{
    public class CustomerAccount : IdentityUser 
    {
        public string? DeliveryAddress { get; set; }
        public PaymentDetails? PaymentDetails { get; set; }
        public List<Order>? Orders { get; set; }
        public ShoppingCart? Cart { get; set; }

        public CustomerAccount()
        {
            Orders = new List<Order>();
            Cart = new ShoppingCart();
        }
    }
}
