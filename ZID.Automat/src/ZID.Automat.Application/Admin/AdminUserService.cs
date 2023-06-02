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
        UserAdminDetailedDto GetDetailedUser(int id);
        public void SetBlockiert(int id, bool blockiert);

    }

    public class AdminUserService : IAdminUserService
    {
        private readonly IRepositoryRead _repositoryRead;
        private readonly IRepositoryWrite _repositoryWrite;

        private readonly IMapper _mapper;

        public AdminUserService(IRepositoryRead repositoryRead, IMapper mapper, IRepositoryWrite repositoryWrite)
        {
            _repositoryRead = repositoryRead;
            _mapper = mapper;
            _repositoryWrite = repositoryWrite;
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

        public void SetBlockiert(int id, bool blockiert)
        {
            var User = _repositoryRead.FindById<User>(id) ?? throw new Exception("User not found");
            User.Blockiert = blockiert;
            _repositoryWrite.Update(User);
        }

        public UserAdminDetailedDto GetDetailedUser(int id)
        {
            var User = _repositoryRead.FindById<User>(id) ?? throw new Exception("User not found");
            var mappi = _mapper.Map<User, UserAdminDetailedDto>(User);

            var bors = User.Borrow.Select((b) =>
            {
                var bo = _mapper.Map<Borrow, UserAdmiBorrowDto>(b);
               
                return bo ?? null!;
            });
            mappi.Borrow = new List<UserAdmiBorrowDto>(bors);
            

            return mappi;
        }
    }
}
