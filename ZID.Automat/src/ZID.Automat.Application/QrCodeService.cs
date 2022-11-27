using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public QrCodeService(IUserRepository userRepository, IActiveBorrowsRepository activeBorrowsRepository, IAlllBorrowsRepository alllBorrowsRepository, IControllerQrCodeRepository cQrCodeRepository,ISaveDBRepository saveRepository)
        {
            _activeBorrowsRepository = activeBorrowsRepository;
            _alllBorrowsRepository = alllBorrowsRepository;
            _userRepository = userRepository;
            _saveRepository = saveRepository;
            _cQrCodeRepository = cQrCodeRepository;
        }

        public bool IsValidQrCode(QrCodeDto qrCode)
        {
            return _cQrCodeRepository.isValidQrCode(qrCode.QRCode) != null;
        }
        
        public void InvalidateQrCode(QrCodeDto qrCode,DateTime now)
        {
            var bo = _cQrCodeRepository.getBorrow(qrCode.QRCode);
            if(bo == null)
            {
                throw new ArgumentException("No Borrow with that qrCode");
            } 
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
                ItemInstanceId = b.ItemInstance.ItemId,
                ItemName = b.ItemInstance.Item.Name,
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
                ItemInstanceId = b.ItemInstance.ItemId,
                ItemName = b.ItemInstance.Item.Name,
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
        public bool IsValidQrCode(QrCodeDto qrCode);
        public void InvalidateQrCode(QrCodeDto qrCode, DateTime now);
    }
        
    public interface IQrCodeUService
    {
        public IEnumerable<BorrowDto> OpenQrCodes();
        public IEnumerable<BorrowDto> AllQrCodes();
        public int OpenQrCodesCount();
    }
}
