//using System.Web.Http;

namespace ZID.Automat.Exceptions
{
    public class ZuVielUnbehandelteBorros : Exception 
    {
        public ZuVielUnbehandelteBorros() : base("Artikel konnte nicht ausgeborgt werden. Sei haben aktuell zu viele offene Verleihe")
        {
        }
    }
}
