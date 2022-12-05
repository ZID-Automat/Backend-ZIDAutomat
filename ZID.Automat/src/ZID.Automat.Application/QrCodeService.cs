using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Repository;

namespace ZID.Automat.Application
{
    public class QrCodeService : IQrCodeCService, IQrCodeUService
    {
        private readonly IActiveBorrowsRepository _activeBorrowsRepository;
        private readonly IAlllBorrowsRepository _alllBorrowsRepository;
        private readonly IUserRepository _userRepository;
        private readonly IControllerQrCodeRepository _cQrCodeRepository;
        private readonly ISaveDBRepository _saveRepository;
        private readonly IItemRepository _itemRepository;
        public QrCodeService(IUserRepository userRepository, IActiveBorrowsRepository activeBorrowsRepository, IAlllBorrowsRepository alllBorrowsRepository, IControllerQrCodeRepository cQrCodeRepository,ISaveDBRepository saveRepository,IItemRepository itemRepository)
        {
            _activeBorrowsRepository = activeBorrowsRepository;
            _alllBorrowsRepository = alllBorrowsRepository;
            _userRepository = userRepository;
            _saveRepository = saveRepository;
            _cQrCodeRepository = cQrCodeRepository;
            _itemRepository = itemRepository;          
        }

        public ValidQrCodeDto IsValidQrCode(QrCodeDto qrCode)
        {
            var borrow = _cQrCodeRepository.isValidQrCode(qrCode.QRCode);
            return new ValidQrCodeDto() { valid = borrow == null?false:borrow.CollectDate == null, ItemId = borrow?.Item.Id??0 };
        }
        
        public void InvalidateQrCode(InvalidateQrCodeDto InvalidateQrCode,DateTime now)
        {   
            var bo = _cQrCodeRepository.getBorrow(InvalidateQrCode.QrCode)?? throw new ArgumentException("No Borrow with that qrCode");
            bo.ItemInstance = _itemRepository.getItemInstance(InvalidateQrCode.ItemInstanceId) ?? throw new ArgumentException("ItemInstance ist nicht gefunden worden");
            bo.CollectDate = now;
            _saveRepository.SaveDb();
        } 
        
        public IEnumerable<BorrowDto> OpenQrCodes()
        {
            return _activeBorrowsRepository.getActiveBorrows().Select(b => new BorrowDto()
            {
                BorrowDate = b.BorrowDate,
                CollectDate = b.CollectDate,
                DueDate = b.PredictedReturnDate,
                ItemId = b.Id,
                ItemInstanceId = b.Item.Id,
                ItemName = b.Item.Name,
                ReturnDate = b.ReturnDate
            });
        }

        public IEnumerable<BorrowDto> AllQrCodes()
        {
            return _alllBorrowsRepository.getAllBorrows().Select(b => new BorrowDto()
            {
                BorrowDate = b.BorrowDate,
                CollectDate = b.CollectDate,
                DueDate = b.PredictedReturnDate,
                ItemId = b.Id,
                ItemInstanceId = b.Item.Id,
                ItemName = b.Item.Name,
                ReturnDate = b.ReturnDate
            });
        }

        public int OpenQrCodesCount()
        {
            return _activeBorrowsRepository.getActiveBorrowsCount();
        }
    }

    public interface IQrCodeCService
    {
        public ValidQrCodeDto IsValidQrCode(QrCodeDto qrCode);
        public void InvalidateQrCode(InvalidateQrCodeDto InvalidateQrCode, DateTime now);
    }
        
    public interface IQrCodeUService
    {
        public IEnumerable<BorrowDto> OpenQrCodes();
        public IEnumerable<BorrowDto> AllQrCodes();
        public int OpenQrCodesCount();
    }
}
