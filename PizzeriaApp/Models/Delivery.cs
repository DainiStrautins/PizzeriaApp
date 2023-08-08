namespace PizzeriaApp.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public int? DeliveryTypeId { get; set; }
        public DeliveryType? DeliveryType { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? ContactName { get; set; }
        public string? ContactPhone { get; set; }
        public string? Instructions { get; set; }
        public string? TrackingNumber { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }
}
