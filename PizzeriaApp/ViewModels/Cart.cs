using PizzeriaApp.Controllers;
using PizzeriaApp.Models;
using static PizzeriaApp.Controllers.HomeController;

namespace PizzeriaApp.ViewModels
{
    public class Cart
    {
        public IEnumerable<UserOrders>? UserOrders { get; set; }
        public IEnumerable<CartCustomPizza>? CartCustomPizza { get; set; }

        public IEnumerable<DeliveryType>? DeliveryTypes { get; set; }

        public string ContactName { get; set; }
        public string? DeliveryAddress { get; set; }
        public string ContactPhone { get; set; }
        public string? Instructions { get; set; }
        public double TotalCost { get; set; }

    }
}
