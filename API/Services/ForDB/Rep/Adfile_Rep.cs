using Amazon.Runtime.Internal.Util;
using API.DAL.Entity.APIDatebaseSet;
using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;
using API.Services.ForAPI.Int;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using ZstdSharp.Unsafe;

namespace API.Services.ForAPI.Rep
{
	public class Adfile_Rep : IAd_files_Service
	{
		private readonly IMongoCollection<Ad_playlist> _ad_playlist;
		private readonly IMongoCollection<Adfile> _ad_file;
		private readonly IMongoCollection<User> _user;
		IMemoryCache _cache;



		public Adfile_Rep(IAPIDatabaseSettings settings, IMongoClient mongoClient, IMemoryCache memoryCache)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);

            _ad_playlist = database.GetCollection<Ad_playlist>("Ad Playlist");

            _ad_file = database.GetCollection<Adfile>("Ad Files");

			_user = database.GetCollection<User>("Users");

			_cache = memoryCache;
		}

		public async Task<Adfile> AddFile(IFormFile file, string login)
		{
			var newfile = new Adfile();
			newfile.name = file.FileName;			
			newfile.mime_type = file.ContentType;
			newfile._id = ObjectId.GenerateNewId();

			try
			{				

				await _user.UpdateOneAsync(Builders<User>.Filter.Eq("login", $"{login}"), Builders<User>.Update.Push("ad-files", newfile._id.ToString()));
				await _ad_file.InsertOneAsync(newfile);
				_cache.Set(newfile._id.ToString(), newfile, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
				_cache.Remove(login);
				return newfile;
			}
			catch (Exception ex)
			{
				Loger.Exaption(ex, "Add-File");
				return null;
			}
		}

		public async Task<bool> DeleteFile(string id, string login)
		{
			try
			{
                var playlist = await _ad_playlist.FindAsync(playlist => playlist.ad_files.Contains(new ad_files { file = id }));
                FilterDefinition<Ad_playlist> filter = new ExpressionFilterDefinition<Ad_playlist>(playlist => playlist.ad_files.Contains(new ad_files { file = id }));
                var update = Builders<Ad_playlist>.Update.PullFilter(p => p.ad_files, p => p.file == id);
               await _ad_playlist.UpdateManyAsync(filter, update);
                await _ad_file.DeleteOneAsync(a => a._id.ToString() == id);
				await _user.UpdateOneAsync(Builders<User>.Filter.Eq("login", $"{login}"), Builders<User>.Update.Pull("ad-files", id));
				foreach(var item in playlist.ToList())
				{
					_cache.Remove(item._id.ToString());
				}
				_cache.Remove(id);
				_cache.Remove(login);
				return true;
			}

			catch (Exception ex)
			{
				Loger.Exaption(ex, "Add-File");
				return false;
			}
		}

		public async Task<Adfile> Getfile(string id)
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
				Loger.Exaption(ex, "getAdfile");
				return null;
			}
		}

		public async Task<List<Adfile>> Getfiles(List<string> ids)
		{
			var list = new List<Adfile>();
			try
			{
				foreach (var id in ids)
				{
					_cache.TryGetValue(id, out Adfile? ad_file);
					if (ad_file == null)
					{
						var brigde = await _ad_file.FindAsync(file => file._id.ToString() == id);
						ad_file = await brigde.FirstAsync();
						ad_file.folder = MimeType.GetFolderName(ad_file.mime_type, true);
						_cache.Set(id, ad_file, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
						list.Add(ad_file);

					}
					else
					{
						list.Add(ad_file);
					}
				}
				return list;

			}
			catch (Exception ex)
			{
				Loger.Exaption(ex, "getAdfile");
				return null;
			}
		}

	}
}
