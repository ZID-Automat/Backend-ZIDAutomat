using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Domain.Models.Logging
{
    public abstract class BaseLogQrCode
    {
        public int Id { get; set; }
        public string Scanned { get; set; } = string.Empty;
        public virtual Borrow? Borrow { get; set; }
        public DateTime DateTime { get; set; }
    }
}
