using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models.Items;
using ZID.Automat.Exceptions;
using ZID.Automat.Repository;

namespace ZID.Automat.Application.Admin
{
    public interface IAdminItemService
    {
        IEnumerable<ItemGetAllDto> GetAllItems();
        void SetItemPosition(ItemChangeLocationDto data);
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
            return _repositoryRead.GetAll<Item>().Select(i => new ItemGetAllDto() { Id = i.Id, Image = i.Image, LocationImAutomat = i.LocationImAutomat, Name = i.Name });
        }

        public void SetItemPosition(ItemChangeLocationDto data)
        {
            var items = _repositoryRead.GetAll<Item>();
            items.ToList().ForEach(i =>
            {
                i.LocationImAutomat= data.ItemLocations.FirstOrDefault(itemloc => itemloc.Id == i.Id)?.Location ?? "";
            });
            _repositoryWrite.Update(items);
        }
    }
}
