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
    public class AutomatLoggingService : IAutomatLoggingService
    {
        private readonly IRepositoryRead _repositoryRead;
        private readonly IRepositoryWrite _repositoryWrite;
        public AutomatLoggingService(IRepositoryRead repositoryRead, IRepositoryWrite repositoryWrite)
        {
            _repositoryRead = repositoryRead;
            _repositoryWrite = repositoryWrite;
        }

        public void LogScannedQrCode(string guidString)
        {
            _repositoryWrite.Add((ScannedQRCodeLog)LogQrCode(new ScannedQRCodeLog(), guidString));
        }

        public void LogInvaldScannedQrCode(string guidString)
        {
            _repositoryWrite.Add((InvalidQRCodeLog)LogQrCode(new InvalidQRCodeLog(), guidString));
        }

        public void EjectedItem(string guidString)
        {
            _repositoryWrite.Add((EjectedItemLog)LogQrCode(new EjectedItemLog(), guidString));
        }

        private BaseLogQrCode LogQrCode(BaseLogQrCode ob, string guidString)
        {
            ob.Scanned = guidString;
            ob.DateTime = DateTime.Now;


            Guid guid;
            if (Guid.TryParse(guidString, out guid))
            {
                var borrowOb = _repositoryRead.FindByGuid<Borrow>(guid);
                ob.Borrow = borrowOb;
            }
            return ob;
        }
    }


    public interface IAutomatLoggingService
    {
        void EjectedItem(string guidString);
        void LogInvaldScannedQrCode(string guidString);
        void LogScannedQrCode(string guidString);
    }
}
