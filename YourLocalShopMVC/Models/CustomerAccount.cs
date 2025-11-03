using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using YourLocalShopMVC.DataInventory;

namespace YourLocalShopMVC.Models
{
    public class CustomerAccount : IdentityUser 
    {
        public string? DeliveryAddress { get; set; }
        
        [Column ("UserPaymentDetails")]
        public PaymentDetails? PaymentDetails { get; set; }
        
        [Column ("UserOrder")]
        public List<int>? OrderIds { get; set; }

        [NotMapped]
        public List<Order> Orders { get; set; }

        [Column ("UserCartId")]
        public int? CartId { get; set; }

        //[NotMapped]
        //public ShoppingCart? Cart { get; set; }

        public CustomerAccount()
        {
            OrderIds = new List<int>();
            Orders = new List<Order>();
            //Cart = new ShoppingCart();
        }
    }
}
