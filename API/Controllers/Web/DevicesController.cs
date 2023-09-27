using API.DAL.Entity.Models;
using API.DAL.Entity.ResponceModels;
using API.Services.ForAPI.Int;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Reflection;
using System.Text;

namespace API.Controllers.Web
{
    public class DevicesController : Controller
    {
        private readonly IDevice_Service _device;
        private readonly IUser_Service _user;
		private readonly IMedia_Playlist_Service _media_playlist;


		public DevicesController(IDevice_Service device, IUser_Service user_Service, IMedia_Playlist_Service playlists)
        {
            _device = device;
            _user = user_Service;
            _media_playlist = playlists;
        }
		[Route("Web/Devices/DevicesFace")]
		[Authorize]
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
			
            return View(model);
        }
        [Route("Web/Devices/DevicesEdit")]
		[Authorize]
		public async Task<IActionResult> DevicesEdit([FromQuery(Name = "Id")] string id)
		{
            var model = await _device.GetDevice(id);
            var m_Playlists = await _media_playlist.GetPlayListUser(User.Identity.Name); // Get all playlist by user
            var actual_Playlist = await _media_playlist.GetMediaPlaylist(model.data.media_playlist); //Get playlist by device
              
            if(m_Playlists.Contains(actual_Playlist.data))
            {
                m_Playlists.Remove(actual_Playlist.data);
            }
			m_Playlists.Insert(0, actual_Playlist.data);//Add in list playlists selected on device playlist
			ViewBag.Plalists = m_Playlists;
			return View(model.data);
		}
		[Route("Web/Devices/DevicesEdit/DeviceSave")]
		[HttpPut]
		[Authorize]
		public async Task<bool> DeviceSave( Device device,  string[] times)
		{
            device.intervals = TimeIntervals.ParceArray(times);

            var answer = await _device.EditDevice(device);

			//var model = await _device.GetDevice(device._id);


			return (answer.data);
		}
		[Route("Web/Devices/DevicesEdit/DeviceUpdate")]
		
		[Authorize]
		public async Task<IActionResult> DeviceUpdate([FromQuery(Name ="Id")] string mongoid)
		{	

			var model = await _device.GetDevice(mongoid);
			var m_Playlists = await _media_playlist.GetPlayListUser(User.Identity.Name); // Get all playlist by user
			var actual_Playlist = await _media_playlist.GetMediaPlaylist(model.data.media_playlist); //Get playlist by device
			
			if (m_Playlists.Contains(actual_Playlist.data))
			{
				m_Playlists.Remove(actual_Playlist.data);
			}
			m_Playlists.Insert(0, actual_Playlist.data); //Add in list playlists selected on device playlist 
			ViewBag.Plalists = m_Playlists;

			return PartialView(model.data);
		}

	}
}
