// Clayton DeSimone
// Web Services
// Final Project
// 4/29/2024

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendFinance.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }

        [Required(ErrorMessage = "Expense category is required.")]
        [StringLength(50)]
        [Display(Name = "Expense Category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date incurred is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Incurred")]
        public DateTime DateIncurred { get; set; }
        
        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"ID: {ExpenseId}, Category: {Category}, Amount: {Amount:C}, Date Incurred: {DateIncurred.ToShortDateString()}";
        }
    }
}