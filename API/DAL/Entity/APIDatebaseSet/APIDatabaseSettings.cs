namespace API.DAL.Entity.APIDatebaseSet
{
    public class APIDatabaseSettings : IAPIDatabaseSettings
    {
        public string NameCollection { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}
