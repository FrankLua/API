using API.Services.ForS3.Configure;
using Microsoft.AspNetCore.Mvc;
using API.Services.ForAPI.Int;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.Entity.SecrurityClass;
using API.DAL.Entity.ResponceModels;
using MongoDB.Bson;
using Newtonsoft.Json;
using MongoDB.Bson.IO;
using API.DAL.Entity.SupportClass;
using API.Services.ForS3.Int;
using API.Services.ForS3.Rep;
using Microsoft.AspNetCore.RateLimiting;
using API.Services.ForDB.Int;
using System.Text;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Unicode;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ad : ControllerBase
    {
        private readonly IAppConfiguration _appConfiguration;
        private readonly IAws3Services _aws3Services;
        private readonly IAd_files_Service _Ad_files;
        private readonly IAd_Playlist_Service _playlist;
        private readonly IDevice_Service _device;
        IMemoryCache _cache;
        public ad(IMemoryCache cache,IAppConfiguration appConfiguration, IDevice_Service device, IAd_Playlist_Service playlist, IAd_files_Service Ad_files)
        {
            _cache = cache;
            _Ad_files = Ad_files;
            _device = device;
            _playlist = playlist;
            _appConfiguration = appConfiguration;
            _aws3Services = new Aws3Services(_cache, _appConfiguration.AwsAccessKey, _appConfiguration.AwsSecretAccessKey, _appConfiguration.BucketName, _appConfiguration.URL);
        }
        [EnableRateLimiting("ForFile")]
        [HttpGet]        
        [Route("file")]
        public async Task<BaseResponse<LinkFile>> get_files([FromQuery(Name = "id")] string fileid)
        {
           
            var file = await _Ad_files.Getfile(fileid);

            var document = _aws3Services.GetLinkAd(file);
            
                if (document == null)
                {
                  var badResponse = new BaseResponse<LinkFile>() { error = "Not found" };
                  return badResponse;
                }
                var response = new BaseResponse<LinkFile>() { data = new LinkFile() {URL = document.data } };
                return response;
                       
            
            

        }
        [HttpGet]
        [EnableRateLimiting("ForOther")]        
        [Route("playlist")]
        public async Task<BaseResponse<Media_Ad_playlist_for_API>> get_playlist([FromQuery(Name = "Id")] string deviceid)
        {
            
            BaseResponse<Media_Ad_playlist_for_API> answer = new BaseResponse<Media_Ad_playlist_for_API>();
            var device = await _device.GetDevice(deviceid);
            if (device.data == null)
            {
                answer.error = "Crush";
                
                return answer;
            }
            else if (device.data.ad_playlist == null)
            {
                answer.error = "Playlist Not Found";
                
                return answer;
            }
            else
            {
                string playlist = device.data.ad_playlist;
                var adPlaylist = await _playlist.GetPlaylistAsyncbyId(playlist);

				answer.data = new Media_Ad_playlist_for_API(adPlaylist.data);
                return answer;
            }

        }
		
	}
}
