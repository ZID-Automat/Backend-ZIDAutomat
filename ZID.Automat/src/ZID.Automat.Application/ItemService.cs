using ZID.Automat.Domain.Models;
using ZID.Automat.Dto;
using ZID.Automat.Repository;
using System.Linq;
using ZID.Automat.Dto.Models;
using AutoMapper;
using ZID.Automat.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ZID.Automat.Application
{
    public class ItemService: IItemService
    {
        private readonly IRepositoryRead _repositoryRead;
        private readonly IRepositoryWrite _repositoryWrite;
        private readonly IMapper _mapper;

        public ItemService(IRepositoryRead repositoryRead, IRepositoryWrite repositoryWrite, IMapper mapper)
        {
            _repositoryRead = repositoryRead;
            _repositoryWrite = repositoryWrite;
            _mapper = mapper;
        }

        public IEnumerable<ItemDisplayDto> AllDisplayItems()
        {
            IEnumerable<Item> Items = _repositoryRead.GetAll<Item>().Where(i => i.LocationImAutomat != "");
            return _mapper.Map<IEnumerable<Item>, IEnumerable<ItemDisplayDto>>(Items.ToList());
        }

        public IEnumerable<ItemDisplayDto> PrevBorrowedDisplayItemsUser(string UserName)
        {   
            IEnumerable<Item> items = _repositoryRead.GetAll<Borrow>().Include(b=>b.User).Include(b=>b.ItemInstance).Include(b=>b.ItemInstance.Item).Where(b => b.ItemInstance != null && b.ItemInstance.Item != null && b.User.Name == UserName).Select(b => b.ItemInstance.Item).Distinct().Where(i => i.LocationImAutomat != "");
            return _mapper.Map<IEnumerable<ItemDisplayDto>>(items.ToList());
        }

        public IEnumerable<ItemDisplayDto> PopularItems()
        {
            var Borrows = _repositoryRead.GetAll<Borrow>().Include(i => i.ItemInstance).Include(i => i.ItemInstance.Item).Where(i =>i.ItemInstance != null && i.ItemInstance.Item!= null && i.ItemInstance.Item.LocationImAutomat != "").ToList().GroupBy(b => b.ItemInstance.Item).OrderByDescending(b => b.Key.Id).OrderByDescending(b => b.Count(b => b.BorrowDate > DateTime.Now.AddDays(-7))).Take(10).Select(b => b.FirstOrDefault().ItemInstance.Item);
            var rest = _repositoryRead.GetAll<Item>().Where(i => i.LocationImAutomat != "").ToList().Except(Borrows);
            Borrows = Borrows.Concat(rest).ToList();
            return _mapper.Map<IEnumerable<ItemDisplayDto>>(Borrows);
        }

        public ItemDetailedDto DetailedItem(int ItemId)
        {
            var item = _repositoryRead.FindById<Item>(ItemId);
            return _mapper.Map<ItemDetailedDto>(item);
        }
        public ItemDetailedDto DetailedItem(Guid QrCode)
        {
            var item = (_repositoryRead.GetAll<Borrow>().Where(b => b.GUID == QrCode).SingleOrDefault() ?? throw new QrCodeNotExistingException())?.ItemInstance?.Item;
            return _mapper.Map<ItemDetailedDto>(item);
        }

     
    }
    
    public interface IItemService
    {
        public IEnumerable<ItemDisplayDto> AllDisplayItems();
        public IEnumerable<ItemDisplayDto> PrevBorrowedDisplayItemsUser(string UserName);
        public ItemDetailedDto DetailedItem(int ItemId);
        public ItemDetailedDto DetailedItem(Guid QrCode);
        public IEnumerable<ItemDisplayDto> PopularItems();
    }
}
