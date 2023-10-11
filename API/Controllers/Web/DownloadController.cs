﻿using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;
using API.Services.ForS3.Configure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using API.DAL.Entity.APIResponce;
using API.Services.ForAPI.Int;
using API.Services.ForS3.Int;
using API.Services.ForS3.Rep;
using static API.DAL.Entity.SupportClass.MimeType;
using System;
using API.Services.ForDB.Int;
using System.Security.Claims;
using StackExchange.Redis;
using Microsoft.AspNetCore.SignalR;
using static System.Net.WebRequestMethods;
using System.Text;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers.Web
{
    public class DownloadController : Controller
	{
		
		private readonly IAppConfiguration _appConfiguration;
		private readonly IAws3Services _aws3Services;
		private readonly IMedia_File_Service _media_file;
		private readonly IAd_files_Service _ad_file;
		private readonly IUser_Service _user;
		IMemoryCache _cache;
        
		private Dictionary<string, string> _loadings = new Dictionary<string, string>();
        
        public DownloadController( IMemoryCache cache, IDevice_Service device,IAppConfiguration appConfiguration,IMedia_File_Service media_file, IUser_Service user_Service, IAd_files_Service ad_file)
		{
            
            _appConfiguration = appConfiguration;
			
			_user = user_Service;
			_media_file = media_file;
			_ad_file = ad_file;
			_cache = cache;
			_aws3Services = new Aws3Services(_cache, _appConfiguration.AwsAccessKey, _appConfiguration.AwsSecretAccessKey, _appConfiguration.BucketName, _appConfiguration.URL);
		}
		[Route("Web/Download/DownloadFace")]
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> DownloadFace()
		{
			var playlistlistMed = await _user.GetUserFilesId(User.Identity.Name);

			var playlistlistAd = await _user.GetUserAdFilesId(User.Identity.Name);


			var media_list = await _media_file.GetFiles(playlistlistMed);
			
			var ad_list = await _ad_file.Getfiles(playlistlistAd);

            
            ViewBag.Role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            ViewBag.Med = media_list;
			ViewBag.Ad = ad_list;
		    return View();
			
			
		}
		[Route("Web/Download/DownloadFaceUp")]
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> DownloadFaceUp()
		{
			var playlistlistMed = await _user.GetUserFilesId(User.Identity.Name);

			var playlistlistAd = await _user.GetUserAdFilesId(User.Identity.Name);


			var media_list = await _media_file.GetFiles(playlistlistMed);

			var ad_list = await _ad_file.Getfiles(playlistlistAd);


			
			ViewBag.Med = media_list;
			ViewBag.Ad = ad_list;
			return View();


		}
		[Route("Web/Download/Delete")]
		[HttpPost]
		[Authorize]
		public async Task<BaseResponse<bool>> DeleteFile(string id, string type)
		{
			var response = new BaseResponse<bool>();

			try
			{
				if(type == "Med")
				{
					Media_file file = await _media_file.GetFile(id);
					await _media_file.DeleteFile(id, User.Identity.Name);
					bool answer = await _aws3Services.DeleteFileAsync(MimeType.GetFolderName(file.mime_type), file.name);
					response.data = true;
					response.error = null;
					return response;
				}
				else
				{
					Adfile file = await _ad_file.Getfile(id);
					response.data = await _ad_file.DeleteFile(id, User.Identity.Name);
					bool answer = await _aws3Services.DeleteFileAsync(MimeType.GetFolderName(file.mime_type,true), file.name);
					response.data = true;
					response.error = null;
					return response;
				}				
			}
			catch
			{
				response.error = "Exaption";
				return response;
			}





		}
		[Route("Web/Download/DownloadFace")]
		[HttpPost]
		[DisableRequestSizeLimit]
		[Authorize]
		public async Task< IActionResult> DownloadFace(bool ad, string role, string loadId,IFormFile file)
		{	
					
			
			ViewBag.Role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;			
            if (file != null)
			{
				if (CheakMimetype(file.ContentType)&& await _aws3Services.CheackFileAsync(GetFolderName(file.ContentType,ad), file.FileName))
				{

					if (ad)
					{
						var newfile = await _ad_file.AddFile(file, User.Identity.Name);
						newfile.folder = GetFolderName(file.ContentType, ad);
						
						await _aws3Services.UploadFileAsync(file, ad,loadId);
						return RedirectPermanent("~/Web/Download/DownloadFace");
					}
					else
					{
						var newfile = await _media_file.AddFile(file, User.Identity.Name,role);
						newfile.folder = GetFolderName(file.ContentType, ad);
						await _aws3Services.UploadFileAsync(file, ad, loadId);
						return RedirectPermanent("~/Web/Download/DownloadFace");
					}

				}
				else
				{
					return RedirectPermanent("~/Web/Download/DownloadFace");
				}

			}
			else 
			{
				List<string> list = await _user.GetUserFilesId(User.Identity.Name);
				List<Media_file> media_list = await _media_file.GetFiles(list);
				ViewBag.Answer = "File not pick";
				return View(media_list);
			}
			
		}
		[HttpGet]
		[DisableRequestSizeLimit]
		[Route("Web/Download/Upload")]
        public  async Task Upload([FromQuery(Name = "Id")] string id)
		{
			
			_cache.TryGetValue("Loader", out Dictionary<string, string>? _loadings);
			if(_loadings == null)
			{
				_loadings = new Dictionary<string, string>();
				_cache.Set("Loader", _loadings);
			}			
			if (_loadings.ContainsKey(id)){
				
				
				
					Response.Headers.Add("Content-Type", "text/event-stream");
					await Response
						.WriteAsync($"data: {_loadings[id]}\n\n");
				
				
			}
			else
			{
				_loadings.Add(id, "0/0");
				Response.Headers.Add("Content-Type", "text/event-stream");
				await Response
					.WriteAsync($"data: {_loadings[id]}\n\n");
			}


		}
		[HttpPost]
		[DisableRequestSizeLimit]
		[Route("Web/Download/DeleteIdLodear")]
		public async Task DeleteIdLodear(string id)
		{

			_cache.TryGetValue("Loader", out Dictionary<string, string>? _loadings);
			if (_loadings == null)
			{
				_loadings = new Dictionary<string, string>();
				_cache.Set("Loader", _loadings);
			}
			if (_loadings.ContainsKey(id))
			{
				_loadings.Remove(id);
			}	


		}


	}

}
