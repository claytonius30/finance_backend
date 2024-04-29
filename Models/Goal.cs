// Clayton DeSimone
// Web Services
// Final Project
// 4/29/2024

using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace BackendFinance.Models
{
    public class Goal
    {
        [Key]
        public int GoalId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Set")]
        public DateTime SetDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Goal date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Goal Date")]
        public DateTime GoalDate { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(50)]
        public string Description { get; set; }

        public string Status { get; set; }

        public Guid Id { get; set; }
    }
}
