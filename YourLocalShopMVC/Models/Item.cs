using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourLocalShopMVC.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Column("ItemName")]
        public required string Name { get; set; }
        [DataType(DataType.Currency), Column(name: "Price", TypeName = "decimal(18, 2)")]
        public required decimal Price { set; get; }
        [Column("Stock")]
        public int? Stock { set; get; } 
    }
}
