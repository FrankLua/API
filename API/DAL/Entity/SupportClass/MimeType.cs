namespace API.DAL.Entity.SupportClass
{
    public class MimeType
    {
        private static readonly List<string> _mimetype = new List<string>() 
        {
          "video/mp4","audio/mp4"


        };
        public static string GetFolderName(string MimeType, bool Ad = false)
        {
            //return folder name for media file
            if (Ad)
            {
				switch (MimeType.Split('/')[0])
				{
					case "video": return "Ad/video";
					case "audio": return "Ad/audio";
					case "image": return "Ad/image";
				}
				return null;
			}
            else
            {
				switch (MimeType.Split('/')[0])
				{
					case "video": return "video";
					case "audio": return "audio";
					case "image": return "image";
				}
				return null;
			}

        }
        public static bool CheakMimetype(string MimeType)
        {
            //return boolean if list "_mimetype" have type return true, otherwise false
            return _mimetype.Contains(MimeType);
        }
    }
}
