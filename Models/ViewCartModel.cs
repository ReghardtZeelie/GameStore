using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ViewCartModel
    {
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public decimal ItemRetail { get; set; }

        public int QTy { get; set; }
    }
}
