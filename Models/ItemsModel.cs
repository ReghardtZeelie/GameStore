

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
        public byte[] ImageFile { get; set; }
        public string fileType { get; set; }
        public string FileName { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

    }
}
