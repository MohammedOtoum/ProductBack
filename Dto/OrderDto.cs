namespace ProductTask.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string? CustomerId { get; set; }

        public List<OrderDetailDto>? OrderDetails { get; set; }
    }
}
