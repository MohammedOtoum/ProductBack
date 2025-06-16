
namespace ProductTask.Dto
{
    public class ProductDto
    {
        public string? Name { get; set; }
        public decimal price { get; set; }
        public string? Description { get; set; }
        public IFormFile? ImageURL { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName
        {
            get
            {
                return CategoryId switch
                {
                    1 => "Electronics",
                    2 => "Home & Kitchen",
                    3 => "Clothing",
                    4 => "Beauty & Personal Care",
                    5 => "Digital Products",
                    _ => " ",
                };
            }
        }
    }
}
