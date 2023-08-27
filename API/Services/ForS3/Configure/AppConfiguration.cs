namespace API.Services.ForS3.Configure
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration()
        {
            BucketName = "94f597f6-e30486cd-4123-4c3b-850b-9f89dfcfbdf1";
            AwsAccessKey = "ei40358";
            AwsSecretAccessKey = "148d4b26803f2acf37bf8afe25acfd88";
            URL = "https://s3.timeweb.com";
        }
        public string AwsAccessKey { get; set; }
        public string AwsSecretAccessKey { get; set; }
        public string URL { get; set; }
        public string BucketName { get; set; }
    }
}
