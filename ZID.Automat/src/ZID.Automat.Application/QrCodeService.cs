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

namespace ZID.Automat.Application
{
    public class QrCodeService : IQrCodeCService, IQrCodeUService
    {
        private readonly IRepositoryRead _repositoryRead;
        private readonly IRepositoryWrite _repositoryWrite;
        private readonly IMapper _mapper;

        public QrCodeService(IRepositoryRead repositoryRead, IRepositoryWrite repositoryWrite, IMapper mapper)
        {
            _repositoryRead = repositoryRead;
            _repositoryWrite = repositoryWrite;
            _mapper = mapper;
        }

        public ValidQrCodeDto IsValidQrCode(QrCodeDto qrCode)
        {
            var borrow = (_repositoryRead.GetAll<Borrow>().Where(b => b.GUID == qrCode.QRCode).SingleOrDefault() ?? throw new QrCodeNotExistingException());
            return new ValidQrCodeDto() { valid = borrow.CollectDate == null, ItemId = borrow?.ItemInstance.Item.Id??0 };
        }
        
        public void InvalidateQrCode(InvalidateQrCodeDto InvalidateQrCode,DateTime now)
        {   
            var borrow = (_repositoryRead.GetAll<Borrow>().Where(b => b.GUID == InvalidateQrCode.QrCode).SingleOrDefault() ?? throw new QrCodeNotExistingException());
            borrow.CollectDate = DateTime.Now;
            _repositoryWrite.Update(borrow);
        } 
        
        public IEnumerable<BorrowDto> OpenQrCodes(string cn)
        {
            var borrows = _repositoryRead.GetAll<Borrow>().Where(b => b.ReturnDate == null && b.User.Name == cn);
            return _mapper.Map<IEnumerable<BorrowDto>>(borrows);
        }

        public IEnumerable<BorrowDto> AllQrCodes(string  cn)
        {
            var borrows = _repositoryRead.GetAll<Borrow>().Where(b => b.User.Name == cn);
            return _mapper.Map<IEnumerable<BorrowDto>>(borrows);
        }

        public int OpenQrCodesCount(string cn)
        {
            return _repositoryRead.GetAll<Borrow>().Where(b=>b.User.Name == cn).Count();
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
