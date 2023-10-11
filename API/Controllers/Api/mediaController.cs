

using Amazon;
using Amazon.Runtime.Internal.Util;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;
using API.Services.ForAPI.Int;
using API.Services.ForDB.Int;
using API.Services.ForS3.Configure;
using API.Services.ForS3.Int;
using API.Services.ForS3.Rep;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using System.Net;
using System.Text.Unicode;
using TimewebNet.Exceptions;
using TimeWebNet;

namespace API.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : Controller
    {
        private readonly IDevice_Service _device;
        private readonly IMedia_File_Service _file;
        private readonly IAppConfiguration _appConfiguration;
        private readonly IAws3Services _aws3Services;        
        private readonly IMedia_Playlist_Service _playlist;
		IMemoryCache _cache;


		public MediaController(IMemoryCache cache,IAppConfiguration appConfiguration,  IMedia_Playlist_Service playlist, IMedia_File_Service file, IDevice_Service device)
        {
			_cache = cache;
			_device = device;
            _file = file;
            _playlist = playlist;          
            _appConfiguration = appConfiguration;
			_aws3Services = new Aws3Services(_cache, _appConfiguration.AwsAccessKey, _appConfiguration.AwsSecretAccessKey, _appConfiguration.BucketName, _appConfiguration.URL);
		}		
        
        [HttpGet]
        [NonAction]
        [Route("playlist")]
        public async Task<BaseResponse<Media_playlist_for_api>> Get_playlist([FromQuery(Name = "deviceId")] string deviceId)
        {
            
            var device =  await _device.GetDevice(deviceId);
            if(device.data == null)
            {
                BaseResponse<Media_playlist_for_api> badResponce = new BaseResponse<Media_playlist_for_api>();
                badResponce.error = "Crush";
                return badResponce;
            }
            var playlistforweb =  await _playlist.GetMediaPlaylist(device.data.media_playlist);
            if (playlistforweb.data == null)
            {
                BaseResponse<Media_playlist_for_api> badResponce = new BaseResponse<Media_playlist_for_api>();
                badResponce.error = "Crush";
                return badResponce;
            }
            BaseResponse<Media_playlist_for_api> answer = new BaseResponse<Media_playlist_for_api>();
            answer.data = new Media_playlist_for_api(playlistforweb.data);
            
            return answer;
        }
        [HttpGet]
        [EnableRateLimiting("ForFile")]        
        [Route("file")]
        public async Task<BaseResponse<LinkFile>> Get_file([FromQuery(Name = "Id")] string fileid)
        {
            
            Media_file file = await _file.GetFile(fileid);
            if(file == null)
            {
                return new BaseResponse<LinkFile> {error = "Not found" };
            }
            var document =  _aws3Services.GetLinkMed(file);
            if (document == null)
            {

				return new BaseResponse<LinkFile> { error = "Not found" };
			}
            var response = new BaseResponse<LinkFile> { data = new LinkFile { URL = document.data } };
			return response;
		}
    }
}
