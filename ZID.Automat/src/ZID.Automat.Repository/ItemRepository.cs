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

        public List<Item> getItemWithItemInstance()
        {
            return _context.Items.Include(i => i.ItemInstances).ToList();
        }
    }

    public interface IItemRepository
    {
        public List<Item> getItemWithItemInstance();
    }
}