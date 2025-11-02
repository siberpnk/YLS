using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourLocalShopMVC.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        [NotMapped]
        public List<Item> Contents { get; set; }

        [Column ("ItemKeys")]
        public List<int>? ItemKeys { get; set; }
      
        [DataType(DataType.Currency), Column(name: "TotalCost", TypeName = "decimal(18, 2)")]
        public double TotalCost { get; set; }

        public ShoppingCart()
        {
            ItemKeys = new List<int>();
            Contents = new List<Item>();
        }

        public void Dto(int i)
        {
            //Contents.Add(null);
        }
    }
}
