using PizzeriaApp.Models;
using static PizzeriaApp.Controllers.HomeController;

namespace PizzeriaApp.ViewModels
{
    public class CartCustomPizza
    {
        public UserOrders? UserOrders { get; set; }
        public IEnumerable<CartSelectedItems>? cartSelectedItems { get; set; }
    }
}
