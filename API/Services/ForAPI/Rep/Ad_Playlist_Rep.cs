using Amazon.Runtime.Internal.Util;
using API.DAL.Entity.APIDatebaseSet;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;
using API.Services.ForAPI.Int;
using DnsClient;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace API.Services.ForAPI.Rep
{
	public class Ad_Playlist_Rep : IAd_Playlist_Service
	{
        IMemoryCache _cache;
        private readonly IMongoCollection<Media_Ad_playlist> _ad_playlist;
		public Ad_Playlist_Rep(IAPIDatabaseSettings settings, IMongoClient mongoClient, IMemoryCache memoryCache)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);

			_ad_playlist = database.GetCollection<Media_Ad_playlist>("Ad Playlist");

			_cache = memoryCache;
			
		}

		public async Task<BaseResponse<Media_Ad_playlist>> GetPlaylistAsyncbyId(string idPlaylist)
		{
            
            BaseResponse<Media_Ad_playlist> answer = new BaseResponse<Media_Ad_playlist>();
			try
			{				
                _cache.TryGetValue(idPlaylist, out Media_Ad_playlist? playlist_from_cache);
				if(playlist_from_cache == null)
				{
                    var bridge = await _ad_playlist.FindAsync(playlist => playlist._id == idPlaylist);
                    var playlist = await bridge.FirstAsync();					
                    answer.data = playlist;
                    _cache.Set(idPlaylist, playlist,new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
				else
				{
                    answer.data = playlist_from_cache;
                }				
				return answer;

			}
            catch (InvalidOperationException ex)
            {
                string[] par = new string[] { "ad_playlis" };
                Loger.ExaptionForNotFound(ex, method: "GetPlaylistAsyncbyId", id: idPlaylist, par);
				answer.error = "NotFound";
                return null;
            }
            catch (Exception e) 
			{
				Loger.Exaption(e,"GetPlaylist");
				answer.error = "Crush";
				return answer;
			}
		}
	}
	
}
