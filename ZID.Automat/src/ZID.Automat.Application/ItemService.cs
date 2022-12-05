using ZID.Automat.Domain.Models;
using ZID.Automat.Dto;
using ZID.Automat.Repository;
using System.Linq;
using ZID.Automat.Dto.Models;

namespace ZID.Automat.Application
{
    public class ItemService: IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        public ItemService(IItemRepository itemRepository,IUserRepository userRepository)
        {
            _itemRepository = itemRepository;
            _userRepository = userRepository;
        }
                
        public IReadOnlyList<ItemDisplayDto> AllDisplayItems()
        {
            IReadOnlyList<Item> Items = _itemRepository.getItemWithItemInstance();
            List<ItemDisplayDto> itemDisplays = Items.Select(item => new ItemDisplayDto()
            {
                Available = _itemRepository.isItemAvalable(item.Id, DateTime.Now),
                Name = item.Name,
                Description = item.Description,
                Image = item.Image,
                SubName = item.SubName,
                Id = item.Id
            }).ToList();   
            return itemDisplays;
        }

        public IReadOnlyList<ItemDisplayDto> PrevBorrowedDisplayItemsUser(string UserName)
        {
            var User = _userRepository.FindUser(UserName) ?? throw new ArgumentNullException("No User with this UserName");
            IReadOnlyList<Item> Items = _itemRepository.getPrevBorrowedItemsOfUser(User.Id);
            
            List<ItemDisplayDto> itemDisplays = Items.Select(item => new ItemDisplayDto()
            {
                Available = _itemRepository.isItemAvalable(item.Id,DateTime.Now),
                Name = item?.Name ?? string.Empty,
                Description = item?.Description ?? string.Empty,
                Image = item?.Image??string.Empty,
                SubName = item?.SubName??string.Empty,
                Id = item?.Id??-1
            }).ToList();
            return itemDisplays;
        }

        public ItemDetailedDto DetailedItem(int ItemId)
        {
            var item = _itemRepository.getItem(ItemId);

            return new ItemDetailedDto()
            {
                Available = _itemRepository.isItemAvalable(ItemId,DateTime.Now),
                Name = item.Name??"Name",
                Description = item.Description,
                Image = item.Image,
                SubName = item.SubName,
                Categorie = item.Categorie?.Name??"Kabel",
                Price = item.Price,
                Id = item.Id
            };
        }
        public ItemDetailedDto DetailedItem(string QrCode)
        {
            int id = _itemRepository.loadItemFromQrCode(QrCode)?? throw new ArgumentException("Item not found");
            var item = _itemRepository.getItem(id);
            return new ItemDetailedDto()
            {
                Available = _itemRepository.isItemAvalable(id, DateTime.Now),
                Name = item.Name ?? "Name",
                Description = item.Description,
                Image = item.Image,
                SubName = item.SubName,
                Categorie = item.Categorie?.Name ?? "Kabel",
                Price = item.Price,
                Id = item.Id
            };
        }
    }

    public interface IItemService
    {
        public IReadOnlyList<ItemDisplayDto> AllDisplayItems();
        public IReadOnlyList<ItemDisplayDto> PrevBorrowedDisplayItemsUser(string UserName);
        public ItemDetailedDto DetailedItem(int ItemId);

        public ItemDetailedDto DetailedItem(string QrCode);
    }
}
