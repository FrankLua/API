using API.Services.ForAPI.Int;
using API.Services.ForAPI.Rep;
using API.Services.ForS3.Configure;

namespace API
{
    public static class ScopeBuilder 
    {
        public static void InitializerServices(this IServiceCollection service)
        {
			service.AddScoped<IAd_files_Service, Adfile_Rep>();
			service.AddScoped<IAd_Playlist_Service, Ad_Playlist_Rep>();
			service.AddScoped<IDevice_Service, Device_Rep>();
            service.AddScoped<IUser_Service, User_Rep>();
            service.AddScoped<IAppConfiguration, AppConfiguration>();
            service.AddScoped<IMedia_Playlist_Service, Media_Playlist_Rep>();
            service.AddScoped<IMedia_File_Service, Media_File_Rep>();
        }
    }
    
}
