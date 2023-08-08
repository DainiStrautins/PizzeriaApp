using PizzeriaApp.Models;

namespace PizzeriaApp.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Product>? Pizza { get; set; }
        public IEnumerable<Product>? Snacks { get; set; }

    }
}
