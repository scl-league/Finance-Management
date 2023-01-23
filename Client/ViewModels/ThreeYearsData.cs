using System;
namespace FinanceManagement.Client.ViewModels
{
    public class ThreeYearsData
    {
        public YearlyItem? CurrentYear { get; set; }
        public YearlyItem? LastYear { get; set; }
        public YearlyItem? PreviousYear { get; set; }
    }
}

