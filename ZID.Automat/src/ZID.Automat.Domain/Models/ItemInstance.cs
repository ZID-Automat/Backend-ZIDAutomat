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

        public Borrow? _borrows { get; set; } = default;
    }
}
