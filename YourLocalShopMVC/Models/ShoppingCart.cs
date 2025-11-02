using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourLocalShopMVC.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        [Column ("Contents")]
        public List<Item>? Contents { get; set; }
        [DataType(DataType.Currency), Column(name: "TotalCost", TypeName = "decimal(18, 2)")]
        public double TotalCost { get; set; }

        public ShoppingCart()
        {
            Contents = new List<Item>();
        }
    }
}
