

using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using API.DAL.Entity.APIResponce;
using API.Services.ForS3;
using API.Services.ForS3.Configure;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TimewebNet.Exceptions;
using TimeWebNet;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class MediaController : Controller
    {
        private readonly IAppConfiguration _appConfiguration;
        private readonly IAws3Services _aws3Services;
        
        public MediaController(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
            _aws3Services = new Aws3Services(_appConfiguration.AwsAccessKey, _appConfiguration.AwsSecretAccessKey, _appConfiguration.BucketName, _appConfiguration.URL);
        }
        [HttpPost]
        
        [Route("videoPost")]
        public BaseResponse<FileContentResult> UploadDocumentToS3([FromForm]  IFormFile file)
        {
            try
            {
                
                if (file == null )
                {
                    BaseResponse<FileContentResult> badResponse = new BaseResponse<FileContentResult>();
                    badResponse.error = "BadQueryParam";
                    return badResponse;
                }               

                var result = _aws3Services.UploadFileAsync(file);
                BaseResponse<FileContentResult> response = new BaseResponse<FileContentResult>();
                
                return response;
            }
            catch (Exception ex)
            {
                BaseResponse<FileContentResult> badResponse = new BaseResponse<FileContentResult>();
                badResponse.error = "Error";
                return badResponse;
            }
        }
        [HttpGet]
        [Route("videoGet")]
        public async Task<IActionResult> GetDocumentFromS3([FromQuery(Name = "VideoName")] string videoName)
        {
            try
            {
                if (string.IsNullOrEmpty(videoName))
                {
                    Response.StatusCode = 404;
                    return Content("BadQueryParam");
                }                 

                

                var document = _aws3Services.DownloadFileAsync(videoName).Result;
                Response.StatusCode = 400;
                return File(document, "video/mp4", videoName);                
            }
            catch 
            {
                Response.StatusCode = 500;
                return Content("Error");
            }
        }
        [HttpGet]
        [Route("Get")]
        public async Task<FileResult> GetImg()
        {
            string accessKey = "ei40358";
            string secretKey = "148d4b26803f2acf37bf8afe25acfd88";

            AmazonS3Config config = new AmazonS3Config();
            config.ServiceURL = "https://s3.timeweb.com";

            AmazonS3Client s3Client = new AmazonS3Client(
                    accessKey,
                    secretKey,
                    config
                    );
            string filePath = Directory.GetCurrentDirectory() + "\\ExampleImg\\" + "fsdfsdfsd.jpg";
            ListBucketsResponse response = await s3Client.ListBucketsAsync();
            foreach (S3Bucket b in response.Buckets)
            {


                // 2. Put the object-set ContentType and add metadata.
                var putRequest2 = new PutObjectRequest
                {
                    BucketName = b.BucketName,
                    Key = "abc/xys/uvw/RussiaSolder",
                    FilePath = filePath,
                    ContentType = "image/jpeg"
                };
                PutObjectResponse response2 = await s3Client.PutObjectAsync(putRequest2);

                var directoryTransferUtility =
                    new TransferUtility(s3Client);

                // 1. Upload a directory.
               
                GetObjectRequest request = new GetObjectRequest()
                {
                    BucketName = b.BucketName,
                    Key = "abc/xys/uvw/RussiaSolder"

                };
                MemoryStream ms = null;
                using (var responsE = await s3Client.GetObjectAsync(request))
                {
                    if (responsE.HttpStatusCode == HttpStatusCode.OK)
                    {
                        ms = new MemoryStream();


                     await responsE.ResponseStream.CopyToAsync(ms);
                        
                    }
                }
                byte[] fileBytes2 = null;
                    

                return File(ms.GetBuffer(), "image/jpeg", "RussiaSolder.jpg");



                
            }


          

            //Convert to Byte Array
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            //Return the Byte Array
            return File(fileBytes, "image/jpeg", "fsdfsdfsd.jpg");
        }
    }
}
