using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Dto.Models.Analytics
{
    public record QrCodeScannDto
    {
        public int Id { get; set; }
        public string Scanned { get; set; } = string.Empty;
        public int?  BorrowId { get; set; }
        public DateTime DateTime { get; set; }
    }
}
