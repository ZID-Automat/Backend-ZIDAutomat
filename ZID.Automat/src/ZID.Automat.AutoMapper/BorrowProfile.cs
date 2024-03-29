﻿using AutoMapper;
using ZID.Automat.Domain.Models;
using ZID.Automat.Domain.Models.Logging;
using ZID.Automat.Dto.Models;
using ZID.Automat.Dto.Models.Analytics;
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
                .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.ItemInstance!.Item.Name))
                .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate))
                .ForMember(dest => dest.CollectDate, opt => opt.MapFrom(src => src.CollectDate))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.PredictedReturnDate))
                .ForMember(dest => dest.BorrowDate, opt => opt.MapFrom(src => src.BorrowDate))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.Status()));
            


            CreateMap<Borrow, UserAdmiBorrowDto>()
                .ForMember(dest => dest.Itemname, opt => opt.MapFrom(src => (src.ItemInstance != null) ? src.ItemInstance!.Item.Name : ""))
                .ForMember(dest => dest.Stati, opt => opt.MapFrom(src=>src.Status()));


         


            CreateMap<Borrow, BorrowAdminDetailedDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ItemInstance.Item.Name))
                .ForMember(dest => dest.SubName, opt => opt.MapFrom(src => src.ItemInstance.Item.SubName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemInstance.ItemId))
                .ForMember(dest => dest.late, opt => opt.MapFrom(b => b.PredictedReturnDate < b.ReturnDate || b.PredictedReturnDate < DateTime.Now));


            CreateMap<BaseLogQrCode, LogQrCodeAdminDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BorrowId, opt => {
                    opt.MapFrom(src => src.Borrow.Id);
                    opt.Condition(src => src.Borrow != null);
                 });


            
        }
    }
}
