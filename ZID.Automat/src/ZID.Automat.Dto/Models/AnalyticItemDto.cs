using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Dto.Models
{
    public class AnalyticItemDto
    {
        public string Name { get; set; } = default!;
        public List<AnalyticItemMonth> Monate { get; set; } = new List<AnalyticItemMonth>();
    }

    public class AnalyticItemMonth
    {
        public string Name { get; set; } = default!;
        public int Value { get; set; }
    }
}
