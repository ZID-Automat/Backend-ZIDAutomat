//using System.Web.Http;

namespace ZID.Automat.Extension
{
    public class PasswordWrongException:Exception 
    {
        public PasswordWrongException() : base("The Entered Password ist wrong")
        {
        }
    }
}
