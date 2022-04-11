using AngloAmerican.Api.Models;
using AngloAmerican.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AngloAmerican.Api.UnitTest
{
    public class PnlServiceTest
    {
        [Fact]
        public void ExtractHistoricalPnl_ShouldReturn_CorrectResult_For_Single_YearAndMonth()
        {
            Dictionary<DateTime, decimal> pnlDictionary = new Dictionary<DateTime, decimal>();
            var offsetDate = new DateTime(2022, 4, 1);
            
            pnlDictionary.Add(offsetDate, 100);
            pnlDictionary.Add(offsetDate.AddDays(5), 101);

            var expectedNumOfYears = GetNumberOfYearsFromDictionary(pnlDictionary);
            var expectedNumOfMonths = GetNumberOfMonthsFromDictionary(pnlDictionary);
            var expectedNumOfDays = GetNumberOfDaysFromDictionary(pnlDictionary);

            var sut = new PnlService();
            var result = sut.ExtractHistoricalPnl(pnlDictionary);

            var actualNumOfYears = GetNumberOfYearsFromActual(result);
            var actualNumOfMonths = GetNumberOfMonthsFromActual(result);
            var actualNumOfDays = GetNumberOfDaysFromActual(result);

            Assert.Equal(expectedNumOfYears, actualNumOfYears);
            Assert.Equal(expectedNumOfMonths, actualNumOfMonths);
            Assert.Equal(expectedNumOfDays, actualNumOfDays);
        }

        [Fact]
        public void ExtractHistoricalPnl_ShouldReturn_CorrectResult_For_Multiple_YearsAndMonths()
        {
            Random rnd = new Random();
            Dictionary<DateTime, decimal> pnlDictionary = new Dictionary<DateTime, decimal>();
            var offsetDate = new DateTime(2021, 4, 1);
            var endDate = new DateTime(2022, 4, 1);

            while (offsetDate < endDate)
            {
                pnlDictionary.Add(offsetDate, rnd.Next(95, 105));
                offsetDate = offsetDate.AddDays(1);
            }
            var expectedNumOfYears = GetNumberOfYearsFromDictionary(pnlDictionary);
            var expectedNumOfMonths = GetNumberOfMonthsFromDictionary(pnlDictionary);
            var expectedNumOfDays = GetNumberOfDaysFromDictionary(pnlDictionary);

            var sut = new PnlService();
            var result = sut.ExtractHistoricalPnl(pnlDictionary);

            var actualNumOfYears = GetNumberOfYearsFromActual(result);
            var actualNumOfMonths = GetNumberOfMonthsFromActual(result);
            var actualNumOfDays = GetNumberOfDaysFromActual(result);

            Assert.Equal(expectedNumOfYears, actualNumOfYears);
            Assert.Equal(expectedNumOfMonths, actualNumOfMonths);
            Assert.Equal(expectedNumOfDays, actualNumOfDays);
        }

        private int GetNumberOfYearsFromDictionary(Dictionary<DateTime, decimal> pnlDictionary)
        {
            return pnlDictionary.GroupBy(a => a.Key.Year).Count();
        }

        private int GetNumberOfMonthsFromDictionary(Dictionary<DateTime, decimal> pnlDictionary)
        {
            var numOfMonths = 0;
            var pnlsByYear = pnlDictionary.GroupBy(a => a.Key.Year);

            for (var i = 0; i < pnlsByYear.Count(); i++)
            {
                numOfMonths += pnlsByYear.ElementAt(i).GroupBy(a => a.Key.Month).Count();
            }

            return numOfMonths;
        }

        private int GetNumberOfDaysFromDictionary(Dictionary<DateTime, decimal> pnlDictionary)
        {
            var numOfDays = 0;
            var pnlsByYear = pnlDictionary.GroupBy(a => a.Key.Year);

            for (var i = 0; i < pnlsByYear.Count(); i++)
            {
                numOfDays += pnlsByYear.ElementAt(i).Count();
            }

            return numOfDays;
        }

        private int GetNumberOfYearsFromActual(IEnumerable<PnlYtd> annualPnls)
        {
            return annualPnls.Count();
        }

        private int GetNumberOfMonthsFromActual(IEnumerable<PnlYtd> annualPnls)
        {
            var numOfMonths = 0;

            annualPnls.ToList().ForEach(year =>
            {
                numOfMonths += year.MonthlyPnls.Count();
            });

            return numOfMonths;
        }

        private int GetNumberOfDaysFromActual(IEnumerable<PnlYtd> annualPnls)
        {
            var numOfDays = 0;

            annualPnls.ToList().ForEach(year =>
            {
                year.MonthlyPnls.ForEach(month =>
                {
                    numOfDays += month.DailyPnls.Count;
                });
            });

            return numOfDays;
        }
    }
}
