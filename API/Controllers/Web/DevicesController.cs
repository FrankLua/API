using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;
using API.Services.ForAPI.Int;
using API.Services.ForDB.Int;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using static MongoDB.Driver.WriteConcern;

namespace API.Controllers.Web
{
    public class DevicesController : Controller
    {
        private readonly IDevice_Service _device;

        private readonly IUser_Service _user;
		private readonly IMedia_Playlist_Service _media_playlist;
		private readonly IAd_Playlist_Service _ad_playlist;


		public DevicesController(IDevice_Service device, IUser_Service user_Service, IMedia_Playlist_Service playlists, IAd_Playlist_Service ad_Playlist)
        {
            _device = device;
            _user = user_Service;
            _media_playlist = playlists;
			_ad_playlist = ad_Playlist;

		}
		[HttpGet]
		[Route("Web/Devices/DevicesFace")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DevicesFace()
        {
            var login = User.Identity.Name;
			
            var user = await _user.GetUserInfo(login);
            var list = user.data.devices.ToList();
			List<DeviceResponce> model = new List<DeviceResponce>();
			foreach (var device in list)
			{
				model.Add(_device.GetDevice(device).Result.data);
			}
            ViewBag.Role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

            return View(model);
        }
		[Route("Web/Devices/DeviceFace/CreateDevice")]
        [Authorize(Roles = "user")]
        [HttpPost]
		public async Task<bool> CreateDevice(string name, string address)
		{		
			
			var login = User.Identity.Name;
			var device = new Device(name, address);

			var result = await _device.CreateDevice(device, login);

			return result.data;
		}
		[Route("Web/Devices/DeviceFace/UpdateDevice")]
        [Authorize(Roles = "user")]
        [HttpGet]
		public async Task<IActionResult> UpdateDeviceFace()
		{
			var login = User.Identity.Name;

			var user = await _user.GetUserInfo(login);
			var list = user.data.devices.ToList();
			List<DeviceResponce> model = new List<DeviceResponce>();
			foreach (var device in list)
			{
				model.Add(_device.GetDevice(device).Result.data);
			}
            ViewBag.Role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;

            return View(model);
		}
		[Route("Web/Devices/DeviceFace/Delete")]
		[Authorize]
		[HttpDelete]
		public async Task<bool> DeleteDeviceFace([FromQuery(Name ="Id")]string id)
		{
			var result = await _device.DeleteDevice(id,User.Identity.Name);
			if (result.data)
			{
				return result.data;
			}
			else { return false; }
			
		}
        [HttpGet]
        [Route("Web/Devices/DevicesEdit")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DevicesEdit([FromQuery(Name = "Id")] string id)
		{
			var model = await _device.GetDevice(id);
			var m_Playlists = await _media_playlist.GetPlayListUser(User.Identity.Name); // Get all playlist by user
			var publicPlaylist = await _media_playlist.GetPublicPlayList();
			if (model.data.media_playlist != null)
			{
				var actual_Playlist = await _media_playlist.GetMediaPlaylist(model.data.media_playlist); //Get playlist by device

				if (m_Playlists.Contains(actual_Playlist.data))
				{
					m_Playlists.Remove(actual_Playlist.data);
				}
				if (publicPlaylist.Exists(pub => pub._id == actual_Playlist.data._id))
				{
					var targetPlaylist =  publicPlaylist.Find(pub => pub._id == actual_Playlist.data._id);
					publicPlaylist.Remove(targetPlaylist);
				}
				m_Playlists.Insert(0, actual_Playlist.data);//Add in list playlists selected on device playlist
			}
			var a_Playlists = await _ad_playlist.GetPlayListUser(User.Identity.Name); // Get all playlist by user

			if (model.data.ad_playlist != null)
			{
				var actual_AdPlaylist = await _ad_playlist.GetPlaylistAsyncbyId(model.data.ad_playlist); //Get playlist by device

				if (a_Playlists.Contains(actual_AdPlaylist.data))
				{
					a_Playlists.Remove(actual_AdPlaylist.data);
				}
				a_Playlists.Insert(0, actual_AdPlaylist.data);//Add in list playlists selected on device playlist
			}

            ViewBag.Role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            ViewBag.PublicPlaylist = publicPlaylist;
			ViewBag.AdPlaylust = a_Playlists;
			ViewBag.Plalists = m_Playlists;
			return View(model.data);
		}
		[Route("Web/Devices/DevicesEdit/DeviceSave")]
		[HttpPut]
        [Authorize(Roles = "user")]
        public async Task<bool> DeviceSave( string deviceJson , string intervalsJson)
		{

			var device = JsonConvert.DeserializeObject<Device>(deviceJson);

			var intervalsArray = JsonConvert.DeserializeObject<string[]>(intervalsJson);


			device.intervals = TimeIntervals.ParceArray(intervalsArray);

			var answer = await _device.EditDevice(device);

			var model = await _device.GetDevice(device._id);


			return (true);
		}
		[Route("Web/Devices/DevicesEdit/DeviceUpdate")]
        [HttpGet]
        [Authorize(Roles = "user")]
		public async Task<IActionResult> DeviceUpdate([FromQuery(Name ="Id")] string mongoid)
		{	

			var model = await _device.GetDevice(mongoid);
			var m_Playlists = await _media_playlist.GetPlayListUser(User.Identity.Name); // Get all playlist by user
			var publicPlaylist = await _media_playlist.GetPublicPlayList();
			if (model.data.media_playlist != null)
			{
				var actual_Playlist = await _media_playlist.GetMediaPlaylist(model.data.media_playlist); //Get playlist by device

				if (m_Playlists.Contains(actual_Playlist.data))
				{
					m_Playlists.Remove(actual_Playlist.data);
				}
				if (publicPlaylist.Exists(pub => pub._id == actual_Playlist.data._id))
				{
					var targetPlaylist = publicPlaylist.Find(pub => pub._id == actual_Playlist.data._id);
					publicPlaylist.Remove(targetPlaylist);
				}
				m_Playlists.Insert(0, actual_Playlist.data);//Add in list playlists selected on device playlist
			}
			var a_Playlists = await _ad_playlist.GetPlayListUser(User.Identity.Name); // Get all playlist by user
			
			if (model.data.ad_playlist != null)
			{
				var actual_AdPlaylist = await _ad_playlist.GetPlaylistAsyncbyId(model.data.ad_playlist); //Get playlist by device

				if (a_Playlists.Contains(actual_AdPlaylist.data))
				{
					a_Playlists.Remove(actual_AdPlaylist.data);
				}
				a_Playlists.Insert(0, actual_AdPlaylist.data);//Add in list playlists selected on device playlist
			}

            ViewBag.Role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            ViewBag.PublicPlaylist = publicPlaylist;
			ViewBag.AdPlaylust = a_Playlists;
			ViewBag.Plalists = m_Playlists;
			return View(model.data);
		}
		
	}
}
