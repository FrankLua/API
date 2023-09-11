using API.Services.ForS3.Configure;
using API.Services.ForS3;
using Microsoft.AspNetCore.Mvc;
using API.DAL.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using API.Services.ForAPI.Int;
using Amazon.S3.Model;
using MongoDB.Driver.Core.Misc;
using System.Linq;

namespace API.Controllers.Web
{

    public class PlayListsController : Controller
	{
		
		
		private readonly IMedia_File_Service _media_file;
		private readonly IUser_Service _user;
		private readonly IMedia_Playlist_Service _media_playlist;

		public PlayListsController(  IMedia_File_Service media_file, IUser_Service user_Service, IMedia_Playlist_Service playlist_Service)
		{
			_media_playlist = playlist_Service;
			_user = user_Service;
			_media_file = media_file;
		}
		[Route("Web/PlayLists/Face")]
		public async Task<IActionResult>  Face()
		{
			var model = await _media_playlist.GetPlayListUser(User.Identity.Name);
			if (TempData["answer"] != null) ViewBag.Answer = TempData["answer"].ToString();

			return View(model);
		}
		[HttpPost]
		[Route("Web/PlayLists/Face")]
		public async Task<IActionResult> CreatePlaylist(Media_playlist playlist)
		{
			TempData["answer"] = await _media_playlist.AddPlaylist(User.Identity.Name, playlist);

			return Redirect("~/Web/PlayLists/Face");
		}
		[HttpGet]
		[Route("Web/PlayLists/Edit")]
		public async Task<IActionResult> Edit([FromQuery(Name = "Id")] string fileid)
		{
			var playlist = await _media_playlist.GetMediaPlaylist(fileid);
			
			List<Media_file> playlist_file = await _media_file.GetFiles(playlist.data.media_files_id);
			List<Media_file> files = await _media_file.GetFiles(await _user.GetUserFilesId(User.Identity.Name));

			files = files.Except(playlist_file).ToList();



            ViewBag.Disable_File = files ;
            ViewBag.Enabled_File = playlist_file;
			return View(playlist.data);
		}
		
		[Route("Web/PlayLists/Edit/EditPL")]
		[HttpPost]
		[Authorize]
		public async Task<IActionResult> EditPL(string[] new_list_ids , string id)
		{
			bool answer = await _media_playlist.Edit(id, new_list_ids);
			return View();
		}
	}
}
