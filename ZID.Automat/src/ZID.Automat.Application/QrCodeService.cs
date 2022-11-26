using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Dto.Models;
using ZID.Automat.Repository;

namespace ZID.Automat.Application
{
    public class QrCodeService : IQrCodeAService, IQrCodeUService
    {
        private readonly IActiveBorrowsRepository _activeBorrowsRepository;
        private readonly IUserRepository _userRepository;
        public QrCodeService(IUserRepository userRepository, IActiveBorrowsRepository activeBorrowsRepository)
        {
            _activeBorrowsRepository = activeBorrowsRepository;
            _userRepository = userRepository;
        }

        public bool IsValidQrCode()
        {
            return false;
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
    }

    public interface IQrCodeAService
    {
        public bool IsValidQrCode();
    }
 
    public interface IQrCodeUService
    {
        public IEnumerable<BorrowDto> OpenQrCodes();
    }
}
