//using System.Web.Http;

namespace ZID.Automat.Extension
{
    public class NoItemAvailable : Exception 
    {
        public NoItemAvailable() : base("Kein Item im Automat verfügbar")
        {
        }
    }
}
