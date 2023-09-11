using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;

namespace API.Services.ForAPI.Int
{
    public interface IAd_Playlist_Service
    {

		Task<BaseResponse<Media_Ad_playlist>> GetPlaylistAsyncbyId(string idPlaylist);

    }
}
