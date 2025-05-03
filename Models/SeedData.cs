using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using MoneyTracker1.Data;
using MoneyTracker1.Models.Enums;

namespace MoneyTracker1.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                if (context == null)
                {
                    throw new ArgumentNullException("Null AppDbContext");
                }

                if (context.Categories.Any() || context.Transactions.Any())
                {

                    return;
                }

                foreach (var category in Category.Categories)
                {
                    if (!context.Categories.Any(c => c.Name == category.Name && c.Type == category.Type))
                    {
                        context.Categories.Add(category);
                    }
                }
                context.SaveChanges(); //сохраняю категории

                var dbCategory = context.Categories.ToList();

                context.Transactions.AddRange(
                    new Transaction
                    {
                        Amount = 100000,
                        Date = DateTime.UtcNow.AddDays(-12),
                        Description = "Зарплата за март",
                        CategoryId = dbCategory.First(c => c.Name == "Основной доход").Id,
                        TransactionType = TransactionType.Income,

                    },
                    new Transaction
                    {
                        Amount = 3000,
                        Date = DateTime.UtcNow.AddDays(-7),
                        Description = "Фриланс-проект",
                        CategoryId = dbCategory.First(c => c.Name == "Дополнительный доход").Id,
                        TransactionType = TransactionType.Income
                    },
                    new Transaction
                    {
                        Amount = 1100,
                        Date = DateTime.UtcNow.AddDays(-5),
                        Description = "Магазин Лента",
                        CategoryId = dbCategory.First(c => c.Name == "Продукты").Id,
                        TransactionType = TransactionType.Expense
                    },
                    new Transaction
                    {
                        Amount = 600,
                        Date = DateTime.UtcNow.AddDays(-3),
                        Description = "Кинотеатр",
                        CategoryId = dbCategory.First(c => c.Name == "Развлечения").Id,
                        TransactionType = TransactionType.Expense
                    },
                    new Transaction
                    {
                        Amount = 600,
                        Date = DateTime.UtcNow.AddDays(-3),
                        Description = "Тренировка",
                        CategoryId = dbCategory.First(c => c.Name == "Спорт").Id,
                        TransactionType = TransactionType.Expense
                    },

                    new Transaction
                    {
                        Amount = 115000,
                        Date = DateTime.UtcNow.AddDays(-12),
                        Description = "Зарплата Апрель",
                        CategoryId = dbCategory.First(c => c.Name == "Основной доход").Id,
                        TransactionType = TransactionType.Income,

                    },
                    new Transaction
                    {
                        Amount = 25000,
                        Date = DateTime.UtcNow.AddDays(-7),
                        Description = "Премия",
                        CategoryId = dbCategory.First(c => c.Name == "Дополнительный доход").Id,
                        TransactionType = TransactionType.Income
                    },
                    new Transaction
                    {
                        Amount = 2750,
                        Date = DateTime.UtcNow.AddDays(-4),
                        Description = "Корм для кота",
                        CategoryId = dbCategory.First(c => c.Name == "Животные").Id,
                        TransactionType = TransactionType.Expense
                    },

                    new Transaction
                    {
                        Amount = 2750,
                        Date = DateTime.UtcNow.AddDays(-2),
                        Description = "Кредит машина",
                        CategoryId = dbCategory.First(c => c.Name == "Кредиты").Id,
                        TransactionType = TransactionType.Expense
                    },
                    new Transaction
                    {
                        Amount = 2750,
                        Date = DateTime.UtcNow.AddDays(-8),
                        Description = "Командировка",
                        CategoryId = dbCategory.First(c => c.Name == "Иное").Id,
                        TransactionType = TransactionType.Expense
                    }


                );                        

                
                context.SaveChanges();
                Console.WriteLine("SeedData: сохраняю изменения...");
            }
        }
    }
}
