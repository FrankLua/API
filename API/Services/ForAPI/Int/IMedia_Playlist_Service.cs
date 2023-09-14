using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;

namespace API.Services.ForAPI.Int
{
    public interface IMedia_Playlist_Service
    {
        Task<BaseResponse<Media_playlist>> GetMediaPlaylist(string mongo_db_id);

        BaseResponse<Media_playlist> GetMediaPlaylistSync(string mongo_db_id);


        Task<List<Media_playlist>> GetPlayListUser(string login);

        Task<bool> Edit(string id,string[]new_file);

        Task<string> AddPlaylist(string login, Media_playlist newplaylist);
    }
}
