// Clayton DeSimone
// Web Services
// Final Project
// 4/29/2024

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BackendFinance.Models
{
    public class User : IdentityUser<Guid>
    {
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        public FinancialSummary? FinancialSummary { get; set; }
    }
}
