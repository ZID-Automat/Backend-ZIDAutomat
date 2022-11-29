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

        public ItemInstance? getItemInstance(int IIID) => _context.ItemInstances.SingleOrDefault(II => II.Id == IIID);
        public Item? getItem(int ItemId) => _context.Items.SingleOrDefault(i => i.Id == ItemId);
        public IReadOnlyList<Item> getItemWithItemInstance() =>_context.Items.Include(i => i.ItemInstances).ToList();
        public IReadOnlyList<Item> getPrevBorrowedItemsOfUser(int UserId) =>_context.Borrows.Include(b => b.ItemInstance.Item).Where(b => b.UserId == UserId).Select(s => s.ItemInstance.Item).Distinct().ToList();
        public bool isItemAvalable(int ItemID, DateTime t) => _context.ItemInstances.Include(II => II.Borrows).DefaultIfEmpty().Where(II => II.ItemId == ItemID && (II.Borrows.Count() != 0? II.Borrows.OrderBy(B => B.ReturnDate).First().ReturnDate < t:true)).FirstOrDefault() != null;

        public IReadOnlyList<ItemInstance> getFreeItemInstances(int itemId, DateTime t) => _context.ItemInstances.Where(II => II.ItemId == itemId && ((II.Borrows.Count() != 0)?II.Borrows.OrderBy(B => B.ReturnDate).First().ReturnDate < t:true)).ToList();
        public ItemInstance? getFreeItemInstance(int itemId,DateTime t) => _context.ItemInstances.Where(II => II.ItemId == itemId && ((II.Borrows.Count() != 0) ? II.Borrows.OrderBy(B => B.ReturnDate).First().ReturnDate < t : true)).FirstOrDefault();

        public int? loadItemFromQrCode(string QrCode) => _context.Borrows.Include(b=>b.ItemInstance).Where(b => b.UUID == QrCode).SingleOrDefault()?.ItemInstance.ItemId;
    }

    public interface IItemRepository
    {
        public Item? getItem(int ItemId);
        public int? loadItemFromQrCode(string QrCode);
        public IReadOnlyList<Item> getItemWithItemInstance();
        public IReadOnlyList<Item> getPrevBorrowedItemsOfUser(int UserID);

        public bool isItemAvalable(int ItemId,DateTime t);

        public IReadOnlyList<ItemInstance> getFreeItemInstances(int itemId, DateTime t);
        public ItemInstance? getFreeItemInstance(int itemId, DateTime t);
        public ItemInstance? getItemInstance(int IIID);

    }
}