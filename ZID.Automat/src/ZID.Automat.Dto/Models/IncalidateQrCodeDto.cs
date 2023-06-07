using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Dto.Models
{
    public record InvalidateQrCodeDto
    {
        public string QrCode { get; set; } = string.Empty;
    }
}
