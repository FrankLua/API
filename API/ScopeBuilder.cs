using API.Services.ForAPI.Int;
using API.Services.ForAPI.Rep;
using API.Services.ForDB.Int;
using API.Services.ForS3.Configure;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

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
        public static void InitializerRateLimiter(this IServiceCollection service)
        {
            service.AddRateLimiter(opt => opt.AddConcurrencyLimiter("ForFile", parametrs =>
            {
                parametrs.PermitLimit = 5;
                parametrs.QueueLimit = 5;
                parametrs.QueueProcessingOrder = QueueProcessingOrder.NewestFirst;
            }).RejectionStatusCode = 423);
            service.AddRateLimiter(opt => opt.AddConcurrencyLimiter("ForOther", parametrs =>
            {
                parametrs.PermitLimit = 10;
                parametrs.QueueLimit = 25;
                parametrs.QueueProcessingOrder = QueueProcessingOrder.NewestFirst;
            }).RejectionStatusCode = 423);

        }
    }
    
}
