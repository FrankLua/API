using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;
using API.Services.ForS3.Int;
using SharpCompress.Common;
using System;
using System.Net;
using System.Net.Mime;

namespace API.Services.ForS3.Rep
{
    public class Aws3Services : IAws3Services
    {
        private AmazonS3Config config;
        private readonly string _bucketName;
        private readonly IAmazonS3 _awsS3Client;

        public Aws3Services(string awsAccessKeyId, string awsSecretAccessKey, string bucketName, string url)
        {

            var newRegion = RegionEndpoint.GetBySystemName("ru-1");
            AmazonS3Config config = new AmazonS3Config()
            {

                RegionEndpoint = newRegion,
                Timeout = TimeSpan.FromSeconds(600),
                ReadWriteTimeout = TimeSpan.FromSeconds(600),
                RetryMode = RequestRetryMode.Standard,
                MaxErrorRetry = 3,

            };
            config.ServiceURL = "https://s3.timeweb.com";
            _bucketName = bucketName;
            _awsS3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, clientConfig: config);
        }

        public async Task<bool> CheackFileAsync(string folder, string file)
        {
            try
            {

                ListObjectsRequest request = new ListObjectsRequest();
                request.BucketName = _bucketName; //Amazon Bucket Name
                request.Prefix = folder;
                ListObjectsResponse response = await _awsS3Client.ListObjectsAsync(request);
                foreach (S3Object item in response.S3Objects)
                {
                    if (item.Key.Split('/')[1] == file)
                    {
                        return false;
                    }
                }

                return true;

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
                DeleteObjectRequest request = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = $"{folder}/{file}"
                };



                await _awsS3Client.DeleteObjectAsync(request);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<GetObjectResponse> DownloadAdFileAsync(Adfile file)
        {
            MemoryStream ms = null;

            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = $"{file.folder}/{file.name}"
                };

                var response = await _awsS3Client.GetObjectAsync(getObjectRequest);

                /*
                using (var response = await _awsS3Client.GetObjectAsync(getObjectRequest))
                {
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        using (ms = new MemoryStream())
                        {
                            await response.ResponseStream.CopyToAsync(ms);
                        }
                    }
                }
                
                if (ms is null || ms.ToArray().Length < 1)
                    throw new FileNotFoundException(string.Format("The document '{0}' is not found", file));
                */
                return response;
            }
            catch (AmazonS3Exception ex)
            {

                Loger.ExaptionForNotFound(ex, "AWS-GetFIle", id: file._id.ToString(), add: null);
                return null;
            }
            catch (Exception ex)
            {
                Loger.Exaption(ex, "AWS3");
                return null;
            }
        }

        public async Task<GetObjectResponse> DownloadFileAsync(Media_file file)
        {
            MemoryStream ms = null;

            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = $"{file.folder}/{file.name}"
                };

                var response = await _awsS3Client.GetObjectAsync(getObjectRequest);

                /*
                using (var response = await _awsS3Client.GetObjectAsync(getObjectRequest))
                {
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        using (ms = new MemoryStream())
                        {
                            await response.ResponseStream.CopyToAsync(ms);
                        }
                    }
                }
                
                if (ms is null || ms.ToArray().Length < 1)
                    throw new FileNotFoundException(string.Format("The document '{0}' is not found", file));
                */
                return response;
            }
            catch (Exception ex)
            {
                Loger.Exaption(ex, "AWS3");
                return null;
            }
        }
        public async Task<bool> UploadFileAsync(IFormFile file, bool ad)
        {

            var transfere = new TransferUtility(_awsS3Client);
            int BufferSize = 50 * 1024 * 1024;
            string path = MimeType.GetFolderName(file.ContentType, ad);
            try
            {

                using (var newMemoryStream = new StreamContent(file.OpenReadStream(), bufferSize: BufferSize))
                {
                    var stream = await newMemoryStream.ReadAsStreamAsync();


                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = stream,
                        Key = $"{path}/{file.FileName}",
                        BucketName = _bucketName,
                        ContentType = file.ContentType,

                    };

                    WebRequest.DefaultWebProxy = null;
                    await transfere.UploadAsync(uploadRequest);




                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
