//using System.Web.Http;

namespace ZID.Automat.Extension
{
    public class QrCodeNotExistingException : Exception 
    {
        public QrCodeNotExistingException() : base("Kein Item im Automat verfügbar")
        {
        }
    }
}
