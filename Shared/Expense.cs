using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceManagement.Shared
{
    public class Expense
    {

        public Expense()
        {            
        }
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(50)]
        public string? Subject { get; set; }

        [Required]
        public ExpenseCategory Category { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}

