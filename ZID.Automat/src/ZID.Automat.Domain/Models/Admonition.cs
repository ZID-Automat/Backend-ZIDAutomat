using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Domain.Models
{
    public class Admonition
    {
        public int Id { get; set; }
        public DateTime AdmonitionDate { get; set; }
        public string Comment { get; set; } = string.Empty;

        public int BorrowId { get; set; }
        public Borrow Borrow { get; set; } = null!;

        public int AdmonitionTypeId { get; set; }
        public AdmonitionType AdmonitionType { get; set; } = null!;

    }
}
