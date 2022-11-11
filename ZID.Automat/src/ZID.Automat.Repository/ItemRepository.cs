using ZID.Automat.Domain.Models;
using ZID.Automat.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ZID.Automat.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly AutomatContext _context;
        public ItemRepository(AutomatContext automatContext)
        {
            _context = automatContext;
        }

        public IReadOnlyList<Item> getItemWithItemInstance() =>_context.Items.Include(i => i.ItemInstances).ToList();

        public IReadOnlyList<Item> getPrevBorrowedItemsOfUser(int UserId) =>_context.Borrows.Include(b => b.ItemInstance.Item).Where(b => b.UserId == UserId).Select(s => s.ItemInstance.Item).ToList();
    }

    public interface IItemRepository
    {
        public IReadOnlyList<Item> getItemWithItemInstance();
        public IReadOnlyList<Item> getPrevBorrowedItemsOfUser(int UserID);
    }
}