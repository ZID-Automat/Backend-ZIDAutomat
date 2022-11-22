using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Domain.Models
{
    public class Borrow
    {
        public int Id { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime PredictedReturnDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public string UUID { get; set; } = string.Empty;

        public int ItemInstanceId { get; set; }
        public ItemInstance ItemInstance { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public int AdmonitionId { get; set; }
       
        private List<Admonition> _Admonition { get; set; } = new List<Admonition>();
        public IReadOnlyList<Admonition> Admonition => _Admonition;

        public void AddAdmonition(Admonition admonition)
        {
            if (admonition == null)
                throw new ArgumentNullException("Can't add new Admonition, because it is null");

            if (admonition.AdmonitionDate < BorrowDate)
                throw new ArgumentException("Can't add new Admonition, because it is older than BorrowDate");

            if(admonition.AdmonitionType == null)
            {
                throw new ArgumentException("Can't add new Admonition, because AdmonitionType is null");
            }
            _Admonition.Add(admonition);
        }
    }
}
