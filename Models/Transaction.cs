
namespace Personal_Finance_Tracker.Models
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; } = "General";
        public DateTime Date { get; set; } = DateTime.Now;
        public TransactionType Type { get; set; }
    }
    public enum TransactionType { Income, Expense }
}
