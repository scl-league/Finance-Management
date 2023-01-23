using System.Net.Http.Json;
using FinanceManagement.Client.ViewModels;
using FinanceManagement.Shared;

namespace FinanceManagement.Client.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _http;

        private readonly int _currentYear = DateTime.Now.Year;


        public DataService(HttpClient http)
        {
            this._http = http;
        }

        // Current Yearl Earning
        public async Task<ICollection<YearlyItem>> LoadYearlyEarnings(int searchYear = 0)
        {
            var currentYearEarning = await _http.GetFromJsonAsync<Earning[]>("Earnings");

            searchYear = (searchYear == 0) ? _currentYear : searchYear;

            return  currentYearEarning
                .Where(e => e.Date >= new DateTime(searchYear, 1, 1) && e.Date <= new DateTime(searchYear, 12, 31))
                .GroupBy(e => e.Date.Month)                
                .Select(e => new YearlyItem
                {                    
                    Month = GetMonthAsText(e.Key, searchYear),
                    Amount = e.Sum(i => i.Amount)
                }).ToList();
        }

        // Current Yearl Expense
        public async Task<ICollection<YearlyItem>> LoadYearlyExpenses(int searchYear = 0)
        {
            var currentYearExpense = await _http.GetFromJsonAsync<Expense[]>("Expenses");

            searchYear = (searchYear == 0) ? _currentYear : searchYear;

            var data = currentYearExpense
                .Where(exp => exp.Date >= new DateTime(searchYear, 1, 1) && exp.Date <= new DateTime(searchYear, 12, 31))
                .GroupBy(exp => exp.Date.Month)                
                .Select(exp => new YearlyItem
                {
                    Month = GetMonthAsText(exp.Key, searchYear),
                    Amount = exp.Sum(i => i.Amount)
                }).ToList();

            return data;
        }

        // Last 3 Months Earning
        public async Task<ThreeMonthsData> LoadLast3MonthsEarnings(int searchQuarter = 0, int searchYear = 0)
        {
            var currentMonth = 0;
            var lastMonth = 0;
            var previousMonth = 0;

            searchYear = (searchYear == 0) ? _currentYear : searchYear;

            if (searchQuarter == 0)
            {
                 currentMonth = DateTime.Today.Month;
                 lastMonth = currentMonth - 1;
                 previousMonth = currentMonth-2;
            }
            else
            {
                var quarter = GetQuarter(searchQuarter);
                currentMonth = quarter[0];                
                lastMonth =quarter[1];
                previousMonth = quarter[2];
            }

            return new ThreeMonthsData
            {
                CurrentMonth = new MonthlyData
                {
                    Data = await GetMonthlyEarnings(currentMonth, searchYear),
                    Label = GetMonthAsText(currentMonth, searchYear).ToString()
                },
                LastMonth = new MonthlyData
                {
                    Data = await GetMonthlyEarnings(lastMonth, searchYear),
                    Label = GetMonthAsText(lastMonth, searchYear).ToString()
                },
                PreviousMonth = new MonthlyData
                {
                    Data = await GetMonthlyEarnings(previousMonth, searchYear),
                    Label = GetMonthAsText(previousMonth, searchYear).ToString()
                },

            };
        }

        private static int[] GetQuarter(int quarter)
        {
            return quarter switch
            {
                1 => new[] { 1, 2, 3 },
                2 => new[] { 3, 4, 5 },
                3 => new[] { 7, 8, 9 },
                4 => new[] { 10, 11, 12 },
                 _ => throw new NotSupportedException()
            };
        }

        // Monthly Earning 
        private async Task<ICollection<MonthlyItem>> GetMonthlyEarnings(int month, int year)
        {
            var yearlyEarning = await _http.GetFromJsonAsync<Earning[]>("Earnings");

            return yearlyEarning
                .Where(ern => (ern.Date >= new DateTime(year, month, 1)) && (ern.Date <= new DateTime(year, month, LastDayOfMonth(month,year))))
                .GroupBy(ern => ern.Category)
                .Select(ern => new MonthlyItem
                {
                    Amount = ern.Sum(item => item.Amount),
                    Category = ern.Key.ToString()
                }).ToList();
        }

        // Getting last day of month
        private static int LastDayOfMonth(int month, int year)
        {
            return DateTime.DaysInMonth(year, month);
        }

        // Monthly Expense
        private async Task<ICollection<MonthlyItem>> GetMonthlyExpenses(int month, int year)
        {
            var data = await _http.GetFromJsonAsync<Expense[]>("Expenses");

            return data.Where(expense => expense.Date >= new DateTime(year, month, 1)
                && expense.Date <= new DateTime(year, month, LastDayOfMonth(month, year)))               
                .GroupBy(expense => expense.Category)
                .Select(expense => new MonthlyItem
                {
                    Amount = expense.Sum(item => item.Amount),
                    Category = expense.Key.ToString()
                })
                .ToList();
        }

        // Last 3 Months Expense
        public async Task<ThreeMonthsData> LoadLast3MonthsExpenses(int searchQuarter = 0,int searchYear = 0)
        {
            var currentMonth = 0;
            var lastMonth = 0;
            var previousMonth = 0;

            searchYear = (searchYear == 0) ? _currentYear : searchYear;

            if (searchQuarter == 0)
            {

                currentMonth = DateTime.Today.Month;
                lastMonth = currentMonth-1;
                previousMonth = currentMonth-2;
            }
            else
            {
                var quarter = GetQuarter(searchQuarter);
                currentMonth = quarter[0];
                // actually next month
                lastMonth = quarter[1];
                // next after next month
                previousMonth = quarter[2];
            }

            searchYear = (searchYear == 0) ? _currentYear : searchYear;

            return new ThreeMonthsData
            {
                CurrentMonth = new MonthlyData
                {
                    Data = await GetMonthlyExpenses(currentMonth, searchYear),
                    Label = GetMonthAsText(currentMonth, searchYear)
                },
                LastMonth = new MonthlyData
                {
                    Data = await GetMonthlyExpenses(lastMonth, searchYear),
                    Label = GetMonthAsText(lastMonth, searchYear)
                },
                PreviousMonth = new MonthlyData
                {
                    Data = await GetMonthlyExpenses(previousMonth, searchYear),
                    Label = GetMonthAsText(previousMonth, searchYear)
                }
            };
        }

        private static string GetMonthAsText(int month, int year)
        {
            return month switch
            {
                1 => $"Jan",
                2 => $"Feb",
                3 => $"Mar",
                4 => $"Apr",
                5 => $"May",
                6 => $"Jun",
                7 => $"Jul",
                8 => $"Aug",
                9 => $"Sep",
                10 => $"Oct",
                11 => $"Nov",
                12 => $"Dec",
                _ => throw new NotImplementedException(),
            };
        }

    }
}

