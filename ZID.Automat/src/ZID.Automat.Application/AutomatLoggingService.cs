using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;
using ZID.Automat.Domain.Models.Logging;
using ZID.Automat.Repository;

namespace ZID.Automat.Application
{
    public interface IAutomatLoggingService
    {
        void EjectedItem(string guidString, Borrow borrow);
        void LogInvaldScannedQrCode(string guidString, Borrow borrow);
        void LogScannedQrCode(string guidString, Borrow borrow);
    }

    public class AutomatLoggingService : IAutomatLoggingService
    {
        private readonly IRepositoryRead _repositoryRead;
        private readonly IRepositoryWrite _repositoryWrite;
        public AutomatLoggingService(IRepositoryRead repositoryRead, IRepositoryWrite repositoryWrite)
        {
            _repositoryRead = repositoryRead;
            _repositoryWrite = repositoryWrite;
        }

        public void LogScannedQrCode(string guidString, Borrow borrow)
        {
            _repositoryWrite.Add((ScannedQRCodeLog)LogQrCode(new ScannedQRCodeLog(), guidString, borrow));
        }

        public void LogInvaldScannedQrCode(string guidString, Borrow borrow)
        {
            _repositoryWrite.Add((InvalidQRCodeLog)LogQrCode(new InvalidQRCodeLog(), guidString, borrow));
        }

        public void EjectedItem(string guidString, Borrow borrow)
        {
            _repositoryWrite.Add((EjectedItemLog)LogQrCode(new EjectedItemLog(), guidString, borrow));
        }

        private BaseLogQrCode LogQrCode(BaseLogQrCode ob, string guidString, Borrow borrow)
        {
            ob.Scanned = guidString;
            ob.DateTime = DateTime.Now;
            ob.Borrow = borrow;
            return ob;
        }
    }


}
