using AutoMapper;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;

namespace ZID.Automat.AutoMapper
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDisplayDto>()
             .ForMember(dest => dest.Available, opt => opt.MapFrom(src => src.ItemInstances.Any(item => item.borrow == null)));


            CreateMap<Item, ItemDetailedDto>()
              .ForMember(dest => dest.Available, opt => opt.MapFrom(src => src.ItemInstances.Any(item => item.borrow == null)))
              .ForMember(dest => dest.Categorie, opt => opt.MapFrom(src => src.Categorie.Name));
        }
    }
}