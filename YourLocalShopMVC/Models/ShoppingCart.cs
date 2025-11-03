using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YourLocalShopMVC.Controllers;

namespace YourLocalShopMVC.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        //[NotMapped]
        //public List<Item> Contents { get; set; }

        [NotMapped]
        public Dictionary<Item, int> Contents { get; set; }

        [Column("ItemKeys")]
        public List<int>? ItemKeys { get; set; }
      
        [DataType(DataType.Currency), Column(name: "TotalCost", TypeName = "decimal(18, 2)")]
        public decimal TotalCost { get; set; }

        public ShoppingCart()
        {
            ItemKeys = new List<int>();
            Contents = new Dictionary<Item, int>();
        }

        public void AddToContents (Item item)
        {
            int count;
            if (Contents.TryGetValue(item, out count))
            {
                Contents[item]++;
            }
            else
            {
                Contents.Add(item, 1);
            }
        }

        public void RemoveItem(Item item)
        {
            ItemKeys.Remove(item.Id);
            Contents.Remove(item);

            if (TotalCost > 0)
                TotalCost -= item.Price;
        }

        public void AddItem(Item item)
        {
            ItemKeys?.Add(item.Id);
            TotalCost += item.Price;

            AddToContents(item);
        }

        public void SeedContents(ShoppingCart cartOther)
        {
            if(this != cartOther)
            {

            }
        }
    }
}
