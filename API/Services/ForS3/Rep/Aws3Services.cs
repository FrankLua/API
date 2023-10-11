using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using API.Controllers.Web;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;

using API.Services.ForS3.Int;
using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using SharpCompress.Common;
using System;
using System.Net;
using System.Net.Mime;
using System.Text;

namespace API.Services.ForS3.Rep
{
    public class Aws3Services : IAws3Services
    {
        private readonly string _amazonAccessKeyId;
        private readonly string _amazonSecretAccessKey;
		private readonly string URL;
		private readonly string _bucketName;
        private readonly AmazonS3Config _config;
        IMemoryCache _cache;

        public Aws3Services(IMemoryCache cache , string awsAccessKeyId, string awsSecretAccessKey, string bucketName, string url)
        {
            _cache = cache;
            var newRegion = RegionEndpoint.GetBySystemName("ru-1");
            _amazonAccessKeyId = awsAccessKeyId;
            _amazonSecretAccessKey = awsSecretAccessKey;
            URL = "https://s3.timeweb.com";

			AmazonS3Config config = new AmazonS3Config()
            {
                
                RegionEndpoint = newRegion,
                Timeout = TimeSpan.FromSeconds(600),
                ReadWriteTimeout = TimeSpan.FromSeconds(600),
                RetryMode = RequestRetryMode.Standard,
                MaxErrorRetry = 3,
                ServiceURL = "https://s3.timeweb.com"
            };
            
            _bucketName = bucketName;
            _config = config;
        }

        public async Task<bool> CheackFileAsync(string folder, string file)
        {
            try
            {

                ListObjectsRequest request = new ListObjectsRequest();
                request.BucketName = _bucketName; //Amazon Bucket Name
                request.Prefix = folder;
                using (var _client = new AmazonS3Client(_amazonAccessKeyId, _amazonSecretAccessKey, _config))
                {
                    var response = await _client.ListObjectsAsync(request);
                    foreach (var item in response.S3Objects)
                    {
                        if (item.Key.Split('/')[1] == file)
                        {
                            return false;

                        }
                    }

                    return true;
                }
                
                

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteFileAsync(string folder, string file)
        {
            try
            {
                using (var _client = new AmazonS3Client(_amazonAccessKeyId, _amazonSecretAccessKey, _config))
                {
                    DeleteObjectRequest request = new DeleteObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = $"{folder}/{file}"
                    };



                    await _client.DeleteObjectAsync(request);
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        public async Task<GetObjectResponse> DownloadAdFileAsync(Adfile file)
        {
            try
            {
                using (var _client = new AmazonS3Client(_amazonAccessKeyId, _amazonSecretAccessKey, _config))
                {
                    GetObjectRequest getObjectRequest = new GetObjectRequest
                    {
                        BucketName = _bucketName,
                        Key = $"{file.folder}/{file.name}"
                    };
                    var response = await _client.GetObjectAsync(getObjectRequest);

                    return response;

                }
                
            }
            catch (AmazonS3Exception ex)
            {

                Loger.ExceptionForNotFound(ex, "AWS-GetFIle", id: file._id.ToString(), add: null);
                return null;
            }
            catch (Exception ex)
            {
                Loger.Exception(ex, "AWS3");
                return null;
            }
        }

        public async Task<GetObjectResponse> DownloadFileAsync(Media_file file)
        {
			try
			{
				using (var _client = new AmazonS3Client(_amazonAccessKeyId, _amazonSecretAccessKey, _config))
				{
					GetObjectRequest getObjectRequest = new GetObjectRequest
					{
						BucketName = _bucketName,
						Key = $"{file.folder}/{file.name}"
					};

					var response = await _client.GetObjectAsync(getObjectRequest);

					return response;
				}
			}
			catch (Exception ex)
			{
				Loger.Exception(ex, "AWS3");
				return null;
			}
		}
        public async Task<bool> UploadFileAsync(IFormFile file, bool ad, string loadId)
        {

            
            int BufferSize = 50 * 1024 * 1024;
            string path = MimeType.GetFolderName(file.ContentType, ad);
            try
            {

                using (var _client = new AmazonS3Client(_amazonAccessKeyId, _amazonSecretAccessKey, _config))
                {
                    using (var newMemoryStream = new StreamContent(file.OpenReadStream(), bufferSize: BufferSize).ReadAsStreamAsync())
                    {		

						var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = newMemoryStream.Result,
                            Key = $"{path}/{file.FileName}",
                            BucketName = _bucketName,
                            ContentType = file.ContentType,
							
						};                                       
                        
                        var transfere = new TransferUtility(_client);
                        uploadRequest.UploadProgressEvent += (sender, e) => UploadRequest_UploadProgressEvent(this,e,loadId);


						await transfere.UploadAsync(uploadRequest);





                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

		private void UploadRequest_UploadProgressEvent(object? sender, UploadProgressArgs? e,string id)
		{
            _cache.TryGetValue("Loader", out Dictionary<string, string> loger);
            loger[id] = $"{e.TransferredBytes}/{e.TotalBytes}";
            _cache.Set("Loader", loger);

		}		

		public BaseResponse<string> GetLinkMed(Media_file file)
		{
		   BaseResponse<string> response = new BaseResponse<string>();
            string data =$"{URL}/{_bucketName}/{file.folder}/{file.name}";
            response.data = data;
            return response;

		}

        public BaseResponse<string> GetLinkAd(Adfile file)
        {
            BaseResponse<string> response = new BaseResponse<string>();
            string data = $"{URL}/{_bucketName}/{file.folder}/{file.name}";
            response.data = data;
            return response;
        }
    }
}
