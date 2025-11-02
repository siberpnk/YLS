namespace YourLocalShopMVC.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<Item>? OrderItems { get; set; }
        public CustomerAccount? Purchaser { get; set; }
        public string? DeliveryAddress { get; set; }
    }
}
