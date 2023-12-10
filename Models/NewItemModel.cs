using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


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
