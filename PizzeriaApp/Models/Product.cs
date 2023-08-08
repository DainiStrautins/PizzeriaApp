namespace PizzeriaApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public byte[] Image { get; set; }
        public Category Category { get; set; }
        public int? CategoryId { get; set; }

        public Badge? Badge { get; set; }
        public int? BadgeId { get; set; }
        public double Multiplier { get; set; }
    }
}
