using API.DAL.Entity.APIDatebaseSet;
using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;
using API.Services.ForAPI.Int;
using MongoDB.Driver;

namespace API.Services.ForAPI.Rep
{
	public class Adfile_Rep : IAd_files_Service
	{
		private readonly IMongoCollection<Adfile> _ad_file;
		private readonly IMongoCollection<User> _user;




		public Adfile_Rep(IAPIDatabaseSettings settings, IMongoClient mongoClient)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);

			_ad_file = database.GetCollection<Adfile>("Ad Files");

			_user = database.GetCollection<User>("Users");
		}
		public async Task<Adfile> getAdfile(string id)
		{

			Adfile file = null;
			try
			{
				file = await _ad_file.FindAsync(file => file._id.ToString() == id).Result.FirstAsync();

				file.folder = MimeType.GetFolderName(file.mime_type, true);

				return file;
			}
			catch (Exception ex)
			{
				Loger.Exaption(ex,"getAdfile");
				return file;
			}
		}
	}
}
