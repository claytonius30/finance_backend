using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendFinance.Models
{
    public class Income
    {
        [Key]
        public int IncomeId { get; set; }

        [Required(ErrorMessage = "Income source is required.")]
        [StringLength(50)]
        [Display(Name = "Income Source")]
        public string Source { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Date received is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Received")]
        public DateTime DateReceived { get; set; }
        
        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"ID: {IncomeId}, Source: {Source}, Amount: {Amount:C}, Date Received: {DateReceived.ToShortDateString()}";
        }
    }
}