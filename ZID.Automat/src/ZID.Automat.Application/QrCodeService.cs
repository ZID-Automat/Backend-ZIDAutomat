using AutoMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Exceptions;
using ZID.Automat.Repository;
using Microsoft.EntityFrameworkCore;

namespace ZID.Automat.Application
{
    public class QrCodeService : IQrCodeCService, IQrCodeUService
    {
        private readonly IRepositoryRead _repositoryRead;
        private readonly IRepositoryWrite _repositoryWrite;
        private readonly IMapper _mapper;
        private readonly IAutomatLoggingService _automatLoggingService;

        public QrCodeService(IRepositoryRead repositoryRead, IRepositoryWrite repositoryWrite, IMapper mapper, IAutomatLoggingService automatLoggingService)
        {
            _repositoryRead = repositoryRead;
            _repositoryWrite = repositoryWrite;
            _mapper = mapper;
            _automatLoggingService = automatLoggingService;
        }

        public ValidQrCodeDto IsValidQrCode(QrCodeDto qrCode)
        {
            Guid guid;
            Borrow? borrow = null;
            bool isGuid = Guid.TryParse(qrCode.QRCode, out guid);
            if (isGuid)
            {
                borrow = (_repositoryRead.GetAll<Borrow>().Where(b => b.GUID == guid).SingleOrDefault());
                bool? valid = borrow?.IsValid();
                if (!(valid ?? false))
                {
                    _automatLoggingService.LogInvaldScannedQrCode(qrCode.QRCode, borrow);
                }
                if (borrow == null)
                {

                }
            }

            _automatLoggingService.LogScannedQrCode(qrCode.QRCode, borrow);

            if (!isGuid)
            {
                return new ValidQrCodeDto()
                {
                    valid = false,
                    ItemId = 0,
                    Message="QRCode",
                    Message2="nicht valid"

                };
            }
            if (borrow == null)
            {
                return new ValidQrCodeDto()
                {
                    valid = false,
                    ItemId = 0,
                    Message = "Keine Ausleihe",
                    Message2 = "mit diesem QRCode"
                };
            }
            if(!(borrow.BorrowDate.AddHours(1) >= DateTime.Now))
            {
                return new ValidQrCodeDto()
                {
                    valid = false,
                    ItemId = 0,
                    Message = "Qr Code",
                    Message2 = "abgelaufen"
                };
            }
            if ((borrow.CollectDate != null))
            {
                return new ValidQrCodeDto()
                {
                    valid = false,
                    ItemId = 0,
                    Message = "Qr Code",
                    Message2 = "breits verwended"
                };
            }


            return new ValidQrCodeDto() { valid = borrow?.IsValid() ?? false, ItemId = borrow?.ItemInstance?.Item.Id ?? 0 };
        }
        
        public void InvalidateQrCode(InvalidateQrCodeDto InvalidateQrCode,DateTime now)
        {   
            var borrow = (_repositoryRead.GetAll<Borrow>().Where(b => b.GUID.ToString()== InvalidateQrCode.QrCode).SingleOrDefault() ?? throw new QrCodeNotExistingException());
            borrow.CollectDate = DateTime.Now;
            _repositoryWrite.Update(borrow);
            _automatLoggingService.EjectedItem(InvalidateQrCode.QrCode, borrow);
        } 
        
        public IEnumerable<BorrowDto> OpenQrCodes(string cn)
        {
            var borrows = _repositoryRead.GetAll<Borrow>().Where(b => (b.CollectDate == null && b.BorrowDate.AddHours(1) >= DateTime.Now )&& b.User.Name == cn);
            return _mapper.Map<IEnumerable<BorrowDto>>(borrows.ToList());
        }

        public IEnumerable<BorrowDto> AllQrCodes(string  cn)
        {
            var borrows = _repositoryRead.GetAll<Borrow>().Where(b => b.User.Name == cn).ToList();
            var toRemove = borrows.Where(b => b.CollectDate == null && b.BorrowDate.AddHours(1) < DateTime.Now).ToList();
            _repositoryWrite.Delete<Borrow>(toRemove);
           
            foreach(var b in toRemove)
            {
                borrows.Remove(b);
            }

            return _mapper.Map<IEnumerable<BorrowDto>>(borrows.ToList());
        }

        public int OpenQrCodesCount(string cn)
        {
            //is valid
            return _repositoryRead.GetAll<Borrow>().Include(b=>b.User).Where(b=>b.User.Name == cn && (b.CollectDate == null && b.BorrowDate.AddHours(1) >= DateTime.Now)).ToList().Count();
        }

        public ControllerItemLocationDto ItemLocation(int itemId)
        {
            var item = _repositoryRead.FindById<Item>(itemId) ?? throw new NotFoundException("Item");
            return new ControllerItemLocationDto() { ItemId = itemId, location = item.LocationImAutomat };
        }
    }

    public interface IQrCodeCService
    {
        public ValidQrCodeDto IsValidQrCode(QrCodeDto qrCode);
        public void InvalidateQrCode(InvalidateQrCodeDto InvalidateQrCode, DateTime now);

        public ControllerItemLocationDto ItemLocation(int itemId);
    }
        
    public interface IQrCodeUService
    {
        public IEnumerable<BorrowDto> OpenQrCodes(string cn);
        public IEnumerable<BorrowDto> AllQrCodes(string cn);
        public int OpenQrCodesCount(string cn);
    }
}
