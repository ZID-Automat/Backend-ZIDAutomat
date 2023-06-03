using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Dto.Models.Analytics.User
{
    public class BorrowAdminDetailedDto
    {
        public int Id { get; set; }


        public DateTime BorrowDate { get; set; }
        public DateTime PredictedReturnDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? CollectDate { get; set; }

        public Guid GUID { get; set; } = Guid.Empty;

        public int? ItemInstanceId { get; set; }
        public int? ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SubName { get; set; } = string.Empty;

        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;

        public bool entschuldigt { get; set; }

        public bool late { get; set; }
    }
}
