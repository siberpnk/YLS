using System.ComponentModel.DataAnnotations;
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

        [Required]
        [Column ("ItemIds")]
        public List<int>? ItemIds { get; set; }

        [Required]
        [ForeignKey("PK_AspNetUsers")]
        [Column ("PurchaserId")]
        public string? PurchaserId { get; set; }
        
        [Required]
        [Column ("DeliveryAddress")]
        public string? DeliveryAddress { get; set; }

        [Required]
        [DataType(DataType.Currency), Column(name: "TotalPrice", TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Column ("PurchaseTime")]
        [DataType(DataType.DateTime)]
        public DateTime PurchaseTime { get; set; }

        public Order()
        {
            ItemIds = new List<int>();
            OrderedItems = new List<Item>();
            Customer = new CustomerAccount();
            PurchaseTime = DateTime.Now;
        }

        public void BuildOrder(CustomerAccount customer, ShoppingCart cart)
        {
            PurchaserId = customer.Id;
            ItemIds = cart.ItemKeys;
            DeliveryAddress = customer.DeliveryAddress;
            TotalPrice = cart.TotalCost;
        }
    }
}
