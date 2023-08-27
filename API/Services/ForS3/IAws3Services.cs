namespace API.Services.ForS3
{
    public interface IAws3Services
    {
        Task<byte[]> DownloadFileAsync(string file);

        Task<bool> UploadFileAsync(IFormFile file);
       
    }
}
