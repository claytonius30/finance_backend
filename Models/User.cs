using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BackendFinance.Models
{
    public class User : IdentityUser<Guid>
    {
        
        //public Guid Id { get; set; }

        //[Required(ErrorMessage = "First name is required.")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        //[Required(ErrorMessage = "Last name is required.")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        public FinancialSummary? FinancialSummary { get; set; }
    }
}
