using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class cartItemsModel
    {
        public int ItemCode { get; set; }
        public string ItemName { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public decimal ItemRetail { get; set; }
        public string Qty { get; set; }

    }
}
