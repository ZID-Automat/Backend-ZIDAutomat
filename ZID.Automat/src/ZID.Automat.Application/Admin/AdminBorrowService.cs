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
    }

    public class AdminBorrowService : IAdminBorrowService
    {

        private readonly IRepositoryRead _repositoryRead;
        private readonly IMapper _mapper;

        public AdminBorrowService(IRepositoryRead repositoryRead, IMapper mapper)
        {
            _repositoryRead = repositoryRead;
            _mapper = mapper;
        }

        public BorrowAdminDetailedDto BorrowDetailed(int id)
        {
            var bor = _repositoryRead.FindById<Borrow>(id);
            var mapped = _mapper.Map<Borrow, BorrowAdminDetailedDto>(bor);
            return mapped;
        }
    }
}
