using AutoMapper;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Dto.Models.Analytics.User;

namespace ZID.Automat.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserAdminGetAll>()
             .ForMember(dest => dest.BorrowCount, opt => opt.Ignore());


            CreateMap<User, UserAdminDetailedDto>()
              .ForMember(dest => dest.Borrow, opt => opt.Ignore());


        }
    }
}