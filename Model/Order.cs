namespace ProductTask.Model
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerId { get; set; }

        // Navigation property
        public List<OrderDetailes>? OrderDetails { get; set; }
    }
}
