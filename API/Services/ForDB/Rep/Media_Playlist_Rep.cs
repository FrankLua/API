using API.DAL.Entity.APIDatebaseSet;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;
using API.Services.ForAPI.Int;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json.Nodes;
using System.Text.Json;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Redis;
using System.Xml.Linq;

namespace API.Services.ForAPI.Rep
{
    public class Media_Playlist_Rep : IMedia_Playlist_Service
    {
        private readonly IMongoCollection<Media_playlist> _media_playlist;
        private readonly IMongoCollection<User> _user;
		private readonly IMongoCollection<Device> _device;
		IMemoryCache _cache;

        public Media_Playlist_Rep(IAPIDatabaseSettings settings, IMongoClient mongoClient, IMemoryCache memoryCache)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _media_playlist = database.GetCollection<Media_playlist>("Media Playlists");
            _user = database.GetCollection<User>("Users");
            _device = database.GetCollection<Device>("Device");
			_cache = memoryCache;
        }

        public async Task<bool> AddPlaylist(string login, string role, string name)
        {
			var newplaylist = new Media_playlist();
			newplaylist.name = name;
			switch (role)
			{
				case "admin": newplaylist.is_public = true; break;
				case "user": newplaylist.is_public = false; break;
			}
			newplaylist._id = ObjectId.GenerateNewId();

            try
            {

                await _media_playlist.InsertOneAsync(newplaylist);
                await _user.UpdateOneAsync(Builders<User>.Filter.Eq("login", $"{login}"), Builders<User>.Update.Push("media-playlist", newplaylist._id.ToString()));
				_cache.Set(newplaylist._id.ToString(), newplaylist, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
				_cache.Remove(login);
				return true;

            }

            catch (Exception ex)
            {
                Loger.Exception(ex, "AddPlaylist");
                return false;
            }
        }

		public async Task<BaseResponse<bool>> DeletePlaylist(string login, string idPlaylist)
		{
			var responce = new BaseResponse<bool>();
			try
			{
				var devices = await _device.FindAsync(device => device.media_play_list.Contains(idPlaylist));
				FilterDefinition<Device> u = new ExpressionFilterDefinition<Device>(device => device.media_play_list.Contains(idPlaylist));
				await _device.UpdateManyAsync(u, Builders<Device>.Update.Set("media_playlist",BsonNull.Value));


				await _media_playlist.DeleteOneAsync(playlist => playlist._id.ToString() == idPlaylist);
				await _user.UpdateOneAsync(Builders<User>.Filter.Eq("login", $"{login}"), Builders<User>.Update.Pull("media-playlist", idPlaylist));
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
				Loger.ExceptionForNotFound(ex, method: "DeletePlaylist/Ad", login, par);
				responce.data = false;
				return responce;
			}
			catch (Exception ex)
			{
				Loger.Exception(ex, "DeletePlaylist/Ad");
				responce.data = false;
				return responce;
			}
		}

		public async Task<bool> Edit(string id, string[] new_file)
        {
            try
            {
                List<string>files = new_file.ToList();

                var playlist = await _media_playlist.FindAsync(playlist => playlist._id.ToString() == id).Result.FirstAsync();

                await  _media_playlist.DeleteOneAsync(playlist => playlist._id.ToString() == id);

                playlist.media_files_id = files;

                await _media_playlist.InsertOneAsync(playlist);

                _cache.Remove(id);
                _cache.Set(playlist._id.ToString(), playlist, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

                //await _media_playlisst.UpdateOneAsync(Builders<BsonDocument>.Filter.Eq("_id", $"{id}"), Builders<BsonDocument>.Update.Set("media_files_id", files));
                return true;
            }
            catch (InvalidOperationException ex)
            {
                string[] par = new string[] { "Med_playlist" };
                Loger.ExceptionForNotFound(ex, method: "EditPlaylist/Med", id, par);

                return false;
            }
            catch (Exception ex)
            {
                Loger.Exception(ex, "EditPlaylist/Med");

                return false;
            }
        }

        public async Task<BaseResponse<Media_playlist>> GetMediaPlaylist(string mongo_db_id)
        {
            BaseResponse<Media_playlist> answer = new BaseResponse<Media_playlist>();
            try
            {
                _cache.TryGetValue(mongo_db_id, out Media_playlist? playlist);
                if(playlist == null)
                {
                    var bright = await _media_playlist.FindAsync(playlist => playlist._id.ToString() == mongo_db_id);

                    playlist = await bright.FirstAsync();

                    answer.data = playlist;

                    _cache.Set(mongo_db_id, playlist, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));

                    return answer;
                }
                else
                {
                    answer.data = playlist;
                    return answer;
                }
                
            }
            catch (InvalidOperationException ex)
            {
                string[] par = new string[] { "Media_playlist" };
                Loger.ExceptionForNotFound(ex, method: "GetMediaPlaylist", mongo_db_id, par);
                answer.error = "NotFound";
                return answer;
            }
            catch (Exception ex)
            {
                Loger.Exception(ex,"GetMediaPlaylist");
                answer.error = "Crush";
                return answer;
            }
           
        }

        public BaseResponse<Media_playlist> GetMediaPlaylistSync(string mongo_db_id)
        {
            BaseResponse<Media_playlist> answer = new BaseResponse<Media_playlist>();
            try
            {
                var bright = _media_playlist.Find(playlist => playlist._id.ToString() == mongo_db_id);
                answer.data = bright.First();
                return answer;
            }
            catch (Exception ex)
            {
                Loger.Exception(ex, "GetMediaPlaylist");
                answer.error = "Crush";
                return answer;
            }
        }

        public async Task<List<Media_playlist>> GetPlayListUser(string login)
        {
            try
            {
                _cache.TryGetValue(login, out User? user);
                if(user == null)
                {
                    var brigde_for_user = await _user.FindAsync(user => user.login == login);
                    user = await brigde_for_user.FirstAsync();
                    _cache.Set(login, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                }
                List< Media_playlist > list = new List< Media_playlist >();
                foreach ( string id_playlist in user.media_playlists)
                {
                    _cache.TryGetValue(id_playlist, out Media_playlist? playlist);
                    if(playlist == null)
                    {
                        var brigde_for_id_playlist = await _media_playlist.FindAsync(playlist => playlist._id.ToString() == id_playlist);
                        playlist = await brigde_for_id_playlist.FirstAsync();
                        _cache.Set(id_playlist, playlist, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
                        list.Add(playlist);
                    }
                    else
                    {
                        list.Add(playlist);
                    }
                }                
                
                return list;

            }
            catch (InvalidOperationException ex)
            {
                string[] par = new string[] { "Media_playlist" };
                Loger.ExceptionForNotFound(ex, method: "GetPlayListUser", login, par);                
                return null;
            }
            catch (Exception ex)
            {
                Loger.Exception(ex, "GetPlayLIstUser");
                return null;
            }


        }
		public async Task<List<Media_playlist>> GetPublicPlayList()
		{
			try
			{
                var list = _media_playlist
                    .FindAsync(p => p.is_public == true)
                    .Result
                    .ToList();
                    
				return list;

			}
			catch (InvalidOperationException ex)
			{
				string[] par = new string[] { "Media_playlist" };
				Loger.ExceptionForNotFound(ex, method: "PublicPlaylist", "---", par);
				return null;
			}
			catch (Exception ex)
			{
				Loger.Exception(ex, "GetPlayLIstUser");
				return null;
			}


		}
	}
}
