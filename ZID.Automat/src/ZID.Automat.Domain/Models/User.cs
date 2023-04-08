using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Interfaces;

namespace ZID.Automat.Domain.Models
{
    public class User:HasName
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Joined { get; set  ; }

        private List<Borrow> _Borrow { get; set; } = new List<Borrow>();
        public IReadOnlyList<Borrow> Borrow => _Borrow;
    }
}
