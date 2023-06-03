using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Dto.Models.Analytics.User
{
    public class UserAdminGetAll
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Joined { get; set; }
        public int BorrowCount { get; set; }
        public string Vorname { get; set; } = string.Empty;
        public string Nachname { get; set; } = string.Empty;
        public bool Blockiert { get; set; } = false;
    }
}
