using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models.Analytics.User;
using ZID.Automat.Dto.Models.Items;
using ZID.Automat.Exceptions;
using ZID.Automat.Repository;

namespace ZID.Automat.Application.Admin
{
    public interface IAdminItemService
    {
        void AddItemDetailed(ItemAdminUpdateAdd data);
        IEnumerable<ItemGetAllDto> GetAllItems();
        ItemAdminDetailedDto ItemDetailedGet(int id);
        void SetItemPosition(ItemChangeLocationDto data);
        void UpdateItemDetailed(ItemAdminUpdateAdd data);
        public int GetItemInstances(int itemId);
        public void AddItemInstace(int itemId);


    }

    public class AdminItemService : IAdminItemService
    {
        private readonly IRepositoryRead _repositoryRead;
        private readonly IRepositoryWrite _repositoryWrite;

        private readonly IMapper _mapper;

        public AdminItemService(IRepositoryRead repositoryRead, IMapper mapper, IRepositoryWrite repositoryWrite)
        {
            _repositoryRead = repositoryRead;
            _mapper = mapper;
            _repositoryWrite = repositoryWrite;
        }

        public IEnumerable<ItemGetAllDto> GetAllItems()
        {
            return _repositoryRead.GetAll<Item>().ToList().Select(i => new ItemGetAllDto() { Id = i.Id, Image = i.Image, LocationImAutomat = i.LocationImAutomat, Name = i.Name });
        }

        public void SetItemPosition(ItemChangeLocationDto data)
        {
            var items = _repositoryRead.GetAll<Item>();
            items.ToList().ForEach(i =>
            {
                i.LocationImAutomat = data.ItemLocations.FirstOrDefault(itemloc => itemloc.Id == i.Id)?.Location ?? "";
            });
            _repositoryWrite.Update(items);
        }

        public ItemAdminDetailedDto ItemDetailedGet(int id)
        {
            var item = _repositoryRead.FindById<Item>(id);
            var mapped = _mapper.Map<Item, ItemAdminDetailedDto>(item);

            var borrows = _repositoryRead.GetAll<Borrow>().Include(i => i.ItemInstance).Include(i => i.ItemInstance.Item);
            var bi = borrows.Where(b => b.ItemInstance.ItemId == item.Id).ToList();
                mapped.Borrows = _mapper.Map<IEnumerable<Borrow>, IEnumerable<UserAdmiBorrowDto>>(bi);
            return mapped;
        }

        public void UpdateItemDetailed(ItemAdminUpdateAdd data)
        {
            var item = _repositoryRead.FindById<Item>(data.Id);
            item.SubName = data.SubName;
            item.Price = data.Price;
            item.CategorieId = data.CategorieId;
            item.Description = data.Description;
            item.Image = data.Image;
            item.Name = data.Name;
            _repositoryWrite.Update(item);
        }

        public void AddItemDetailed(ItemAdminUpdateAdd data)
        {
            var item = new Item();
            item.SubName = data.SubName;
            item.Price = data.Price;
            item.CategorieId = data.CategorieId;
            item.Description = data.Description;
            item.Image = data.Image;
            item.Name = data.Name;
            _repositoryWrite.Add(item);
        }

        public int GetItemInstances(int itemId)
        {
            var item = _repositoryRead.FindById<Item>(itemId);
            var count = item.ItemInstances.Count(II => II.borrow == null);
            return count;
        }

        public void AddItemInstace(int itemId)
        {
            var item = _repositoryRead.FindById<Item>(itemId);
            item.ItemInstances.Add(new ItemInstance() { FirstAdded = DateTime.Now });
            _repositoryWrite.Update(item);
        }
    }
}
