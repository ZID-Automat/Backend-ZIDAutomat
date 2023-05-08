using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Dto.Models;
using ZID.Automat.Repository;

namespace ZID.Automat.Application
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAnalyticsRepository _analyticsRepository;

        public AnalyticsService(IAnalyticsRepository analyticsRepository)
        {
            _analyticsRepository = analyticsRepository;
        }

        public IEnumerable<AnalyticItemDto> GetAnalyticsItems()
        {
            return _analyticsRepository.GetAnalyticsItems();
        }
    }

    public interface IAnalyticsService
    {
        public IEnumerable<AnalyticItemDto> GetAnalyticsItems();
    }
}
