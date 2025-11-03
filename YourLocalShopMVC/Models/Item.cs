using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourLocalShopMVC.Models
{
    public class Item
    {
        public int Id { get; set; }
        
        [Column("Name")]
        public string? Name { get; set; }
        
        [DataType(DataType.Currency), Column(TypeName = "decimal(18, 2)")]
        public required decimal Price { set; get; }
        
        [Column("Stock")]
        public int? Stock { set; get; } 
    }
}
