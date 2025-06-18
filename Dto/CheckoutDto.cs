using ProductTask.Model;

namespace ProductTask.Dto
{
    public class CheckoutDto
    {
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
        public int OrderId { get; set; }

        // Make this nullable:
        public CardInfo? CardInfo { get; set; }
    }
}
