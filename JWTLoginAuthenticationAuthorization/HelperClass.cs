namespace GameStore
{
    public class HelperClass
    {
        static readonly string[] SizeTypes = { "KB", "MB" };
        private readonly IConfiguration _config;
        public HelperClass(IConfiguration config)
        {
            _config = config;
        }
        public Byte[] imagetoByteArray(IFormFile File, ref string log)
        {
            MemoryStream memoryStream = new MemoryStream();
            Byte[] ImageArray;

            try
            {
                File.CopyTo(memoryStream);
                ImageArray = memoryStream.ToArray();

                Int64 KB = int.Parse(decimal.Round(memoryStream.Length / 1024, 0, MidpointRounding.AwayFromZero).ToString()) + 1;

                if (KB > Convert.ToInt64(_config["ImageSettings:Size"]))
                {
                    log = "Image may not be larger than " + convertSize(Convert.ToInt64(_config["ImageSettings:Size"])) + ". " + " Please resize and try again!";
                    memoryStream.Close();
                    memoryStream.Dispose();
                    return null;
                }
                return ImageArray;

            }
            catch (Exception ex)
            {
                log = ex.Message;
            }
            return null;
        }

        public static string convertSize(Int64 bytes)
        {
            int counter = 0;
            decimal number = (decimal)bytes;
            while (Math.Round(number / 1024) >= 1)
            {
                number = number / 1024;
                counter++;
            }
            return string.Format("{0:n1} {1}", number, SizeTypes[counter]);
        }
    }
}
