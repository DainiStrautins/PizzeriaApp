using PizzeriaApp.Models;
using static PizzeriaApp.Controllers.HomeController;

namespace PizzeriaApp.ViewModels
{
    public class OrderDetailsViewModel
    {

        public IEnumerable<OrderDetails>? orderDetails { get; set; }
        public IEnumerable<CustomOrder>? orderDetailsCustom { get; set; }
    }
    public class CustomOrder
    {
        public OrderDetails? OrderDetails { get; set; }
        public IEnumerable<CartSelectedItems>? cartSelectedItems { get; set; }
    }
}
