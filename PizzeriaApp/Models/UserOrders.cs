namespace PizzeriaApp.Models
{
    public class UserOrders
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public string? UserId { get; set; }
        public OrderDetails? OrderDetails { get; set; }
        public int? OrderDetailsId { get; set; }
        public Status? Status { get; set; }
        public int? StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserOrders? ParentOrder { get; set; }
        public int? ParentOrderId { get; set; }
    }
}
