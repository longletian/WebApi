using Microsoft.AspNetCore.Http;

namespace WebApi.Models
{
    public class FileInfoData
    {
        public string FileName { get; set; }

        public string FileType { get; set; }

        public IFormFile FileInfo { get; set; }
        
    }
}