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

        public UserAdminDetailedDto GetDetailedUser(int id)
        {
            var User = _repositoryRead.FindById<User>(id) ?? throw new Exception("User not found");
            var mappi = _mapper.Map<User, UserAdminDetailedDto>(User);

            var bors = User.Borrow.Select((b) =>
            {
                var bo = _mapper.Map<Borrow, UserAdmiBorrowDto>(b);
                if((b.PredictedReturnDate < DateTime.Now && b.ReturnDate != null)|| b.ReturnDate >b.PredictedReturnDate)
                {
                    bo.Stati = 0;
                }else if (b.ReturnDate == null)
                {
                    bo.Stati = 1;
                }
                else
                {
                    bo.Stati = 2;
                }
                return bo ?? null!;
            });
            mappi.Borrow = new List<UserAdmiBorrowDto>(bors);
            

            return mappi;
        }
    }
}
