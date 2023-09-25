using API.DAL.Entity.Models;
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

namespace API.Controllers.Web
{
    public class DownloadController : Controller
	{

		private readonly IAppConfiguration _appConfiguration;
		private readonly IAws3Services _aws3Services;
		private readonly IMedia_File_Service _media_file;
		private readonly IAd_files_Service _ad_file;
		private readonly IUser_Service _user;

		public DownloadController(IDevice_Service device,IAppConfiguration appConfiguration,IMedia_File_Service media_file, IUser_Service user_Service, IAd_files_Service ad_file)
		{
			_appConfiguration = appConfiguration;
			_aws3Services = new Aws3Services(_appConfiguration.AwsAccessKey, _appConfiguration.AwsSecretAccessKey, _appConfiguration.BucketName, _appConfiguration.URL);
			_user = user_Service;
			_media_file = media_file;
			_ad_file = ad_file;
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
				

			ViewBag.Med = media_list;
			ViewBag.Ad = ad_list;
		    return View();
			
			
		}
		[Route("Web/Download/Delete")]
		[HttpPost]
		[Authorize]
		public async Task<BaseResponse<string>> DeleteFile(string id)
		{
			BaseResponse<string> response = new BaseResponse<string>();

			try
			{
				Media_file file = await _media_file.GetFile(id);
				response.data = await _media_file.DeleteFile(id, User.Identity.Name);
				bool answer = await _aws3Services.DeleteFileAsync(MimeType.GetFolderName(file.mime_type), file.name);
				response.data = "Request successful!";
				response.error = null;
				return response;
			}
			catch
			{
				response.error = "Exaption";
				return response;
			}





		}
		[Route("Web/Download/DownloadFace")]
		[HttpPost]
		[Authorize]
		public async Task< IActionResult> DownloadFace(IFormFile file, bool ad)
		{
			if (file != null)
			{
				if (file != null & CheakMimetype(file.ContentType)&& await _aws3Services.CheackFileAsync(GetFolderName(file.ContentType,ad), file.FileName))
				{

					if (ad)
					{
						var newfile = await _ad_file.AddFile(file, User.Identity.Name);
						newfile.folder = GetFolderName(file.ContentType, ad);
						await _aws3Services.DownloadAdFileAsync(newfile);
						return RedirectPermanent("~/Web/Download/DownloadFace");
					}
					else
					{
						var newfile = await _media_file.AddFile(file, User.Identity.Name);
						newfile.folder = GetFolderName(file.ContentType, ad);
						await _aws3Services.DownloadFileAsync(newfile);						
						
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
	}
}
