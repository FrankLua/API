using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;

namespace API.Services.ForAPI.Int
{
    public interface IMedia_File_Service
    {
        Task<Media_file>? GetFile(string id);


        Task<List<Media_file>> GetFiles(List<string> ids);


        Task<Media_file> AddFile(IFormFile file, string login);

        Task<string> DeleteFile(string id, string login);
    }
}
