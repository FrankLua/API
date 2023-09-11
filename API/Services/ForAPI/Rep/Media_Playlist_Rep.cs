using API.DAL.Entity.APIDatebaseSet;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;
using API.Services.ForAPI.Int;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace API.Services.ForAPI.Rep
{
    public class Media_Playlist_Rep : IMedia_Playlist_Service
    {
        private readonly IMongoCollection<Media_playlist> _media_playlist;
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<BsonDocument> _media_playlisst;
       

        public Media_Playlist_Rep(IAPIDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _media_playlist = database.GetCollection<Media_playlist>("Media Playlists");
            _user = database.GetCollection<User>("Users");
            _media_playlisst = database.GetCollection<BsonDocument>("Media Playlists");
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
                answer.data = await _media_playlist.FindAsync(playlist => playlist._id.ToString() == mongo_db_id).Result.FirstAsync();
                return answer;
            }
            catch (Exception ex)
            {
                Loger.Exaption(ex,"GetMediaPlaylist");
                answer.error = "Crush";
                return answer;
            }
        }

        public async Task<List<Media_playlist>> GetPlayListUser(string login)
        {
            try
            {
                User user = await _user.Find(user => user.login == login).FirstAsync();
                List<Media_playlist> list = await _media_playlist.FindAsync(playlist => user.media_playlists.Contains(playlist._id.ToString())).Result.ToListAsync();
                return list;

            }
            catch (Exception ex)
            {
                return null;
            }


        }
    }
}
