//using System.Web.Http;

namespace ZID.Automat.Exceptions
{
    public class ZuVielUnbehandelteBorros : Exception 
    {
        public ZuVielUnbehandelteBorros() : base("Zu Viele offene Verleihe")
        {
        }
    }
}
