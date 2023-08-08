using PizzeriaApp.Models;

namespace PizzeriaApp.ViewModels
{
    public class ProductPaginationViewModel
    {
        public int PageIndex { get; set; } = 1;
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 9;
        public List<Product> Products { get; set; }
        public IEnumerable<Badge>? Badges { get; set; }
    }
}
