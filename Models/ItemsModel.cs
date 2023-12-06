

namespace Models
{
    public class ItemsModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public decimal itemCost { get; set; }
        public decimal ItemWholeSale {get;set;}
        public decimal ItemRetail { get; set; }
    }
}
