using API.DAL.Entity.Models;
using MongoDB.Driver;
using API.DAL.Entity.SupportClass;
using API.DAL.Entity.APIDatebaseSet;
using MongoDB.Bson;
using System;
using API.Services.ForS3;
using API.Services.ForS3.Configure;
using API.Services.ForAPI.Int;

namespace API.Services.ForAPI.Rep
{
    public class Media_File_Rep : IMedia_File_Service
    {
        private readonly IMongoCollection<Media_file> _media_file;
        private readonly IMongoCollection<User> _user;




        public Media_File_Rep(IAPIDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);

            _media_file = database.GetCollection<Media_file>("Media Files");

            _user = database.GetCollection<User>("Users");
        }

        public async Task<string> AddFile(IFormFile file, string login)
        {
            Media_file newfile = new Media_file();
            newfile.name = file.FileName;
            newfile.is_public = false;
            newfile.mime_type = file.ContentType;
            newfile._id = ObjectId.GenerateNewId();

            try
            {
                //if(await _aws3.CheackFileAsync($"{MimeType.GetFolderName(newfile.mime_type)}/{newfile.name}"))
                //{
                await _user.UpdateOneAsync(Builders<User>.Filter.Eq("login", $"{login}"), Builders<User>.Update.Push("media-files", newfile._id.ToString()));
                await _media_file.InsertOneAsync(newfile);
                //}




                return $"Ok";
            }
            catch (Exception ex)
            {
                Loger.Exaption(ex,"Add-File");
                return $"{ex.Message}";
            }


        }

        public async Task<string> DeleteFile(string id, string login)
        {
            try
            {
                await _media_file.DeleteOneAsync(a => a._id.ToString() == id);
                await _user.UpdateOneAsync(Builders<User>.Filter.Eq("login", $"{login}"), Builders<User>.Update.Pull("media-files", id));
                return null;
            }
            catch (Exception ex)
            {
                return $"Error: {ex}";
            }

        }

        public async Task<Media_file> GetFile(string mongo_db_id)
        {
            Media_file file = null;
            try
            {
                file = await _media_file.FindAsync(file => file._id.ToString() == mongo_db_id).Result.FirstAsync();

                file.folder = MimeType.GetFolderName(file.mime_type);

                return file;
            }
            catch (Exception ex)
            {
                Loger.Exaption(ex,"GetFile");
                return file;
            }
        }

        public async Task<List<Media_file>> GetFiles(List<string> ids)
        {
            List<Media_file> files = new List<Media_file>();
            try
            {
                foreach (string id in ids)
                {
                    files.Add(await _media_file.Find(file => file._id.ToString() == id).FirstAsync());
                }

                return files;
            }
            catch (Exception ex)
            {
                Loger.Exaption(ex,"Get-files");
                return null;
            }
        }
    }
}
