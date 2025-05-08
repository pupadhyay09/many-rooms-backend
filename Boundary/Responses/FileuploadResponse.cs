namespace ManyRoomStudio.Boundary.Responses
{
    public class FileuploadResponse
    {
        public int TotalFile { get; set; }
        public int TotalErrorFile { get; set; }
        public int TotalSuccessful { get; set; }
        public bool Iserror { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
