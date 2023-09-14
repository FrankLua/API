namespace API.Services.ForS3.Configure
{
    public interface IAppConfiguration
    {
		public string AwsAccessKey { get; set; }
		public string AwsSecretAccessKey { get; set; }
		public string BucketName { get; set; }
        public string URL { get; set; }

    }
}
