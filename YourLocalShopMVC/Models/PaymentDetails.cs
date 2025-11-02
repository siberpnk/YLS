namespace YourLocalShopMVC.Models
{
    public class PaymentDetails
    {
        public int Id { get; set; }
        public required String CardHoldersName { get; set; }
        public required String CreditCardHash { get; set; }
        public required DateOnly ExpiryDate { get; set; }
    }
}
