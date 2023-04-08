//using System.Web.Http;

namespace ZID.Automat.Extension
{
    public class NotFoundException : Exception 
    {
        public NotFoundException(string element) : base($"{element} not found")
        {
        }
    }
}
