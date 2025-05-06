namespace Products.API.DTO
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string ShortDescription { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public string PricePerUnitText { get; set; }
        public string Image { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
