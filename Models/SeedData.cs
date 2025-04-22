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

                //Если в базе данных уже есть категории, 
                // то возвращается инициализатор заполнения и категории не добавляются
                if (context.Categories.Any())
                {
                    return; //уже добавлена
                }

                //Переменная отвечающая за создание списка предварительных категорий для БД
                var categories = new List<Category>
                {
                    new Category {Name = "Зарплата", Type = "Income"},
                    new Category {Name = "Дополнительный доход", Type = "Income"},
                    new Category {Name = "Продукты", Type = "Expense"},
                    new Category {Name = "Развлечения", Type = "Expense"},
                    new Category {Name = "Транспорт", Type = "Expense"},
                    new Category {Name = "Спорт", Type = "Expense"}

                };

                Console.WriteLine("SeedData: добавляю категории...");
                context.Categories.AddRange(categories); //добавляю категории в таблицу
                context.SaveChanges(); //сохраняю категории

                context.Transactions.AddRange(
                    new Transaction
                    {
                        Amount = 100000,
                        Date = DateTime.UtcNow.AddDays(-12),
                        Description = "Зарплата за март",
                        CategoryId = categories.First(c => c.Name == "Зарплата").Id,
                        TransactionType = TransactionType.Income,

                    },
                    new Transaction
                    {
                        Amount = 3000,
                        Date = DateTime.UtcNow.AddDays(-7),
                        Description = "Фриланс-проект",
                        CategoryId = categories.First(c => c.Name == "Дополнительный доход").Id,
                        TransactionType = TransactionType.Income
                    },
                    new Transaction
                    {
                        Amount = 1100,
                        Date = DateTime.UtcNow.AddDays(-5),
                        Description = "Магазин Лента",
                        CategoryId = categories.First(c => c.Name == "Продукты").Id,
                        TransactionType = TransactionType.Expense
                    },
                    new Transaction
                    {
                        Amount = 600,
                        Date = DateTime.UtcNow.AddDays(-3),
                        Description = "Кинотеатр",
                        CategoryId = categories.First(c => c.Name == "Развлечения").Id,
                        TransactionType = TransactionType.Expense
                    },
                    new Transaction
                    {
                        Amount = 600,
                        Date = DateTime.UtcNow.AddDays(-3),
                        Description = "Тренировка",
                        CategoryId = categories.First(c => c.Name == "Спорт").Id,
                        TransactionType = TransactionType.Expense
                    }

                );                        

                
                context.SaveChanges();
                Console.WriteLine("SeedData: сохраняю изменения...");
            }
        }
    }
}
