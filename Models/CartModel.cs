
namespace Models
{
    public class CartModel
    {
        public int CartId { get; set; }
        public int UserID { get; set; }
        public List<ItemsModel> cartItems { get; set; }
    }
}
