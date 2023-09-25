using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;

namespace API.Services.ForAPI.Int
{
    public interface IAd_Playlist_Service
    {

		Task<BaseResponse<Media_Ad_playlist>> GetPlaylistAsyncbyId(string idPlaylist);

		Task<BaseResponse<bool>> DeletePlaylist(string login ,string idPlaylist);
		Task<BaseResponse<bool>> AddPlaylist(string login, Media_Ad_playlist newplaylist);

        Task<bool> Edit(string id, string[] new_file);

        Task<List<Media_Ad_playlist>> GetPlayListUser(string login);


	}
}
