namespace PizzeriaApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Delivery Delivery { get; set; }
        public int DeliveryId { get; set; }
        public DateTime DateOfPayment { get; set; }
        public double Price { get; set; }
        public List<UserOrders> UserOrders { get; set; }
    }
}
