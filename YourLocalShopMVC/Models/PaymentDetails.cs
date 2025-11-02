using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourLocalShopMVC.Models
{
    public class PaymentDetails
    {
        public int Id { get; set; }
        [Column ("CardHolderName")]
        [Required]
        public required String CardHoldersName { get; set; }
        [Column ("CreditCardHash")]
        public required String CreditCardHash { get; set; }
        [Column ("ExpirayDate")]
        [DataType(DataType.Date)]
        [Required]
        public required DateOnly ExpiryDate { get; set; }
    }
}
