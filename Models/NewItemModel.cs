using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class NewItemModel
    {
      
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public decimal itemCost { get; set; }
        public decimal ItemWholeSale { get; set; }
        public decimal ItemRetail { get; set; }
        public IFormFile file { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }
}
