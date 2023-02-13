using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Domain.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SubName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal Price { get; set; }

        //noch besser machen(historisierung)
        public string LocationImAutomat { get; set; } = string.Empty;

        public Categorie Categorie = default!;
        public int CategorieId;

        private List<ItemInstance> _ItemInstances { get; set; } = new List<ItemInstance>();
        public IReadOnlyList<ItemInstance> ItemInstances => _ItemInstances;

        private List<Borrow> _borrows { get; set; } = new List<Borrow>();
        public IReadOnlyList<Borrow> Borrows => _borrows;


        /// <summary>
        /// Adds a new ItemInstance to the Item
        /// </summary>
        /// <param name="itemInstance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddItemInstance(ItemInstance itemInstance)
        {
            if(itemInstance == null)
            {
                throw new ArgumentNullException();
            }
            if (Math.Abs((DateTime.Now - itemInstance.FirstAdded).TotalHours) > 1)
            {
                throw new ArgumentException("older than 1 hour");
            }

            _ItemInstances.Add(itemInstance);
        }

        public void AddBorrow(Borrow borrow, DateTime now)
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
