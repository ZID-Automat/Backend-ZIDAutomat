using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Repository;

namespace ZID.Automat.Application
{
    public class BorrowService:IBorrowService
    {
        public readonly IItemRepository _itemRepository;
        public readonly IUserRepository _userRepository;
        
        public BorrowService(IItemRepository itemRepository, IUserRepository userRepository)
        {
            _itemRepository = itemRepository;
            _userRepository = userRepository;
        }

        public void Borrow(BorrowDataDto BData,string UserName, DateTime now)
        { 
            if (_itemRepository.isItemAvalable(BData.ItemId,now))
            {
                var user = _userRepository.FindUser(UserName)?? throw new ArgumentException("Der Username ist nicht bekannt.");
                var II = _itemRepository.getFreeItemInstance(BData.ItemId, now);
                II?.AddBorrow(new Borrow() { PredictedReturnDate = BData.DueTime, BorrowDate = DateTime.Now, User = user,ItemInstance = II,ReturnDate = null},DateTime.Now);
            }
        }
    }
    
    public interface IBorrowService   
    {
        public void Borrow(BorrowDataDto BData, string UserName, DateTime now);
    }
}
