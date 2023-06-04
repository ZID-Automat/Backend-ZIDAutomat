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
        public virtual ItemInstance? ItemInstance { get; set; } = null!;

        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;


        public bool entschuldigt { get; set; }
        

        public int Status()
        {
            if (entschuldigt)
            {
                return 2;
            }
            if (PredictedReturnDate < ReturnDate || PredictedReturnDate < DateTime.Now)
            {
                return 0;
            }
            else if (ReturnDate == null)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

        public int StatusEntschuldigt()
        {
            if (entschuldigt)
            {
                return 3;
            }
            if (PredictedReturnDate < ReturnDate || (PredictedReturnDate < DateTime.Now&&ReturnDate== null))
            {
                return 0;
            }
            else if (ReturnDate == null)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
    }
}
