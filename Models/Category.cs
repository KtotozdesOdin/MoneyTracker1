using System.ComponentModel.DataAnnotations;

namespace MoneyTracker1.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Type { get; set; }  //Доход или расход

        public ICollection<Transaction> Transactions { get; set; }  //свойство для определения связи один ко многим
                                                                    //одна категории много тразакций

        public static IEnumerable<Category> Categories = new List<Category>()
        {
            new Category { Name = "Основной доход", Type = "Income" },
            new Category { Name = "Дополнительный доход", Type = "Income" },
            new Category { Name = "Продукты", Type = "Expense" },
            new Category { Name = "Развлечения", Type = "Expense" },
            new Category { Name = "Транспорт", Type = "Expense" },
            new Category { Name = "Спорт", Type = "Expense" },
            new Category { Name = "Жилье", Type = "Expense" },
            new Category { Name = "Кредиты", Type = "Expense" },
            new Category { Name = "Животные", Type = "Expense" },
            new Category { Name = "Иное", Type = "Expense" }
        };

    }
}
