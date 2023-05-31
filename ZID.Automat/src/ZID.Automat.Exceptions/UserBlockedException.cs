//using System.Web.Http;

namespace ZID.Automat.Exceptions
{
    public class UserBlockedException : Exception 
    {
        public UserBlockedException() : base("Der User ist blockiert")
        {
        }
    }
}
