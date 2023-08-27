namespace API.Services.ForS3.Configure
{
    public interface IAppConfiguration
    {
        string AwsAccessKey { get; set; }
        string AwsSecretAccessKey { get; set; }

        string BucketName { get; set; }
        public string URL { get; set; }

    }
}
