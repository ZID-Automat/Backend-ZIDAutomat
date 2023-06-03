using AutoMapper;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Dto.Models.Analytics.User;

namespace ZID.Automat.AutoMapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Categorie, CategoryUpdateDto>();

            CreateMap<CategoryAddDto, Categorie>();
        }
    }
}