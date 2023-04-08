using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Domain.Interfaces;

namespace ZID.Automat.Domain.Models
{
    public class Borrow : HasGuid
    {
        public int Id { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime PredictedReturnDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? CollectDate { get; set; }

        public Guid GUID { get; set; } =Guid.Empty;

        public int? ItemInstanceId { get; set; }
        public ItemInstance? ItemInstance { get; set; } = null!;

        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;


        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
    }
}
