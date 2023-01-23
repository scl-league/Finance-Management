using System;
using System.Linq;
using FinanceManagement.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManagement.Server.Data
{

    public static class SeedingData
    {
        public static DateTime today = DateTime.Today;
        public static DateTime lastMonth = today.AddMonths(-1);
        public static DateTime previousMonth = today.AddMonths(-2);

        public static Random random = new Random();


        // Data for Custorm Repository
        public static void AddEarningRepository(this IServiceCollection service)
        {           
            var earningRepository = new MemoryRepository<Earning>();

            earningRepository.Add(new Earning { Date = new DateTime(previousMonth.Year, previousMonth.Month, 25), Amount = 2480, Category = EarningCategory.Salary, Subject = "Monthly Salary" });
            earningRepository.Add(new Earning { Date = new DateTime(previousMonth.Year, previousMonth.Month, 12), Amount = 440, Category = EarningCategory.Freelancing, Subject = "Freelancing Client A" });
            earningRepository.Add(new Earning { Date = new DateTime(lastMonth.Year, lastMonth.Month, 25), Amount = 2480, Category = EarningCategory.Salary, Subject = "Monthly Salary" });
            earningRepository.Add(new Earning { Date = new DateTime(lastMonth.Year, lastMonth.Month, 12), Amount = 790, Category = EarningCategory.Freelancing, Subject = "Freelancing Client A" });
            earningRepository.Add(new Earning { Date = new DateTime(lastMonth.Year, lastMonth.Month, 4), Amount = 387, Category = EarningCategory.CapitalGain, Subject = "ETF Dividends" });
            earningRepository.Add(new Earning { Date = new DateTime(today.Year, today.Month, 25), Amount = 2480, Category = EarningCategory.Salary, Subject = "Monthly Salary" });
            earningRepository.Add(new Earning { Date = new DateTime(today.Year, today.Month, 14), Amount = 680, Category = EarningCategory.Freelancing, Subject = "Freelancing Client A" });
            earningRepository.Add(new Earning { Date = new DateTime(today.Year, today.Month, 12), Amount = 245, Category = EarningCategory.Flipping, Subject = "Old TV" });

            service.AddSingleton<IRepository<Earning>>(earningRepository);
        }

        // Adding Sample Ramdom Data to " DbSet<Finance> " of EF Core InMomory 
        public static void InitializeData(this IServiceCollection service)
        {            
            // creating the contextOption Coz FinanceDbContext constructor was using dbContextOptions
            var contextOptions = new DbContextOptionsBuilder<FinanceDbContext>()
                                    .UseInMemoryDatabase("Finance")
                                    .Options;

            using (var context = new FinanceDbContext(contextOptions))
            {
                if (context.Earnings.Any()) return;

                // Random Generate Data for Earning
                var lastYearEarning = GenerateRandomSampleEarning(1).Concat(GenerateRandomSampleEarning(1));
                var totalEarning = lastYearEarning.Concat(GenerateRandomSampleEarning(2));

                foreach (var data in totalEarning)
                {
                    context.Earnings.Add(data);
                }

                // Random Generate Data for Expense
                var lastYearExpense = GenerateRandomSampleExpense(1).Concat(GenerateRandomSampleExpense(1));
                var totalExpense = lastYearExpense.Concat(GenerateRandomSampleExpense(2));

                foreach (var data in totalExpense)
                {
                    context.Expenses.Add(data);
                }
                
                context.Earnings.AddRange(
                    new Earning { Date = new DateTime(previousMonth.Year, previousMonth.Month, 25), Amount = 2480, Category = EarningCategory.Salary, Subject = "Monthly Salary" },
                    new Earning { Date = new DateTime(previousMonth.Year, previousMonth.Month, 12), Amount = 440, Category = EarningCategory.Freelancing, Subject = "Freelancing Client A" },
                    new Earning { Date = new DateTime(lastMonth.Year, lastMonth.Month, 25), Amount = 2480, Category = EarningCategory.Salary, Subject = "Monthly Salary" },
                    new Earning { Date = new DateTime(lastMonth.Year, lastMonth.Month, 12), Amount = 790, Category = EarningCategory.Freelancing, Subject = "Freelancing Client A" },
                    new Earning { Date = new DateTime(lastMonth.Year, lastMonth.Month, 4), Amount = 387, Category = EarningCategory.CapitalGain, Subject = "ETF Dividends" },
                    new Earning { Date = new DateTime(today.Year, today.Month, 25), Amount = 2480, Category = EarningCategory.Salary, Subject = "Monthly Salary" },
                    new Earning { Date = new DateTime(today.Year, today.Month, 14), Amount = 680, Category = EarningCategory.Freelancing, Subject = "Freelancing Client A" },
                    new Earning { Date = new DateTime(today.Year, today.Month, 12), Amount = 245, Category = EarningCategory.Flipping, Subject = "Old TV" }                    
                );
                context.Expenses.AddRange(
                    new Expense { Date = new DateTime(previousMonth.Year, previousMonth.Month, 8), Amount = 1050, Category = ExpenseCategory.Housing, Subject = "Rent" },
                    new Expense { Date = new DateTime(previousMonth.Year, previousMonth.Month, 18), Amount = 140, Category = ExpenseCategory.Education, Subject = "Books" },
                    new Expense { Date = new DateTime(lastMonth.Year, lastMonth.Month, 6), Amount = 1050, Category = ExpenseCategory.Housing, Subject = "Rent" },
                    new Expense { Date = new DateTime(lastMonth.Year, lastMonth.Month, 15), Amount = 415, Category = ExpenseCategory.Healthcare, Subject = "H-Care" },
                    new Expense { Date = new DateTime(lastMonth.Year, lastMonth.Month, 27), Amount = 76, Category = ExpenseCategory.Entertainment, Subject = "Harry Potter Series" },
                    new Expense { Date = new DateTime(today.Year, today.Month, 7), Amount = 1050, Category = ExpenseCategory.Housing, Subject = "Rent" },
                    new Expense { Date = new DateTime(today.Year, today.Month, 13), Amount = 870, Category = ExpenseCategory.Entertainment, Subject = "New TV" }

                );
                context.SaveChanges();
            }
        }        

        // Randomly Generate Earning Data
        private static IEnumerable<Earning> GenerateRandomSampleEarning(int year)
        {            
            var currentYear = today.Year;

            var earningCategoryNameList = Enum.GetNames(typeof(EarningCategory));
            var earningCategoryName = new string[earningCategoryNameList.Length];

            var i = 0;            
            foreach (var item in Enum.GetNames(typeof(EarningCategory)))
            {                
                earningCategoryName[i] = item;
                i++;
            }

            return Enumerable.Range(1, 12).Select(index => new Earning
            {
                //Date = new DateTime(currentYear-year, random.Next(1,12), random.Next(1, 28)),
                Date = new DateTime(currentYear-year, index, random.Next(1, 28)),
                Amount = random.Next(50, 10000),
                Category = (EarningCategory)Enum.Parse(typeof(EarningCategory), earningCategoryName[random.Next(earningCategoryName.Length)]),
                Subject = SubjectsForEarning[random.Next(SubjectsForEarning.Length)]
            })
              .ToArray(); 
        }

        // Randomly Generate Earning Data
        private static IEnumerable<Expense> GenerateRandomSampleExpense(int year)
        {            
            var currentYear = today.Year;

            var expenseCategoryNameList = Enum.GetNames(typeof(ExpenseCategory));
            var expenseCategoryName = new string[expenseCategoryNameList.Length];

            var i = 0;
            foreach (var item in Enum.GetNames(typeof(ExpenseCategory)))
            {
                expenseCategoryName[i] = item;
                i++;
            }

            return Enumerable.Range(1, 12).Select(index => new Expense
            {

                //Date = new DateTime(currentYear - year, random.Next(1, 12), random.Next(1, 28)),
                Date = new DateTime(currentYear - year, index, random.Next(1, 28)),
                Amount = random.Next(50, 10000),
                Category = (ExpenseCategory)Enum.Parse(typeof(ExpenseCategory), expenseCategoryName[random.Next(expenseCategoryName.Length)]),
                Subject = SubjectsForExpense[random.Next(SubjectsForExpense.Length)]
            })
              .ToArray();
        }

        // data for Earning.subject
        private static readonly string[] SubjectsForEarning = new[]
        {
            "Monthly Salary", "Freelancing Client A", "ETF Dividends", "Freelancing Client B", "Gift", "Part Time", "Stock", "Bonus", "Training", "Tutioring"
        };

        private static readonly string[] SubjectsForExpense = new[]
        {
            "Book", "New TV", "New iPhone", "House Painting", "Urgent Care", "Dog Helath", "Movie", "Online Learning", "Transportation", "Dinning"
        };

    }

}

