//using System.Web.Http;

using System.Net;

namespace ZID.Automat.Exceptions
{
    public class BorrowDueTimeInvalidException : Exception
    {
        public BorrowDueTimeInvalidException() : base("Die Borrow duetime ist nicht valide")
        {
        }

    }
}