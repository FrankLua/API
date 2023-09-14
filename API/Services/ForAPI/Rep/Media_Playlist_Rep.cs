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

namespace API.Services.ForAPI.Rep
{
    public class Media_Playlist_Rep : IMedia_Playlist_Service
    {
        private readonly IMongoCollection<Media_playlist> _media_playlist;
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<BsonDocument> _media_playlisst;
        IMemoryCache _cache;

        public Media_Playlist_Rep(IAPIDatabaseSettings settings, IMongoClient mongoClient, IMemoryCache memoryCache)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _media_playlist = database.GetCollection<Media_playlist>("Media Playlists");
            _user = database.GetCollection<User>("Users");
            _media_playlisst = database.GetCollection<BsonDocument>("Media Playlists");
            _cache = memoryCache;
        }

        public async Task<string> AddPlaylist(string login, Media_playlist newplaylist)
        {
            newplaylist._id = ObjectId.GenerateNewId();

            try
            {

                await _media_playlist.InsertOneAsync(newplaylist);
                await _user.UpdateOneAsync(Builders<User>.Filter.Eq("login", $"{login}"), Builders<User>.Update.Push("media-playlist", newplaylist._id.ToString()));
                return "OK";

            }

            catch (Exception ex)
            {
                return ex.Message;
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

                //await _media_playlisst.UpdateOneAsync(Builders<BsonDocument>.Filter.Eq("_id", $"{id}"), Builders<BsonDocument>.Update.Set("media_files_id", files));
                return true;
            }
            catch
            {
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
                Loger.ExaptionForNotFound(ex, method: "GetMediaPlaylist", mongo_db_id, par);
                answer.error = "NotFound";
                return answer;
            }
            catch (Exception ex)
            {
                Loger.Exaption(ex,"GetMediaPlaylist");
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
                Loger.Exaption(ex, "GetMediaPlaylist");
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
                Loger.ExaptionForNotFound(ex, method: "GetPlayListUser", login, par);                
                return null;
            }
            catch (Exception ex)
            {
                Loger.Exaption(ex, "GetPlayLIstUser");
                return null;
            }


        }
    }
}
