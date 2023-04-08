using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Domain.Interfaces
{
    public interface HasGuid
    {
        public Guid GUID { get; set; }
    }
}
