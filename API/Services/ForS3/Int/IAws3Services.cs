using Amazon.S3.Model;
using API.DAL.Entity.Models;

namespace API.Services.ForS3.Int
{
    public interface IAws3Services
    {
        Task<GetObjectResponse> DownloadFileAsync(Media_file file);

        Task<GetObjectResponse> DownloadAdFileAsync(Adfile file);

        Task<bool> UploadFileAsync(IFormFile file);

        Task<bool> CheackFileAsync(string folder, string file);

        Task<bool> DeleteFileAsync(string folder, string file);



    }
}
