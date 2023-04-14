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
            var ItemInstances = item.ItemInstances;
            var Count = ItemInstances.Count(I => I.borrow is null);

            if (Count == 0)
            {
                throw new NoItemAvailable();
            }

            var user = _repositoryRead.FindByName<User>(UserName)??throw new NoUserFoundException();
            var GUID = Guid.NewGuid();

            if (BData.DueTime < now || BData.DueTime > now.AddDays(_borrowCo.MaxBorrowTime))
            {
                throw new BorrowDueTimeInvalidException();
            }


            var borrow = new Borrow()
            {
                GUID = GUID,
                PredictedReturnDate = BData.DueTime,
                BorrowDate = now,
                User = user,
                ReturnDate = null,
                ItemInstance = ItemInstances.Where(I => I.borrow is null).First()
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
