using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
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

        public IEnumerable<AnalyticItemDto> GetAnalyticsItems()
        {
            //Get every Item and join it to borrows and get for every month how many borrows there are
            return _context.Items
                .Select(item => new AnalyticItemDto
                {
                    Name = item.Name,
                    Monate = new List<AnalyticItemMonth>()
                })
                .ToList();
        }
    }

    public interface IAnalyticsRepository
    {
        public IEnumerable<AnalyticItemDto> GetAnalyticsItems();
    }
}
