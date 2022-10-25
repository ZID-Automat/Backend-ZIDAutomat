using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Domain.Models
{
    public class AdmonitionType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        private List<Admonition> _Admonitions { get; set; } = new List<Admonition>();
        public IReadOnlyList<Admonition> Admonitions => _Admonitions;
    }
}
