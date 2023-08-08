namespace PizzeriaApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category? Parent { get; set; }
        public int? ParentId { get; set; }
    }
}
