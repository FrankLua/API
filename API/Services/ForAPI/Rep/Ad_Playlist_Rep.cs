using API.DAL.Entity.APIDatebaseSet;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;
using API.Services.ForAPI.Int;
using MongoDB.Driver;

namespace API.Services.ForAPI.Rep
{
	public class Ad_Playlist_Rep : IAd_Playlist_Service
	{

	    private readonly IMongoCollection<Media_Ad_playlist> _ad_playlist;
		public Ad_Playlist_Rep(IAPIDatabaseSettings settings, IMongoClient mongoClient)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);

			_ad_playlist = database.GetCollection<Media_Ad_playlist>("Ad Playlist");

			
		}

		public async Task<BaseResponse<Media_Ad_playlist>> GetPlaylistAsyncbyId(string idPlaylist)
		{
			BaseResponse<Media_Ad_playlist> answer = new BaseResponse<Media_Ad_playlist>();
			try
			{				
				answer.data = await _ad_playlist.FindAsync(playlist => playlist._id == idPlaylist).Result.FirstAsync();
				Loger.WriterLogMethod("Ad_playlist", "Db-ok");
				return answer;

			}
			catch(Exception e) 
			{
				Loger.Exaption(e,"GetPlaylist");
				answer.error = "Crush";
				return answer;
			}
		}
	}
	
}
