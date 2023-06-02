using Bogus.DataSets;
using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models.Analytics;
using ZID.Automat.Dto.Models.Analytics.User;
using ZID.Automat.Repository;

namespace ZID.Automat.Application
{
    public interface IAnalyticsService
    {
        IEnumerable<GesammtBorrowsDto> GesammtBorrows();
        IEnumerable<AnalyticItemDto> GetAnalyticsItems();
        IEnumerable<TaeglicheUserDto> TaeglicheUser();
        IEnumerable<WieVielZuspaetDto> WievielZuspat();
    }

    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAnalyticsRepository _analyticsRepository;
        private readonly IRepositoryRead _repositoryRead;

        public AnalyticsService(IAnalyticsRepository analyticsRepository, IRepositoryRead repositoryRead)
        {
            _analyticsRepository = analyticsRepository;
            _repositoryRead = repositoryRead;
        }


        public IEnumerable<GesammtBorrowsDto> GesammtBorrows()
        {
            var result1 = _repositoryRead
            .GetAll<Item>()
            .Select(item => new GesammtBorrowsDto() { Value = item.ItemInstances.Where(i => i.borrow != null).Count(), Label = item.Name });
            return result1;
        }

        public IEnumerable<AnalyticItemDto> GetAnalyticsItems()
        {
            int months = 10;
            DateTime date = DateTime.Now.AddMonths(-months);

            var resultRoh = _repositoryRead
                .GetAll<Item>()
                .OrderByDescending(i => i.ItemInstances.Where(el => el?.borrow?.BorrowDate > date).Count())
                .Take(3)
                .ToList();

            var result1 = resultRoh.Select(e =>
            {
                var monthsi = new List<AnalyticItemMonth>();
                for (int i = 0; i < months; i++)
                {
                    var dati = DateTime.Now.AddMonths(-i);
                    monthsi.Add(new AnalyticItemMonth() { Name = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dati.Month), Value = e.ItemInstances.Where(joe => joe.borrow != null && joe.borrow.BorrowDate.Month == dati.Month && joe.borrow.BorrowDate.Year == dati.Year).Count() });
                }

                return new AnalyticItemDto()
                {
                    Name = e.Name,
                    Monate = monthsi
                };
            });
            return result1;
        }

        public IEnumerable<WieVielZuspaetDto> WievielZuspat()
        {
            return _repositoryRead.GetAll<Borrow>().GroupBy(b => b.StatusEntschuldigt()).Select(b => new WieVielZuspaetDto() { num = b.Count(), label = b.Key });
        }

        public IEnumerable<TaeglicheUserDto> TaeglicheUser()
        {
            Tuple<string, DateOnly,int>[] cats =
            {
                new Tuple<string,DateOnly,int>("Gestern",DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),0),
                new Tuple<string,DateOnly,int>("Letze drei Tagen",DateOnly.FromDateTime(DateTime.Now.AddDays(-3)),1),
                new Tuple<string,DateOnly,int>("Letze Woche",DateOnly.FromDateTime(DateTime.Now.AddDays(-7)),2),
                new Tuple<string,DateOnly,int>("Letze zwei Wochen",DateOnly.FromDateTime(DateTime.Now.AddDays(-14)),3),
                new Tuple<string,DateOnly,int>("Letzer Monat",DateOnly.FromDateTime(DateTime.Now.AddDays(-31)),4),
                new Tuple<string,DateOnly,int>("Mehr als ein Monat",DateOnly.FromDateTime(DateTime.MinValue),5)

            };
            var data = _repositoryRead.GetAll<User>()
                .GroupBy(u => cats.First(c => c.Item2 <= DateOnly.FromDateTime(u.LastLogin)).Item1)
                .OrderByDescending(u=>cats.First(c=>c.Item1 == u.Key).Item2)
                .Select(u => new TaeglicheUserDto() { Label = u.Key, Value = u.Count() });
            return data;
        }
    }


}
