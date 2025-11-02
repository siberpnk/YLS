using System.ComponentModel.DataAnnotations.Schema;

namespace YourLocalShopMVC.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Column ("OrderItems")]
        public List<Item>? OrderItems { get; set; }
        [Column ("Purchaser")]
        public CustomerAccount? Purchaser { get; set; }
        [Column ("DeliveryAddress")]
        public string? DeliveryAddress { get; set; }
    }
}
