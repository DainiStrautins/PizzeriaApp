using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PizzeriaApp.Configurations.Entities;
using PizzeriaApp.Models;
using System.Data;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace PizzeriaApp.Models
{
    public class DatabaseContext : IdentityDbContext<User> 
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<UserOrders> UserOrders { get; set; }
        public DbSet<OrderDetails> OrderDetails{ get; set; }
        public DbSet<PizzeriaApp.Models.DeliveryType> DeliveryType { get; set; }
        public DbSet<PizzeriaApp.Models.Delivery> Delivery { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
