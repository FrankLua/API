using Amazon.Runtime.Internal.Util;
using API.DAL.Entity.APIDatebaseSet;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;
using API.Services.ForAPI.Int;
using DnsClient;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Services.ForAPI.Rep
{
	public class Ad_Playlist_Rep : IAd_Playlist_Service
	{
		IMemoryCache _cache;
		private readonly IMongoCollection<Device> _device;
		private readonly IMongoCollection<Media_Ad_playlist> _ad_playlist;
		private readonly IMongoCollection<User> _user;
		public Ad_Playlist_Rep(IAPIDatabaseSettings settings, IMongoClient mongoClient, IMemoryCache memoryCache )
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);

			_ad_playlist = database.GetCollection<Media_Ad_playlist>("Ad Playlist");

			_user = database.GetCollection<User>("Users");

			_cache = memoryCache;
			_device = database.GetCollection<Device>("Device");
		}

		public async Task<BaseResponse<Media_Ad_playlist>> GetPlaylistAsyncbyId(string idPlaylist)
		{
            
            BaseResponse<Media_Ad_playlist> answer = new BaseResponse<Media_Ad_playlist>();
			try
			{				
                _cache.TryGetValue(idPlaylist, out Media_Ad_playlist? playlist_from_cache);
				if(playlist_from_cache == null)
				{
                    var bridge = await _ad_playlist.FindAsync(playlist => playlist._id.ToString() == idPlaylist);
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

		public async Task<BaseResponse<bool>> AddPlaylist(string login, Media_Ad_playlist newplaylist)
		{
			newplaylist._id = ObjectId.GenerateNewId().ToString();
			var responce = new BaseResponse<bool>();
			try 
			{
				await _ad_playlist.InsertOneAsync(newplaylist);
				await _user.UpdateOneAsync(Builders<User>.Filter.Eq("login", $"{login}"), Builders<User>.Update.Push("ad-playlist", newplaylist._id.ToString()));
				_cache.Set(newplaylist._id.ToString(), newplaylist, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
				_cache.Remove(login);
				responce.data = true;
				return responce;

			}
			catch(Exception ex)
			{
				Loger.Exaption(ex, "Add_AD_Playlist");
				responce.error = "Crush";
				responce.data = false;
				return responce;
			}
		}

		public async Task<List<Media_Ad_playlist>> GetPlayListUser(string login)
		{
			var responce = new List<Media_Ad_playlist>();
			try
			{
				_cache.TryGetValue(login, out User? user);
				if (user == null)
				{
					var brigde_for_user = await _user.FindAsync(user => user.login == login);
					user = await brigde_for_user.FirstAsync();
					_cache.Set(login, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
				}				
				foreach (string id_playlist in user.ad_playlists)
				{
					_cache.TryGetValue(id_playlist, out Media_Ad_playlist? playlist);
					if (playlist == null)
					{
						var brigde_for_id_playlist = await _ad_playlist.FindAsync(playlist => playlist._id.ToString() == id_playlist);
						playlist = await brigde_for_id_playlist.FirstAsync();
						_cache.Set(id_playlist, playlist, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
						responce.Add(playlist);
					}
					else
					{
						responce.Add(playlist);
					}
				}

				return responce;
			}
			catch (InvalidOperationException ex)
			{
				string[] par = new string[] { "Ad_playlist" };
				Loger.ExaptionForNotFound(ex, method: "GetPlayListUser", login, par);
				return null;
			}
			catch (Exception ex)
			{
				Loger.Exaption(ex, "GetPlayLIstUser/Ad");
				return null;
			}
		}

		public async Task<BaseResponse<bool>> DeletePlaylist(string login,string idPlaylist)
		{
			var responce = new BaseResponse<bool>();
			try
			{
				var devices = await _device.FindAsync(device => device.ad_playlist.Contains(idPlaylist));
				FilterDefinition<Device> u = new ExpressionFilterDefinition<Device>(device => device.ad_playlist.Contains(idPlaylist));
				await _device.UpdateManyAsync(u, Builders<Device>.Update.Set("ad_playlist", BsonNull.Value.AsNullableObjectId));

				
				
				await _ad_playlist.DeleteOneAsync(playlist => playlist._id.ToString() == idPlaylist);				
				await _user.UpdateOneAsync(Builders<User>.Filter.Eq("login", $"{login}"), Builders<User>.Update.Pull("ad-playlist", idPlaylist));

				foreach (var item in devices.ToList())
				{
					_cache.Remove(item._id.ToString());
				}
				_cache.Remove(idPlaylist);
				_cache.Remove(login);
				responce.data = true;
				return responce;
			}
			catch (InvalidOperationException ex)
			{
				string[] par = new string[] { "Ad_playlist" };
				Loger.ExaptionForNotFound(ex, method: "DeletePlaylist/Ad", login, par);
				responce.data = false;
				return responce;
			}
			catch (Exception ex)
			{
				Loger.Exaption(ex, "DeletePlaylist/Ad");
				responce.data = false;
				return responce;
			}
		}

        public async Task<bool> Edit(Media_Ad_playlist newPlayList)
        {
            try
            {

                FilterDefinition<Media_Ad_playlist> u = new ExpressionFilterDefinition<Media_Ad_playlist>(playlist => playlist._id == newPlayList._id);

                await _ad_playlist.ReplaceOneAsync(u, newPlayList);         

                

                _cache.Remove(newPlayList._id);
                _cache.Set(newPlayList._id, newPlayList, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

                //await _media_playlisst.UpdateOneAsync(Builders<BsonDocument>.Filter.Eq("_id", $"{id}"), Builders<BsonDocument>.Update.Set("media_files_id", files));
                return true;
            }
            catch (InvalidOperationException ex)
            {
                string[] par = new string[] { "Ad_playlist" };
                Loger.ExaptionForNotFound(ex, method: "EditPlaylist/Ad", newPlayList._id, par);
                
                return false;
            }
            catch (Exception ex)
            {
                Loger.Exaption(ex, "EditPlaylist/Ad");
                
                return false;
            }
        }
    }
	
}
