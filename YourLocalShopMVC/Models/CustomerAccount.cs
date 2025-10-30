using Microsoft.AspNetCore.Identity;
using System.Security.Policy;

namespace YourLocalShopMVC.Models
{
    public class CustomerAccount : IdentityUser
    {
        public string? DeliveryAddress { get; set; }
        public Hash? CreditCardHash { get; set; }
        public List<Order>? Orders { get; set; }

    }
}
