using Amazon.S3.Model;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;

namespace API.Services.ForS3.Int
{
    public interface IAws3Services
    {
		BaseResponse<string> GetLinkMed(Media_file file);

        BaseResponse<string> GetLinkAd(Adfile file);

        Task<GetObjectResponse> DownloadFileAsync(Media_file file);

        Task<GetObjectResponse> DownloadAdFileAsync(Adfile file);

        Task<bool> UploadFileAsync(IFormFile file, bool ad, string loadId);

        Task<bool> CheackFileAsync(string folder, string file);

        Task<bool> DeleteFileAsync(string folder, string file);



    }
}
