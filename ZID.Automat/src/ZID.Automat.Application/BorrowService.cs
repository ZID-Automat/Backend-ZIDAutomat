using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Configuration;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Repository;

namespace ZID.Automat.Application
{
    public class BorrowService : IBorrowService 
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISaveDBRepository _saveRepository;

        private readonly BorrowCo _borrowCo;

        public BorrowService(IItemRepository itemRepository, IUserRepository userRepository, BorrowCo borrowCo, ISaveDBRepository saveRepository)
        {
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _borrowCo = borrowCo;
            _saveRepository = saveRepository;
        }

        /// <summary>
        /// Erstellt Borrow Eintrag
        /// </summary>
        /// <param name="BData"></param>
        /// <param name="UserName"></param>
        /// <param name="now"></param>
        /// <returns>Returnied die UUID des Borrows(QRCode)</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public string Borrow(BorrowDataDto BData, string UserName, DateTime now)
        {
            if (!_itemRepository.isItemAvalable(BData.ItemId, now)) throw new InvalidOperationException("This Item is currently not avalable");
            if (now.AddDays(_borrowCo.MaxBorrowTime) < BData.DueTime) throw new ArgumentException("The DueTime is to long. The maximum is " + _borrowCo.MaxBorrowTime + " days.");
            var user = _userRepository.FindUser(UserName) ?? throw new ArgumentException("Der Username ist nicht bekannt.");
            var II = _itemRepository.getFreeItemInstance(BData.ItemId, now);

            string UUID = Guid.NewGuid().ToString();
            
            II?.AddBorrow(new Borrow() {UUID=UUID, PredictedReturnDate = BData.DueTime, BorrowDate = DateTime.Now, User = user, ItemInstance = II, ReturnDate = null }, DateTime.Now);
            _saveRepository.SaveDb();
            return UUID;
        }
    }
    
    public interface IBorrowService   
    {
        public string Borrow(BorrowDataDto BData, string UserName, DateTime now);
    }
}
