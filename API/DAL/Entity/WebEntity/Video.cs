namespace API.DAL.Entity.WebEntity
{
    public class Video
    {        

        public string MimeType { get; set; }

        public string Name { get; set; }

        public IFormFile file{ get; set; }
    }
}
