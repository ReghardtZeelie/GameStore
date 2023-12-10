
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class CartModel
    {
        
        [System.Text.Json.Serialization.JsonIgnore]
        public int CartId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public int UserID { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public decimal CartTotal { get; set; }
        public List<cartItemsModel> cartItems { get; set; }
    }
}
