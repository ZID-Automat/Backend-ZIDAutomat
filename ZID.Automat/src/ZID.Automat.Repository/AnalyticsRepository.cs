using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models.Analytics;
using ZID.Automat.Infrastructure;

namespace ZID.Automat.Repository
{

    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly AutomatContext _context;
        public AnalyticsRepository(AutomatContext Db)
        {
            _context = Db;
        }

        public IEnumerable<AnalyticItemDto> GetAnalyticsItems(DateTime date)
        {
            //Set wanted Monate
            var months = Enumerable.Range(0, (DateTime.Now.Year - date.Year) * 12 + DateTime.Now.Month - date.Month + 1);


            
            //Get every Item and join it to borrows and get for every month how many borrows there are
            return _context.Items
                .Select(item => new AnalyticItemDto
                {
                    Name = item.Name,
                    Monate =
                    {
                        
                    }
                })
                .ToList();
        }
    }

    public interface IAnalyticsRepository
    {
        public IEnumerable<AnalyticItemDto> GetAnalyticsItems(DateTime date);
    }
}
