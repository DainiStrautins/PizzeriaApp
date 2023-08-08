using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaApp.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public Product? Products { get; set; }
        public int? ProductsId { get; set; }
        public Size? Size { get; set; }
        public int? SizeId { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        [Column(TypeName = "jsonb")]
        public string? Recipe { get; set; }
    }
}
