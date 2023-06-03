using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZID.Automat.Dto.Models.Analytics.User;

namespace ZID.Automat.Dto.Models.Items
{
    public class ItemAdminDetailedDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SubName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategorieId { get; set; }


        public IEnumerable<UserAdmiBorrowDto> Borrows{get;set;}
    }

    public class ItemAdminUpdateAdd
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SubName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int CategorieId { get; set; }
    }
}
