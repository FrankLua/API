using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using API.DAL.Entity.Models;
using API.DAL.Entity.SupportClass;
using API.Services.ForS3.Int;
using System;
using System.Net;

namespace API.Services.ForS3.Rep
{
    public class Aws3Services : IAws3Services
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _awsS3Client;

        public Aws3Services(string awsAccessKeyId, string awsSecretAccessKey, string bucketName, string url)
        {
            AmazonS3Config config = new AmazonS3Config();
            config.ServiceURL = "https://s3.timeweb.com";
            _bucketName = bucketName;
            _awsS3Client = new AmazonS3Client(awsAccessKeyId, awsSecretAccessKey, config);
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
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public async Task<bool> UploadFileAsync(IFormFile file)
        {
            try
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new PutObjectRequest
                    {
                        InputStream = newMemoryStream,
                        Key = $"video/{file.FileName}",
                        BucketName = _bucketName,
                        ContentType = file.ContentType
                    };
                    PutObjectResponse response2 = await _awsS3Client.PutObjectAsync(uploadRequest);




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
