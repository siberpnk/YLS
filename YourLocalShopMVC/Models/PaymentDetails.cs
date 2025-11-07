using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using YourLocalShopMVC.Validation;

namespace YourLocalShopMVC.Models
{
    public class PaymentDetails
    {
        public int Id { get; set; }
       
        [Column ("CardHolderName")]
        [NotRequiredIf(nameof(CreditCardHash))]
        [Display (Name = "Card Holder Name")]
        public required string CardHoldersName { get; set; }
      
        [Column ("CreditCardHash")]
        [HiddenInput]
        public string? CreditCardHash { get; protected set; }
      
        [Column ("ExpirayDate")]
        [DataType(DataType.Date)]
        [Required]
        [Display (Name = "Expiry Date")]
        public required DateOnly ExpiryDate { get; set; }

        [NotMapped]
        [Required]
        [StringLength(4*4)]
        [RegularExpression(@"\d{16}", ErrorMessage = "Credit Card Number must be 16 digits.")]
        [Display (Name = "Card Number")]
        public required string CardNumber { get; set; }

        [NotMapped]
        [Required]
        [RegularExpression(@"\d..", ErrorMessage = "Please enter a valid 3 digit CVC number.")]
        public required int CVC { get; set; }

        public void CreateCreditCardHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                CreditCardHash = GetHash(sha256, CardHoldersName + ExpiryDate + CVC);
            }
        }

        public bool CheckValidHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                if (CreditCardHash != null)
                    return VerifyHash(sha256, CardHoldersName + ExpiryDate + CVC, CreditCardHash);
                else
                    return false;
            }
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            // Hash the input.
            var hashOfInput = GetHash(hashAlgorithm, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}
