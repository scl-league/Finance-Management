using System;
using FinanceManagement.Shared;
using Microsoft.EntityFrameworkCore;

namespace FinanceManagement.Server.Data
{
    public class FinanceDbContext : DbContext
    {

        public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
            : base(options)
        {
        }
        public DbSet<Earning> Earnings { get; set; } = null!;
        public DbSet<Expense> Expenses { get; set; } = null!;
    }
}

