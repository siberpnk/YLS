using System.ComponentModel.DataAnnotations.Schema;

namespace YourLocalShopMVC.Models
{
    public class Order
    {
        public int Id { get; set; }

        [NotMapped]
        //TODO: Change to Dictionary
        public List<Item> OrderedItems { get; set; }

        [NotMapped]
        public CustomerAccount Customer { get; set; }

        [Column ("ItemIds")]
        public List<int>? ItemIds { get; set; }
        
        [Column ("PurchaserId")]
        public int? PurchaserId { get; set; }
        
        [Column ("DeliveryAddress")]
        public string? DeliveryAddress { get; set; }

        public Order()
        {
            OrderedItems = new List<Item>();
            Customer = new CustomerAccount();
        }
    }
}
