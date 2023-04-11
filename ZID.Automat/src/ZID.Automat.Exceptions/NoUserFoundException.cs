//using System.Web.Http;

namespace ZID.Automat.Exceptions
{
    public class NoUserFoundException : Exception 
    {
        public NoUserFoundException() : base($"User not Found")
        {
        }
    }
}
