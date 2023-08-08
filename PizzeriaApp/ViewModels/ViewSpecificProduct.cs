using PizzeriaApp.Models;

namespace PizzeriaApp.ViewModels
{
    public class ViewSpecificProduct
    {
        public Product? Product { get; set; }
        public IEnumerable<Size>? Size { get; set; }

        public OrderDetails? OrderDetails { get; set; }
    }
}
