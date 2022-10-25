using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Domain.Models
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public DateTime Joined { get; set; }

        private List<Borrow> _Borrow { get; set; } = new List<Borrow>();
        public IReadOnlyList<Borrow> Borrow => _Borrow;

        public void AddBorrow(Borrow borrow)
        {
            if (borrow == null)
                throw new ArgumentNullException("Can't add new Borrow, because it is null");

            if (Math.Abs((DateTime.Now - borrow.BorrowDate).TotalHours) > 1)
                throw new ArgumentException("Can't add new Borrow, because it is older than 1 hour");

            if (borrow.PredictedReturnDate < borrow.BorrowDate)
                throw new ArgumentException("Can't add new Borrow, because PredictedReturnDate is older than BorrowDate");
            
            if(borrow.ReturnDate != null)
                throw new ArgumentException("Can't add new Borrow, because ReturnDate is set from the beginning");

            _Borrow.Add(borrow);
        }
    }
}
