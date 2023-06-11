//using System.Web.Http;

namespace ZID.Automat.Exceptions
{
    public class UserBlockedException : Exception 
    {
        public UserBlockedException() : base("Sie sind leider blockiert vom Service")
        {
        }
    }
}
