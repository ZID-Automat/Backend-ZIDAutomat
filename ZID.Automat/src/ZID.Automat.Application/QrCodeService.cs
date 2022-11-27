﻿using System;
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
        private readonly IAlllBorrowsRepository _alllBorrowsRepository;
        private readonly IUserRepository _userRepository;
        public QrCodeService(IUserRepository userRepository, IActiveBorrowsRepository activeBorrowsRepository, IAlllBorrowsRepository alllBorrowsRepository)
        {
            _activeBorrowsRepository = activeBorrowsRepository;
            _alllBorrowsRepository = alllBorrowsRepository;
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

    public interface IQrCodeAService
    {
        public bool IsValidQrCode();
    }
        
    public interface IQrCodeUService
    {
        public IEnumerable<BorrowDto> OpenQrCodes();
        public IEnumerable<BorrowDto> AllQrCodes();
        public int OpenQrCodesCount();
    }
}
