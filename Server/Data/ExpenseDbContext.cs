using System;
using FinanceManagement.Shared;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagement.Server.Data
{
    public class ExpenseDbContext : DbContext
    {

        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options)
            : base(options)
        {
        }
        public DbSet<Expense> Expenses { get; set; } = null!;
    }
}

