using API.Services.ForS3.Configure;
using API.Services.ForS3;
using Microsoft.AspNetCore.Mvc;
using API.DAL.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using API.Services.ForAPI.Int;
using Amazon.S3.Model;
using MongoDB.Driver.Core.Misc;
using System.Linq;
using Newtonsoft.Json;
using System.Security.Claims;

namespace API.Controllers.Web
{

    public class PlayListsController : Controller
	{
		private readonly IAd_files_Service _ad_files;
		private readonly IAd_Playlist_Service _ad_playlist;
		private readonly IMedia_File_Service _media_file;
		private readonly IUser_Service _user;
		private readonly IMedia_Playlist_Service _media_playlist;
		

		public PlayListsController(  IMedia_File_Service media_file, IUser_Service user_Service, IMedia_Playlist_Service playlist_Service, IAd_Playlist_Service ad_servce, IAd_files_Service ad_files)
		{
			_ad_files = ad_files;
			_ad_playlist = ad_servce;
			_media_playlist = playlist_Service;
			_user = user_Service;
			_media_file = media_file;
		}
		[Route("Web/PlayLists/PlaylistsFace")]
		[HttpGet]
		[Authorize]
		public async Task<IActionResult>  PlaylistsFace()
		{

			var mediaPlaylist = await _media_playlist.GetPlayListUser(User.Identity.Name);
			var adPlaylist = await _ad_playlist.GetPlayListUser(User.Identity.Name);



			ViewBag.Role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
			ViewBag.Ad = adPlaylist;
			ViewBag.Media = mediaPlaylist;



			return View();
		}
		[Route("Web/PlayLists/PlaylistsFace/Update")]
		[Authorize]
		[HttpGet]
		public async Task<IActionResult> PlaylistsFaceUpdate()
		{
			var mediaPlaylist = await _media_playlist.GetPlayListUser(User.Identity.Name);
			var adPlaylist = await _ad_playlist.GetPlayListUser(User.Identity.Name);


            ViewBag.Role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            ViewBag.Ad = adPlaylist;
			ViewBag.Media = mediaPlaylist;
			return View();
		}
		[Route("Web/PlayLists/PlaylistsFace/DeleteAd")]
        [Authorize(Roles = "user")]
        [HttpDelete]
		public async Task<bool> DeleteAd(string id)
		{
			await _ad_playlist.DeletePlaylist(User.Identity.Name, id);
			return true;
		}
		[Route("Web/PlayLists/PlaylistsFace/DeleteMed")]
		[Authorize]
		[HttpDelete]
		public async Task<bool> DeleteMed(string id)
		{
			await _media_playlist.DeletePlaylist(User.Identity.Name, id);
			return true;
		}


		[Route("Web/PlayLists/PlaylistsFace/CreatePlaylist")]
		[HttpPost]
		[Authorize]
		public async Task<bool> CreatePlaylist(string name, bool type ,string role)
		{
			if(type == true)
			{
				var playList = new Media_Ad_playlist();
				playList.name = name;
				await _ad_playlist.AddPlaylist(User.Identity.Name, playList);
				return true;
				
			}
			else
			{

				
				await _media_playlist.AddPlaylist(User.Identity.Name,role,name);
				return true;
			}			
		}
		
		[Route("Web/PlayLists/Edit")]
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Edit([FromQuery(Name = "Id")] string fileid)
		{
			var role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            var playlist = await _media_playlist.GetMediaPlaylist(fileid);
			var publicFiles = await _media_file.GetPublicFiles();
			List<Media_file> playlist_file = await _media_file.GetFiles(playlist.data.media_files_id);
			List<Media_file> files = await _media_file.GetFiles(await _user.GetUserFilesId(User.Identity.Name));

			files = files.Except(playlist_file).ToList();
			publicFiles = publicFiles.Except(playlist_file).ToList();
			if(role == "admin")
			{
				publicFiles = publicFiles.Except(files).ToList();
			}



			ViewBag.Role = role;
            ViewBag.PublicFile = publicFiles;
			ViewBag.Disable_File = files ;
            ViewBag.Enabled_File = playlist_file;
			return View(playlist.data);
		}
		[Route("Web/PlayLists/EditAd")]
		[HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> EditAd([FromQuery(Name = "Id")] string fileid)
		{
			var userlist = await _user.GetUserAdFilesId(User.Identity.Name);
			var playlist = await _ad_playlist.GetPlaylistAsyncbyId(fileid);
			var listId = playlist.data.GetIdPlaylists().ToList();
			var playlist_file = await _ad_files.Getfiles(listId);
			var user_list = await _ad_files.Getfiles(userlist);

			user_list = user_list.Except(playlist_file).ToList();


            ViewBag.Role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            ViewBag.Disable_File = user_list;
			ViewBag.Enabled_File = playlist_file;
			return View(playlist.data);
		}

		[Route("Web/PlayLists/Edit/EditPL")]
		[HttpPost]
		[Authorize]
		public async Task<bool> EditPL( string[] new_list_ids , string id)
		{
			bool answer = await _media_playlist.Edit(id, new_list_ids);
			return (answer);
		}
        [Route("Web/PlayLists/Edit/EditAdPL")]
        [HttpPost]
        [Authorize(Roles = "user")]
        public async Task<bool> EditAdPL(string playlistJson)
        {
			var newPlaylist = JsonConvert.DeserializeObject<Media_Ad_playlist>(playlistJson);
            bool answer = await _ad_playlist.Edit(newPlaylist);
            return (answer);
        }
        [Route("Web/PlayLists/Edit/EditUpdate")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditUpdate([FromQuery(Name = "Id")] string id)
		{
            var role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            var playlist = await _media_playlist.GetMediaPlaylist(id);
            var publicFiles = await _media_file.GetPublicFiles();
            List<Media_file> playlist_file = await _media_file.GetFiles(playlist.data.media_files_id);
            List<Media_file> files = await _media_file.GetFiles(await _user.GetUserFilesId(User.Identity.Name));

            files = files.Except(playlist_file).ToList();
            publicFiles = publicFiles.Except(playlist_file).ToList();
            if (role == "admin")
            {
                publicFiles = publicFiles.Except(files).ToList();
            }
            

			publicFiles = publicFiles.Except(playlist_file).ToList();

            ViewBag.Role = role;
            ViewBag.PublicFile = publicFiles;
            ViewBag.Disable_File = files;
            ViewBag.Enabled_File = playlist_file;


            return PartialView(playlist.data);
        }
        [Route("Web/PlayLists/Edit/EditUpdateAd")]
        [HttpGet]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> EditUpdateAd([FromQuery(Name = "Id")] string id)
        {
            var userlist = await _user.GetUserAdFilesId(User.Identity.Name);
            var playlist = await _ad_playlist.GetPlaylistAsyncbyId(id);
            var listId = playlist.data.GetIdPlaylists().ToList();
            var playlist_file = await _ad_files.Getfiles(listId);
            var user_list = await _ad_files.Getfiles(userlist);

            user_list = user_list.Except(playlist_file).ToList();


            ViewBag.Role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            ViewBag.Disable_File = user_list;
            ViewBag.Enabled_File = playlist_file;
            return View(playlist.data);
        }
    }
}
