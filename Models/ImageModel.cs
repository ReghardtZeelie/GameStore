

namespace Models
{
    public class ImageModel
    {
 public int Id { get; set; }
        public byte[] ImageFile { get; set; }
        public string fileType { get; set; }
        public string FileName { get; set; }
    }
}
