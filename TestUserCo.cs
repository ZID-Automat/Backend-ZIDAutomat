using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Configuration.Model
{
    public record TestUserCo
    {
        public bool UseDebug { get; init; }
        public string TestUserName { get; init; } = string.Empty;
        public string TestUserPassword { get; init; } = string.Empty;
    }
}
