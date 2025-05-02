using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using MoneyTracker1.Models.Enums;

namespace MoneyTracker1.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public TransactionType TransactionType { get; set; }  // приход / расход

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

        //цвета для транзакций по типу
        public string ColorTransaction => TransactionType == TransactionType.Income ? "bg-success" : "bg-warning";
    }
}
