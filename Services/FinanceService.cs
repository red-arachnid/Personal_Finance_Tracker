using Personal_Finance_Tracker.Data;
using Personal_Finance_Tracker.Models;

namespace Personal_Finance_Tracker.Services
{
    public class FinanceService
    {
        private Repository _repository;
        private PFTData _data;

        public FinanceService(Repository repository, PFTData data)
        {
            _repository = repository;
            _data = data;
        }

        public List<Transaction> GetTransactionsByUser(Guid userId)
            => _data.Transactions.Where(t => t.UserId == userId).OrderByDescending(t => t.Date).ToList();

        public void AddTransaction(Guid userId, decimal amount, string category, TransactionType type)
        {
            Transaction transaction = new Transaction
            {
                UserId = userId,
                Amount = amount,
                Category = category,
                Date = DateTime.Now,
                Type = type
            };

            _data.Transactions.Add(transaction);
            _repository.SaveData(_data);
        }

        public (decimal TotalIncome, decimal TotalExpense, decimal Saving) GetStats(Guid userId)
        {
            List<Transaction> transactions = GetTransactionsByUser(userId);

            decimal income = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            decimal expense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
            decimal saving = income - expense;
            return (income, expense, saving);
        }

        public void ClearTransactions(Guid userId)
        {
            _data.Transactions.RemoveAll(t => t.UserId == userId);
            _repository.SaveData(_data);
        }
    }
}
