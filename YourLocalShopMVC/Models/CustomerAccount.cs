using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace YourLocalShopMVC.Models
{
    public class CustomerAccount : IdentityUser 
    {
        public string? DeliveryAddress { get; set; }
        [Column ("UserPaymentDetails")]
        public PaymentDetails? PaymentDetails { get; set; }
        [Column ("UserOrder")]
        public List<Order>? Orders { get; set; }
        [Column ("UserCart")]
        public ShoppingCart? Cart { get; set; }

        public CustomerAccount()
        {
            Orders = new List<Order>();
            Cart = new ShoppingCart();
        }
    }
}
