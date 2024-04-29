namespace BackendFinance.Models
{
    public class LockoutInfo
    {
        public string LockoutEnd { get; set; } = default!;
        public string LockoutRemaining { get; set; } = default!;
    }
}
