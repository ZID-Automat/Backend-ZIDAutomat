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

        public void AddBorrow(Borrow borrow,DateTime now)
        {
            if (borrow == null)
                throw new ArgumentNullException("Can't add new Borrow, because it is null");

            if (Math.Abs((now - borrow.BorrowDate).TotalHours) > 1)
                throw new ArgumentException("Can't add new Borrow, because it is older than 1 hour");

            if (borrow.PredictedReturnDate < borrow.BorrowDate)
                throw new ArgumentException("Can't add new Borrow, because PredictedReturnDate is older than BorrowDate");

            if (borrow.ReturnDate != default(DateTime) && borrow.ReturnDate != null)
                throw new ArgumentException("Can't add new Borrow, because ReturnDate is set from the beginning");

            _borrows.Add(borrow);
        }
    }
}
