using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using TimeWebNet;

namespace API.Controllers
{
    public class MediaController : Controller
    {
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
            ListBucketsResponse response = await s3Client.ListBucketsAsync();
            foreach (S3Bucket b in response.Buckets)
            {
                b.
                Console.WriteLine("{0}\t{1}", b.BucketName, b.CreationDate);
            }

            string filePath = Directory.GetCurrentDirectory() + "\\ExampleImg\\" + "fsdfsdfsd.jpg";

            //Convert to Byte Array
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            //Return the Byte Array
            return File(fileBytes, "image/jpeg", "fsdfsdfsd.jpg");
        }
    }
}
