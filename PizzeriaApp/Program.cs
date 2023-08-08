using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PizzeriaApp.Configurations;
using PizzeriaApp.Models;
using System;
using System.Globalization;

namespace PizzeriaApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("SqlConnection"))
            );;
            builder.Services.AddDefaultIdentity<User>().AddRoles<IdentityRole>()
                 .AddEntityFrameworkStores<DatabaseContext>();
            builder.Services.AddAutoMapper(typeof(InitalizeMapper));
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            using (var Scope = app.Services.CreateScope())
            {
                var services = Scope.ServiceProvider;
                try
                {
                    var user = services.GetRequiredService<UserManager<User>>();
                    InitalizeDefaultUser.DefaultTestUser(services,user).Wait();
                    InitalizeDefaultUser.DefaultUserAdmin(services,user).Wait();
                }
                catch (Exception)
                {

                    throw;
                }
            }
           
            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            //Culture specific problems
            var culrureInfo = new CultureInfo("lv-LV");
            culrureInfo.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentUICulture = culrureInfo;
            app.UseRequestLocalization();
            app.Run();
        }
    }
}