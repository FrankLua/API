using Amazon.Runtime.Internal.Util;
using API.DAL.Entity.APIDatebaseSet;
using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;
using API.Services.ForAPI.Int;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace API.Services.ForAPI.Rep
{
	public class Adfile_Rep : IAd_files_Service
	{
		private readonly IMongoCollection<Adfile> _ad_file;
		private readonly IMongoCollection<User> _user;
        IMemoryCache _cache;



        public Adfile_Rep(IAPIDatabaseSettings settings, IMongoClient mongoClient, IMemoryCache memoryCache)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);

			_ad_file = database.GetCollection<Adfile>("Ad Files");

			_user = database.GetCollection<User>("Users");

            _cache = memoryCache;
        }
		public async Task<Adfile> getAdfile(string id)
		{
            
            try
			{
                _cache.TryGetValue(id, out Adfile? ad_file);
				if (ad_file == null)
				{
					var brigde = await _ad_file.FindAsync(file => file._id.ToString() == id);
					ad_file = await brigde.FirstAsync();
                    ad_file.folder = MimeType.GetFolderName(ad_file.mime_type, true);
					_cache.Set(id, ad_file, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
					return ad_file;
                }
				else
				{
                    return ad_file;
                }                
            }
			catch (Exception ex)
			{
				Loger.Exaption(ex,"getAdfile");
				return null;
			}
		}
	}
}
