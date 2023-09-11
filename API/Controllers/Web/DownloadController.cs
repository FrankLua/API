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

namespace API.Controllers.Web
{
    public class DownloadController : Controller
	{
		private readonly IAppConfiguration _appConfiguration;
		private readonly IAws3Services _aws3Services;
		private readonly IMedia_File_Service _media_file;
		private readonly IUser_Service _user;

		public DownloadController(IDevice_Service device,IAppConfiguration appConfiguration,IMedia_File_Service media_file, IUser_Service user_Service)
		{
			_appConfiguration = appConfiguration;
			_aws3Services = new Aws3Services(_appConfiguration.AwsAccessKey, _appConfiguration.AwsSecretAccessKey, _appConfiguration.BucketName, _appConfiguration.URL);
			_user = user_Service;
			_media_file = media_file;
		}
		[Route("Web/Download/DownloadFace")]
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> DownloadFace()
		{
			List<string> list = await _user.GetUserFilesId(User.Identity.Name);
			if (list == null)
			{
				ViewBag.Exaption = "Произошла ошибка!";
				return View();
			}
			List<Media_file>media_list = await _media_file.GetFiles(list);
			if (media_list == null)
			{
				ViewBag.Exaption = "Произошла ошибка!";
				return View();
			}
			else
			{
				
				return View(media_list);
			}
			
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
		public async Task< IActionResult> DownloadFace(IFormFile file)
		{
			if (file != null)
			{
				if(MimeType.CheakMimetype(file.ContentType))
				{
					if(await _aws3Services.CheackFileAsync(MimeType.GetFolderName(file.ContentType),file.FileName)){
						await _aws3Services.UploadFileAsync(file);
						ViewBag.Answer = _media_file.AddFile(file, User.Identity.Name);
						return Redirect("~/Web/Download/DownloadFace");
					}
					else
					{
						
						List<string> list = await _user.GetUserFilesId(User.Identity.Name);
						List<Media_file> media_list = await _media_file.GetFiles(list);
						ViewBag.Answer = "Файл с таким именем уже существует в хранилище";
						return View(media_list);
					}

				}
				else
				{
					List<string> list = await _user.GetUserFilesId(User.Identity.Name);
					List<Media_file> media_list = await _media_file.GetFiles(list);
					ViewBag.Answer = "Not correctly file type!";
					return View(media_list);
					
					
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
