using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceManagement.Shared
{
    public class Earning
    {
        public Earning()
        {            
        }
        public int Id { get; set; }
        
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(50)]
        public string? Subject { get; set; }

        [Required]
        public EarningCategory Category { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}

