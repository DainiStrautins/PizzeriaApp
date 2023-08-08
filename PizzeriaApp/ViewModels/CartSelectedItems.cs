using PizzeriaApp.Models;
using static PizzeriaApp.Controllers.HomeController;

namespace PizzeriaApp.ViewModels
{
    public class CartSelectedItems
    {
        public OrderDetails? orderDetails { get; set; }
        public IEnumerable<SelectedItem>? SelectedItems { get; set; }
    }
}
