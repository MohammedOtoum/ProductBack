using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace ProductTask.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Description { get; set; }

        public byte[]? ImageURL { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public ProductCategories? ProductCategories { get; set; }
    }
}
