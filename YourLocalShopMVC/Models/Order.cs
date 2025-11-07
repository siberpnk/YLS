using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourLocalShopMVC.Models
{
    public class Order
    {
        [Display(Name = "Order Number")]
        public int Id { get; set; }

        [NotMapped]
        [Display(Name = "Items")]
        public Dictionary<Item, int> OrderedItems { get; set; }

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
        [Display(Name = "Delivery Address")]
        public string? DeliveryAddress { get; set; }

        [Required]
        [Display(Name = "Total Price")]
        [DataType(DataType.Currency), Column(name: "TotalPrice", TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [Column ("PurchaseTime")]
        [Display(Name = "Purchased On")]
        [DataType(DataType.DateTime)]
        public DateTime PurchaseTime { get; set; }

        public Order()
        {
            ItemIds = new List<int>();
            OrderedItems = new Dictionary<Item, int>();
            Customer = new CustomerAccount();
            PurchaseTime = DateTime.Now;
        }

        public void BuildOrder(CustomerAccount customer, ShoppingCart cart)
        {
            Customer = customer;
            PurchaserId = customer.Id;
            ItemIds = cart.ItemKeys;
            DeliveryAddress = customer.DeliveryAddress;
            TotalPrice = cart.TotalCost;
            OrderedItems = cart.Contents;
        }
        
        public void AddToContents(Item item)
        {
            int count;
            if (OrderedItems.TryGetValue(item, out count))
            {
                OrderedItems[item]++;
            }
            else
            {
                OrderedItems.Add(item, 1);
            }
        }
    }
}
