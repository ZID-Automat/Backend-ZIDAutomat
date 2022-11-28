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
    public class BorrowRepository : IActiveBorrowsRepository, IAlllBorrowsRepository,IControllerQrCodeRepository
    {
        private readonly AutomatContext _context;

        public BorrowRepository(AutomatContext automatContext)
        {
            _context = automatContext;
        }

        public IEnumerable<Borrow> getActiveBorrows()
        {
            return _context.Borrows.Include(b => b.ItemInstance).Include(b => b.Item).Where(b => b.CollectDate == null).OrderBy(b => b.BorrowDate).ToList();
        }

        public IEnumerable<Borrow> getAllBorrows()
        {
            return _context.Borrows.Include(b => b.ItemInstance).Include(b => b.Item).OrderBy(b => b.BorrowDate).ToList();
        }

        public int getActiveBorrowsCount()
        {
            return _context.Borrows.Include(b => b.ItemInstance).Include(b => b.Item).Where(b => b.CollectDate == null).OrderBy(b => b.BorrowDate).Count();
        }

        public Borrow? isValidQrCode(string UUID)
        {
            return _context.Borrows.Include(b=>b.ItemInstance).Include(b=>b.Item).SingleOrDefault(b => b.UUID == UUID && b.CollectDate == null);
        }

        public Borrow? getBorrow(string UUID)
        {
            return _context.Borrows.SingleOrDefault(b => b.UUID == UUID);
        }
    }

    public interface IActiveBorrowsRepository
    {
        public IEnumerable<Borrow> getActiveBorrows();
        public int getActiveBorrowsCount();
    }

    public interface IAlllBorrowsRepository
    {
        public IEnumerable<Borrow> getAllBorrows();
    }
    
    public interface IControllerQrCodeRepository
    {
        public Borrow? isValidQrCode(string UUID);
        public Borrow? getBorrow(string UUID);
    }
}
