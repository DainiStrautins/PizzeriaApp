using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PizzeriaApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzeriaApp.ViewModels
{
    public class ViewCustomProduct
    {
        public IEnumerable<Product>? Dough { get; set; }
        public IEnumerable<Product>? Sauce { get; set; }
        public IEnumerable<Product>? Cheese { get; set; }
        public IEnumerable<Product>? Toppings { get; set; }
        [Column(TypeName = "jsonb")]
        public string? Recipe { get; set; }
    }
}
