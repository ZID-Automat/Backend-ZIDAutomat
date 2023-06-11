//using System.Web.Http;

namespace ZID.Automat.Exceptions
{
    public class PasswordWrongException:Exception 
    {
        public PasswordWrongException() : base("The Entered Password or Username ist wrong")
        {
        }
    }
}
