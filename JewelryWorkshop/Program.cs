using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JewelryWorkshop.Data;
using DAL.EF;
using DAL.Repositories.Interfaces;
using DAL.Repositories.Implementation;
using JewelryWorkshop.Controllers;
using Serilog;

namespace JewelryWorkshop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("JewelryWorkshopDBContextConnection") ?? throw new InvalidOperationException("Connection string 'JewelryWorkshopDBContextConnection' not found.");

            builder.Services.AddDbContext<IdentityDBContext>(options => options.UseSqlServer(connectionString));

            Log.Logger = new LoggerConfiguration()
            .WriteTo.File("C:\\Users\\User\\source\\repos\\JewelryWorkshop\\JewelryWorkshop\\Logs\\logfile.txt")
            .CreateLogger();

            builder.Services.AddLogging(builder =>
            {
                builder.AddSerilog();
            });

            builder.Services.AddDbContext<JewelryWorkshopContext>();
            builder.Services.AddScoped<IProductTypeRepository, ProductsTypeRepository>();
            builder.Services.AddScoped<IShoppingListRepository, ShoppingListRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOrdersProductRepository, OrdersProductRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
            builder.Services.AddScoped<IStonesWeightRepository, StonesWeightRepository>();
            builder.Services.AddScoped<IPreciousStoneRepository, PreciousStoneRepository>();
            builder.Services.AddScoped<OrderCountViewModelRepository, OrderCountViewModelRepository>();
            builder.Services.AddScoped<CustomerOrderTotalModelRepository, CustomerOrderTotalModelRepository>();

            builder.Services.AddDefaultIdentity<JewelryWorkshopUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<IdentityDBContext>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.Configure<IdentityOptions>(options => {
                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}