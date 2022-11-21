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

        private BorrowCo _borrowCo;

        public BorrowService(IItemRepository itemRepository, IUserRepository userRepository, BorrowCo borrowCo)
        {
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _borrowCo = borrowCo;
        }

        public void Borrow(BorrowDataDto BData, string UserName, DateTime now)
        {
            if (!_itemRepository.isItemAvalable(BData.ItemId, now)) throw new InvalidOperationException("This Item is currently not avalable");
            if (now.AddDays(_borrowCo.MaxBorrowTime) < BData.DueTime) throw new ArgumentException("The DueTime is to long. The maximum is " + _borrowCo.MaxBorrowTime + " days.");
            var user = _userRepository.FindUser(UserName) ?? throw new ArgumentException("Der Username ist nicht bekannt.");
            var II = _itemRepository.getFreeItemInstance(BData.ItemId, now);
            II?.AddBorrow(new Borrow() { PredictedReturnDate = BData.DueTime, BorrowDate = DateTime.Now, User = user, ItemInstance = II, ReturnDate = null }, DateTime.Now);
        }
    }
    
    public interface IBorrowService   
    {
        public void Borrow(BorrowDataDto BData, string UserName, DateTime now);
    }
}
