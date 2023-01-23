using System;
using System.Net.Http;
using FinanceManagement.Client.ViewModels;

namespace FinanceManagement.Client.Services
{
    public interface IDataService
    {
        Task<ICollection<YearlyItem>> LoadYearlyExpenses(int year = 0);
        Task<ICollection<YearlyItem>> LoadYearlyEarnings(int year = 0);
        Task<ThreeMonthsData> LoadLast3MonthsExpenses(int quarter = 0, int year = 0);
        Task<ThreeMonthsData> LoadLast3MonthsEarnings(int quarter = 0, int year = 0);
    }
}

