﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZID.Automat.Dto.Models
{
    public class BorrowDto
    {
        public int ItemId { get; set; }
        public int ItemInstanceId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? CollectDate { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public Guid GUID { get; set; } = Guid.Empty;
        public int status {get;set;}
    }
}
