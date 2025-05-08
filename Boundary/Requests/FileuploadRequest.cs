using System.ComponentModel.DataAnnotations;

namespace ManyRoomStudio.Boundary.Requests
{
    public class FileuploadRequest
    {
        public List<IFormFile>? File { get; set; }

        [Required(ErrorMessage = "Target id is mandatory.")]
        public int TargetId { get; set; }
    }
}
