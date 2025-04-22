using MoneyTracker1.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyTracker1.Models
{
    public class TransactionCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }
    }

}
