using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace NETFinalProject.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(255)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(255)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public FinancialSummary? FinancialSummary { get; set; }

        public override string ToString()
        {
            return $"ID: {UserId}, Name: {FirstName} {LastName}";
        }
    }
}
