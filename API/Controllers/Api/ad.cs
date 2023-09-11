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

namespace API.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ad : Controller
    {
        private readonly IAppConfiguration _appConfiguration;
        private readonly IAws3Services _aws3Services;
        private readonly IAd_files_Service _Ad_files;
        private readonly IAd_Playlist_Service _playlist;
        private readonly IDevice_Service _device;

        public ad(IAppConfiguration appConfiguration, IDevice_Service device, IAd_Playlist_Service playlist, IAd_files_Service Ad_files)
        {
            _Ad_files = Ad_files;
            _device = device;
            _playlist = playlist;
            _appConfiguration = appConfiguration;
            _aws3Services = new Aws3Services(_appConfiguration.AwsAccessKey, _appConfiguration.AwsSecretAccessKey, _appConfiguration.BucketName, _appConfiguration.URL);
        }
        [HttpGet]
        [Route("get_files")]
        public async Task<IActionResult> get_files([FromQuery(Name = "id")] string fileid)
        {
            await Loger.BeginMethod(Request);
            Adfile file = await _Ad_files.getAdfile(fileid);

            var document = await _aws3Services.DownloadAdFileAsync(file);
            
            if (document == null)
            {
                await Loger.FinishMethod(Request, "Not Found...","None");
                return NotFound();
                
            }
            await Loger.FinishMethod(Request, "Ok",document.ContentLength.ToString());
            return File(document.ResponseStream, file.mime_type, file.name);

        }
        [HttpGet]
        [Route("get_playlist")]
        public async Task<BaseResponse<Media_Ad_playlist>> get_playlist([FromQuery(Name = "deviceId")] string deviceid)
        {
            await Loger.BeginMethod(Request);
            BaseResponse<Media_Ad_playlist> answer = new BaseResponse<Media_Ad_playlist>();
            var device = await _device.GetDevice(deviceid);
            if (device.data == null)
            {
                answer.error = "Crush";
                await Loger.FinishMethod(Request, answer.error,answer.ToJson());
                return answer;



            }
            else if (device.data.ad_playlist == null)
            {
                answer.error = "Playlist Not Found";
                await Loger.FinishMethod(Request, answer.error, answer.ToJson());
                return answer;
            }
            else
            {
                string playlist = device.data.ad_playlist;
                await Loger.FinishMethod(Request, "successful!", answer.ToJson());
                return await _playlist.GetPlaylistAsyncbyId(playlist);
            }

        }
    }
}
