using AutoMapper;
using Bogus.DataSets;
using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;
using ZID.Automat.Domain.Models.Logging;
using ZID.Automat.Dto.Models.Analytics;
using ZID.Automat.Dto.Models.Analytics.User;
using ZID.Automat.Repository;

namespace ZID.Automat.Application
{
    public interface IAdminLogShowSerivice
    {
        IEnumerable<LogQrCodeAdminDto> EjectedItems();
        IEnumerable<LogQrCodeAdminDto> InvalidItems();
        IEnumerable<LogQrCodeAdminDto> ScannedItems();
    }

    public class AdminLogShowSerivice : IAdminLogShowSerivice
    {
        private readonly IRepositoryRead _read;
        private readonly IMapper _mapper;

        public AdminLogShowSerivice(IRepositoryRead read, IMapper mapper)
        {
            _read = read;
            _mapper = mapper;
        }

        public IEnumerable<LogQrCodeAdminDto> EjectedItems()
        {
            return getLogs<EjectedItemLog>();
        }

        public IEnumerable<LogQrCodeAdminDto> InvalidItems()
        {
            return getLogs<InvalidQRCodeLog>();
        }


        public IEnumerable<LogQrCodeAdminDto> ScannedItems()
        {
            return getLogs<ScannedQRCodeLog>();
        }


        private IEnumerable<LogQrCodeAdminDto> getLogs<T>() where T : BaseLogQrCode
        {
            IEnumerable<T> data = _read.GetAll<T>();
            return _mapper.Map<IEnumerable<BaseLogQrCode>, IEnumerable<LogQrCodeAdminDto>>(data.ToList());
        }
    }


}
