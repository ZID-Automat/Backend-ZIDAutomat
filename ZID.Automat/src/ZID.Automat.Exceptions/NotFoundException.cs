//using System.Web.Http;

namespace ZID.Automat.Exceptions
{
    public class NotFoundException : Exception 
    {
        public NotFoundException(string element) : base($"{element} not found")
        {
        }
    }
}
