using AutoMapper;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Dto.Models.Analytics.User;

namespace ZID.Automat.AutoMapper
{
    public class BorrowProfile : Profile
    {
        public BorrowProfile()
        {
            CreateMap<Borrow, BorrowDto>()
                .ForMember(dest => dest.ItemInstanceId, opt => opt.MapFrom(src => src.ItemInstance!.Id))
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemInstance!.Item.Id))
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.ItemInstance!.Item.Name));


            CreateMap<Borrow, UserAdmiBorrowDto>()
                .ForMember(dest => dest.Itemname, opt => opt.MapFrom(src => (src.ItemInstance != null) ? src.ItemInstance!.Item.Name : ""))
                .ForMember(dest => dest.Stati, opt => opt.Ignore());
        }
    }
}