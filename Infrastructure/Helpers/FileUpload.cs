namespace ManyRoomStudio.Infrastructure.Helpers
{
    public class FileUpload
    {
        private static readonly string CurrentDirectory = Directory.GetCurrentDirectory();
        public static async Task<string> ExecuteFilAsync(IFormFile file)
        {
            var retval = "";

            if (!Directory.Exists(CurrentDirectory + "\\wwwroot\\Fileupload"))
                Directory.CreateDirectory(CurrentDirectory + "\\wwwroot\\Fileupload");

            Guid myGuid = Guid.NewGuid();
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            string filePath = Path.Combine(CurrentDirectory + "\\wwwroot\\Fileupload", myGuid.ToString());
            using (var stream = new FileStream(filePath + fileExtension, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            retval = myGuid + fileExtension;
            return retval;
        }
    }
}
