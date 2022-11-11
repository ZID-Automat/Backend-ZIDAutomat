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
                Available = item.ItemInstances.Count() != 0,
                Name = item.Name,
                Description = item.Description,
                Image = item.Image,
                SubName = item.SubName
            }).ToList();
            return itemDisplays;
        }

        public IReadOnlyList<ItemDisplayDto> PrevBorrowedDisplayItemsUser(string UserName)
        {
            IReadOnlyList<Item> Items = _itemRepository.getPrevBorrowedItemsOfUser(_userRepository.FindUser(UserName)?.Id??throw new ArgumentNullException("No User with this UserName"));
            List<ItemDisplayDto> itemDisplays = Items.Select(item => new ItemDisplayDto()
            {
                Available = item.ItemInstances.Count() != 0,
                Name = item.Name,
                Description = item.Description,
                Image = item.Image,
                SubName = item.SubName
            }).ToList();
            return itemDisplays;
        }
    }

    public interface IItemService
    {
        public IReadOnlyList<ItemDisplayDto> AllDisplayItems();
        public IReadOnlyList<ItemDisplayDto> PrevBorrowedDisplayItemsUser(string UserName);
    }
}
