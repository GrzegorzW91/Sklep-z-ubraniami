
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SklepZUbraniami.Data;
using SklepZUbraniami.Models;
using SklepZUbraniami.Services;

namespace SklepZUbraniami
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

           
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("SklepDB"));

         
            builder.Services.AddControllersWithViews();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddRazorPages();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<CartService>();

            builder.Services.AddAuthorization();
            builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
    .AddEntityFrameworkStores<AppDbContext>();

            var app = builder.Build();

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();
            app.MapRazorPages();

            // Routing MVC
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Product}/{action=Index}/{id?}");

            // Seed danych
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Products.AddRange(
                    new Product { Name = "Koszulka", Price = 59.99m, Quantity = 10 },
                    new Product { Name = "Spodnie", Price = 89.99m, Quantity = 5 },
                    new Product { Name = "Kurtka", Price = 199.99m, Quantity = 3 },
                    new Product { Name = "Bluza", Price = 129.99m, Quantity = 7 },
                    new Product { Name = "Czapka", Price = 29.99m, Quantity = 15 },
                    new Product { Name = "Buty sportowe", Price = 299.99m, Quantity = 4 },
                    new Product { Name = "Skarpetki (5 par)", Price = 24.99m, Quantity = 20 }
);
                context.SaveChanges();
            }

            app.Run();
        }
    }
}
