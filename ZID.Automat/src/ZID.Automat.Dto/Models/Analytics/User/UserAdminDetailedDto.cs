using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Dto.Models.Analytics.User
{
    public class UserAdminDetailedDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Joined { get; set; }
        public virtual List<UserAdmiBorrowDto> Borrow { get; set; } = new List<UserAdmiBorrowDto>();
        public string Vorname { get; set; } = string.Empty;
        public string Nachname { get; set; } = string.Empty;
    }

    public class UserAdmiBorrowDto
    {
        public int Id { get; set; }
        public DateTime BorrowDate { get; set; }
        public bool Returned { get; set; }
        public string Itemname { get; set; } = string.Empty;
    }
}
