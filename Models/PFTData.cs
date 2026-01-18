
namespace Personal_Finance_Tracker.Models
{
    public class PFTData
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
