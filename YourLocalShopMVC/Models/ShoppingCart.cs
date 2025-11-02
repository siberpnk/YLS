namespace YourLocalShopMVC.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public List<Item>? Contents { get; set; }   
        public double TotalCost { get; set; }
    }
}
