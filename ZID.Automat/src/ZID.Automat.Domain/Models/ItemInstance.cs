using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Domain.Models
{
    public class ItemInstance
    {
        public int Id { get; set; }
        public DateTime FirstAdded { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        private List<Borrow> _borrows = new List<Borrow>();
        public IReadOnlyList<Borrow> Borrows => _borrows;
    }
}
