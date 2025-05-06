using System.ComponentModel.DataAnnotations;

namespace Products.API.Models
{
    public class Article
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string ShortDescription { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Unit { get; set; }
        [Required]
        public string PricePerUnitText { get; set; }
        [Required]
        public string Image { get; set; }
    }
}
