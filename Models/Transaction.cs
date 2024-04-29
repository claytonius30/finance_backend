// Clayton DeSimone
// Web Services
// Final Project
// 4/29/2024

namespace BackendFinance.Models
{
    public class Transaction
    {
        public string Type { get; set; } = default!;
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = default!;
        public DateTime Date { get; set; }
    }
}
