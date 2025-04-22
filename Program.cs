using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoneyTracker1.Data;
using MoneyTracker1.Data;
using MoneyTracker1.Models;
using System;

namespace MoneyTracker1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           //регистрация контекста БД

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("MoneyTrackerConnection")));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Поддержка Razor Pages для Identity UI
            builder.Services.AddRazorPages();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при инициализации базы данных: {ex.Message}");
                }
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
