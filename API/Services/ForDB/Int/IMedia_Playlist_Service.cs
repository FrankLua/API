using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;

namespace API.Services.ForAPI.Int
{
    public interface IMedia_Playlist_Service
    {
        Task<BaseResponse<Media_playlist>> GetMediaPlaylist(string mongo_db_id);

        BaseResponse<Media_playlist> GetMediaPlaylistSync(string mongo_db_id);


        Task<List<Media_playlist>> GetPlayListUser(string login);
		Task<List<Media_playlist>> GetPublicPlayList();

		Task<bool> Edit(string id,string[]new_file);

		Task<BaseResponse<bool>> DeletePlaylist(string login, string idPlaylist);

		Task<bool> AddPlaylist(string login, string role, string name);


    }
}
