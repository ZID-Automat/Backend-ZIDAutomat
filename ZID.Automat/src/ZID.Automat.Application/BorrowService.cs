using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Configuration;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Exceptions;
using ZID.Automat.Repository;

namespace ZID.Automat.Application
{
    public class BorrowService : IBorrowService
    {
        private readonly IRepositoryRead _repositoryRead;
        private readonly IRepositoryWrite _repositoryWrite;
        private readonly IMapper _mapper;

        private readonly BorrowCo _borrowCo;

        public BorrowService(BorrowCo borrowCo, IRepositoryRead repositoryRead, IRepositoryWrite repositoryWrite,IMapper mapper)
        {
            _repositoryRead = repositoryRead;
            _repositoryWrite = repositoryWrite;
            _borrowCo = borrowCo;
            _mapper = mapper;
        }

        public Guid Borrow(BorrowDataDto BData, string UserName, DateTime now)
        {
            var item = _repositoryRead.FindById<Item>(BData.ItemId) ?? throw new NotFoundException("Item");
            var ItemI = item.GetItemInstance();

            if (ItemI == null)
            {
                throw new NoItemAvailable();
            }

            var user = _repositoryRead.FindByName<User>(UserName)??throw new NotFoundException("User");

            if (user.Blockiert)
            {
                throw new UserBlockedException();
            }

            var GUID = Guid.NewGuid();

            if (BData.DueTime < now || BData.DueTime > now.AddDays(_borrowCo.MaxBorrowTime))
            {
                throw new BorrowDueTimeInvalidException();
            }

            if(user.Borrow.Count(b => b.Status() == 0) >= 2)
            {
                throw new ZuVielUnbehandelteBorros();
            }


            if (ItemI.borrow != null)
            {
                var borrowi = ItemI.borrow;
                ItemI.borrow = null;
                _repositoryWrite.Update(ItemI);
                _repositoryWrite.Delete(borrowi);
            }


            var borrow = new Borrow()
            {
                GUID = GUID,
                PredictedReturnDate = BData.DueTime,
                BorrowDate = now,
                User = user,
                ReturnDate = null,
                ItemInstance = ItemI
            };

            _repositoryWrite.Add(borrow);
            return GUID;
        }
    }
    
    public interface IBorrowService   
    {
        public Guid Borrow(BorrowDataDto BData, string UserName, DateTime now);
    }
}
