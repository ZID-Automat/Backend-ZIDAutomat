using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Interfaces;
using ZID.Automat.Infrastructure;

namespace ZID.Automat.Repository
{
    public class GenericRepository: IRepositoryRead,IRepositoryWrite
    {
        private readonly AutomatContext _context;
        public GenericRepository(AutomatContext Db)
        {
            _context = Db;
        }

        public T? FindById<T>(int id) where T : class
        {
            return _context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return _context.Set<T>().ToList();
        }

        public T? FindByGuid<T>(Guid guid) where T : class, HasGuid
        {
            return _context.Set<T>().SingleOrDefault(t => t.GUID == guid);
        }

        public IEnumerable<T> FindAllByGuid<T>(Guid guid) where T : class, HasGuid
        {
            return _context.Set<T>().Where(t => t.GUID == guid).ToList();
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public T? FindByName<T>(string name) where T : class, HasName
        {
            return _context.Set<T>().SingleOrDefault(t => t.Name == name);
        }

        public IEnumerable<T> FindAllByName<T>(string name) where T : class, HasName
        {
            return _context.Set<T>().Where(t => t.Name == name).ToList();
        }

        public void Update<T>(T ent) where T : class
        {
            _context.Set<T>().Update(ent);
            _context.SaveChanges();
        }
    }

    public interface IRepositoryRead
    {
        T? FindById<T>(int id) where T : class;
        IEnumerable<T> GetAll<T>() where T : class;
        T? FindByGuid<T>(Guid guid) where T : class, HasGuid;
        IEnumerable<T> FindAllByGuid<T>(Guid guid) where T : class, HasGuid;
        T? FindByName<T>(string name) where T : class, HasName;
        IEnumerable<T> FindAllByName<T>(string name) where T : class, HasName;
    }

    public interface IRepositoryWrite
    {
        void Add<T>(T entity) where T : class;
        void Update<T>(T ent) where T : class;
    }
}
