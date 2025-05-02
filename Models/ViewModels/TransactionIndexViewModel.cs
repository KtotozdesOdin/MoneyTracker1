namespace MoneyTracker1.Models.ViewModels
{
    public class TransactionIndexViewModel
    {
        public IEnumerable<Transaction> Transactions { get; set; } 

        public IEnumerable<Category> Categories { get; set; }

        public int? SelectedCategory { get; set; }
    }
}
