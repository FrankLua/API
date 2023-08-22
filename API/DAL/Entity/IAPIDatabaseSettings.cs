namespace API.DAL.Entity
{
    public interface IAPIDatabaseSettings
    {
        string NameCollection { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
