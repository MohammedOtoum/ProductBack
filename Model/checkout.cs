// Models/CheckoutForm.cs
using System.ComponentModel.DataAnnotations;

namespace ProductTask.Model
{
    public class CheckoutForm
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? PostalCode { get; set; }
        public string? Notes { get; set; }
        public string? PaymentMethod { get; set; }

        // Make this nullable:
        public CardInfo? CardInfo { get; set; }
    }

    public class CardInfo
    {
        public string? CardHolderName { get; set; }
        public string? CardNumber { get; set; }
        public string? ExpirationDate { get; set; }
        public string? CVV { get; set; }
    }

}