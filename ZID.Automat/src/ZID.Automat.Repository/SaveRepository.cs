using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Infrastructure;

namespace ZID.Automat.Repository
{
    public class SaveRepository:ISaveDBRepository
    {
        private readonly AutomatContext _automatContext;
        public SaveRepository(AutomatContext automatContext)
        {
            _automatContext = automatContext;
        }

        public void SaveDb()
        {
            _automatContext.SaveChanges();
        }
    }
    
    public interface ISaveDBRepository
    {
        public void SaveDb();
    }
}
