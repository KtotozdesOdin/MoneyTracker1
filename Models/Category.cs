using System.ComponentModel.DataAnnotations;

namespace MoneyTracker1.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Type { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
