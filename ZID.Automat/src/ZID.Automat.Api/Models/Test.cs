using System.Text.Json.Serialization;

namespace ZID.Automat.Api.Models
{
    public class Test
    {
        public string Test1 { get; set; }
        public int Hallo { get; set; }
        public Test2 TestOb { get; set; }

        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Male = 'm',
        Female ='s',
    }

    public class Test2
    {
        public string Test { get; set; }
    }
}
