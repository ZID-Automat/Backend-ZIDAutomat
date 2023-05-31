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
    public interface IAdminUserService
    {
        IEnumerable<UserAdminGetAll> GetAllUsers();
    }

    public class AdminUserService : IAdminUserService
    {
        private readonly IRepositoryRead _repositoryRead;
        private readonly IMapper _mapper;

        public AdminUserService(IRepositoryRead repositoryRead, IMapper mapper)
        {
            _repositoryRead = repositoryRead;
            _mapper = mapper;
        }
        public IEnumerable<UserAdminGetAll> GetAllUsers()
        {
            var Users = _repositoryRead.GetAll<User>();
            return Users.Select((m =>
            {
                var mapi = _mapper.Map<User, UserAdminGetAll>(m);
                mapi.BorrowCount = m.Borrow.Count();
                return mapi;
            }));
        }
    }
}
