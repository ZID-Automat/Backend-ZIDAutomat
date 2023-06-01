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
        BorrowAdminDetailedDto BorrowDetailed(int id);
        public void Entschuldigt(int id, bool state);
        public DateTime? Zurueckgeben(int id, bool state);
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
    }
}
