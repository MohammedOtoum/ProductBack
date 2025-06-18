namespace ProductTask.Dto
{
    public class ViewOrder
    {
        public int CheckoutFormId { get; set; }  

        public int OrderId { get; set; }
        public OrderDto? OrderDto { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerLocation { get; set; }
        public int? Price { get; set; }
        public string? PaymentMethod { get; set; }

    }
}
