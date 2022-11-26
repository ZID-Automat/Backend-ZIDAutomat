using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;
using ZID.Automat.Infrastructure;

namespace ZID.Automat.Repository
{
    public class BorrowRepository: IActiveBorrowsRepository
    {
        private readonly AutomatContext _context;

        public BorrowRepository(AutomatContext automatContext)
        {
            _context = automatContext;
        }

        public IEnumerable<Borrow> getActiveBorrows()
        {
            return _context.Borrows.Include(b=>b.ItemInstance).Include(b=>b.ItemInstance.Item).Where(b => b.CollectDate == null).ToList();
        }
    }

    public interface IActiveBorrowsRepository
    {
        public IEnumerable<Borrow> getActiveBorrows();
    }
}
