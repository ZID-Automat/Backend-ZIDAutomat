using ZID.Automat.Domain.Models;
using ZID.Automat.Infrastructure;
using System.Linq;

namespace ZID.Automat.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly AutomatContext _automatContext;
        public UserRepository(AutomatContext automatContext)
        {
            _automatContext = automatContext;
        }

        public void AddUser(User user)
        {
            _automatContext.Users.Add(user);
            _automatContext.SaveChanges();
        }

        public User? FindUser(string UserName)
        {
            return _automatContext.Users.FirstOrDefault(u => u.Username == UserName);
        }

        public bool UserExists(string UserName)
        {
            return _automatContext.Users.Where(u => u.Username == UserName).SingleOrDefault() != null;
        }
    }
    
    public interface IUserRepository
    {
        public User? FindUser(string UserName);

        public void AddUser(User user);

        public bool UserExists(string UserName);
    }
}
