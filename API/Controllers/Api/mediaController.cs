

using Amazon;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;
using API.Services.ForAPI.Int;
using API.Services.ForS3.Configure;
using API.Services.ForS3.Int;
using API.Services.ForS3.Rep;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MongoDB.Bson;
using System.Net;
using TimewebNet.Exceptions;
using TimeWebNet;

namespace API.Controllers.Api
{
    [Route("api/[controller]")]
    public class MediaController : Controller
    {
        private readonly IAppConfiguration _appConfiguration;
        private readonly IAws3Services _aws3Services;
        private readonly IMedia_Playlist_Service _playlist;
        private readonly IMedia_File_Service _file;
        private readonly IDevice_Service _device;

        public MediaController(IAppConfiguration appConfiguration, IMedia_Playlist_Service playlist, IMedia_File_Service file, IDevice_Service device)
        {
            _device = device;
            _file = file;
            _playlist = playlist;
            _appConfiguration = appConfiguration;
            _aws3Services = new Aws3Services(_appConfiguration.AwsAccessKey, _appConfiguration.AwsSecretAccessKey, _appConfiguration.BucketName, _appConfiguration.URL);
        }
        [HttpPost]

        [Route("videoPost")]
        public async Task<BaseResponse<FileContentResult>> UploadDocumentToS3([FromForm] IFormFile file)
        {            
            try
            {

                if (file == null)
                {
                    BaseResponse<FileContentResult> badResponse = new BaseResponse<FileContentResult>();
                    badResponse.error = "BadQueryParam";                    
                    return badResponse;
                }

                //var result = _aws3Services.UploadFileAsync(file);
                BaseResponse<FileContentResult> response = new BaseResponse<FileContentResult>();

                return response;
            }
            catch (Exception ex)
            {
                BaseResponse<FileContentResult> badResponse = new BaseResponse<FileContentResult>();
                Loger.Exaption(ex, "Upload");
                badResponse.error = "Error";
                return badResponse;
            }
        }
        
        [HttpGet]
        [EnableRateLimiting("ForOther")]
        [Route("get_playlist")]
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

        [EnableRateLimiting("ForFile")]
        [HttpGet]
        [Route("file")]
        public async Task<IActionResult> Get_file([FromQuery(Name = "Id")] string fileid)
        {
            
            Media_file file = await _file.GetFile(fileid);
            if(file == null)
            {
                return NotFound();
            }
            var document = await _aws3Services.DownloadFileAsync(file);
            if (document == null)
            {

                return NotFound();
            }            
            return File(document.ResponseStream, file.mime_type, file.name);
        }
    }
}
