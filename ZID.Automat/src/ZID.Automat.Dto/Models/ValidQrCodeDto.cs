using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Dto.Models
{
    public record ValidQrCodeDto
    {
        public bool valid { get; set; }
        public int ItemId { get; set; }

        public string Message { get; set; } = string.Empty;
        public string Message2 { get; set; } = string.Empty;

    }
}
