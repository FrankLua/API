namespace API.Services.ForS3.Configure
{
    public class AppConfiguration : IAppConfiguration
    {
        
        public string AwsAccessKey { get; set; } = "ei40358";
        public string AwsSecretAccessKey { get; set; } = "148d4b26803f2acf37bf8afe25acfd88";
        public string URL { get; set; } = "https://s3.timeweb.com";
        public string BucketName { get; set; } = "94f597f6-e30486cd-4123-4c3b-850b-9f89dfcfbdf1";
    }
}
