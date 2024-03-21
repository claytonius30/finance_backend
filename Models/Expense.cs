using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NETFinalProject.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }

        [Required(ErrorMessage = "Expense category is required.")]
        [StringLength(255)]
        [Display(Name = "Expense Category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date incurred is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Incurred")]
        public DateTime DateIncurred { get; set; }
        
        public int UserId { get; set; }

        public override string ToString()
        {
            return $"ID: {ExpenseId}, Category: {Category}, Amount: {Amount:C}, Date Incurred: {DateIncurred.ToShortDateString()}";
        }
    }
}