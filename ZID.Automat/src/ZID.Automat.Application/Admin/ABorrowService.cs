using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;
using ZID.Automat.Dto.Models;
using ZID.Automat.Repository;

namespace ZID.Automat.Application
{
    public class ABorrowService : IABorrowService
    {
        private readonly IRepositoryRead _repositoryRead;

        public ABorrowService(IRepositoryRead repositoryRead)
        {
            _repositoryRead = repositoryRead;
        }

        
    }

    public interface IABorrowService
    {
        
    }
}
