using System.ComponentModel.DataAnnotations;

namespace Products.API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string BrandName { get; set; }
        [Required]
        public string Name { get; set; }
        public string? DescriptionText { get; set; }
        [Required]
        public List<Article> Articles { get; set; }
    }
}
