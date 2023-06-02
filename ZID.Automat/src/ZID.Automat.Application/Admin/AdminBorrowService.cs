using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models.Analytics.User;
using ZID.Automat.Repository;

namespace ZID.Automat.Application.Admin
{
    public interface IAdminBorrowService
    {
        IEnumerable<UserAdmiBorrowDto> AllBorrows();
        BorrowAdminDetailedDto BorrowDetailed(int id);
        void Entschuldigt(int id, bool state);
        IEnumerable<UserAdmiBorrowDto> FinishedBorrows();
        IEnumerable<UserAdmiBorrowDto> OpenBorrows();
        IEnumerable<UserAdmiBorrowDto> ToDealWithBorrows();
        DateTime? Zurueckgeben(int id, bool state);
    }

    public class AdminBorrowService : IAdminBorrowService
    {

        private readonly IRepositoryRead _repositoryRead;
        private readonly IMapper _mapper;
        private readonly IRepositoryWrite _repositoryWrite;

        public AdminBorrowService(IRepositoryRead repositoryRead, IMapper mapper, IRepositoryWrite repositoryWrite)
        {
            _repositoryRead = repositoryRead;
            _mapper = mapper;
            _repositoryWrite = repositoryWrite;
        }

        public BorrowAdminDetailedDto BorrowDetailed(int id)
        {
            var bor = _repositoryRead.FindById<Borrow>(id);
            var mapped = _mapper.Map<Borrow, BorrowAdminDetailedDto>(bor);
            return mapped;
        }

        public DateTime? Zurueckgeben(int id, bool state)
        {
            var bor = _repositoryRead.FindById<Borrow>(id) ?? throw new Exception("Error");
            bor.ReturnDate = state ? DateTime.Now : null;
            _repositoryWrite.Update(bor);
            return bor.ReturnDate;
        }

        public void Entschuldigt(int id, bool state)
        {
            var bor = _repositoryRead.FindById<Borrow>(id) ?? throw new Exception("Error");
            bor!.entschuldigt = state;
            _repositoryWrite.Update(bor);
        }

        public IEnumerable<UserAdmiBorrowDto> AllBorrows()
        {
            return _mapper.Map<IEnumerable<Borrow>, IEnumerable<UserAdmiBorrowDto>>(_repositoryRead.GetAll<Borrow>());
        }

        public IEnumerable<UserAdmiBorrowDto> ToDealWithBorrows()
        {
            return _mapper.Map<IEnumerable<Borrow>, IEnumerable<UserAdmiBorrowDto>>(_repositoryRead.GetAll<Borrow>()).Where(b => b.Stati == 0);
        }

        public IEnumerable<UserAdmiBorrowDto> OpenBorrows()
        {
            return _mapper.Map<IEnumerable<Borrow>, IEnumerable<UserAdmiBorrowDto>>(_repositoryRead.GetAll<Borrow>()).Where(b => b.Stati == 1);
        }

        public IEnumerable<UserAdmiBorrowDto> FinishedBorrows()
        {
            return _mapper.Map<IEnumerable<Borrow>, IEnumerable<UserAdmiBorrowDto>>(_repositoryRead.GetAll<Borrow>()).Where(b => b.Stati == 2);
        }


    }
}
