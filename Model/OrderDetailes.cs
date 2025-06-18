using System.Text.Json.Serialization;

namespace ProductTask.Model
{
    public class OrderDetailes
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        [JsonIgnore] 
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
