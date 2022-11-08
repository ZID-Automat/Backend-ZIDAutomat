using ZID.Automat.Domain.Models;
using System.Linq;
using ZID.Automat.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ZID.Automat.Repository
{
    public class ItemRepository : IGetItemWithItemInstance, IGetPrevBorrowedItemsOfUser
    {
        private readonly AutomatContext _context;
        public ItemRepository(AutomatContext automatContext)
        {
            _context = automatContext;
        }

        public List<Item> GetBorrowedItemsOfUser(int UserId)
        {
            List<Item> Items = _context.ItemInstances.Include(I => I.Borrows).Where(I => I.Borrows.FirstOrDefault(bor => bor.UserId == UserId) != null).Select(II => II.Item).ToList();
            return Items;
        }

        public List<Item> GetItemWithItemInstance()
        {
            return _context.Items.Include(i => i.ItemInstances).ToList();
        }
    }
    
    public interface IGetItemWithItemInstance
    {
        public List<Item> GetItemWithItemInstance();
    }

    public interface IGetPrevBorrowedItemsOfUser
    {
        public List<Item> GetBorrowedItemsOfUser(int UserId);
    }
}