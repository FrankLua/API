using Amazon.S3;
using API.DAL.Entity;
using API.DAL.Entity.APIResponce;
using API.DAL.Entity.SecrurityClass;
using API.Entity.SecrurityClass;
using API.Services.ForAPI;
using API.Services.ForS3.Configure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

             var service = builder.Services;


            Loger.CreateFirstTxt();
            Loger.WriterTxtfile("I russian");
            service.Configure<APIDatabaseSettings>(
            builder.Configuration.GetSection(nameof(APIDatabaseSettings)));
            service.AddSingleton<IAPIDatabaseSettings>(sp => 
            sp.GetRequiredService<IOptions<APIDatabaseSettings>>().Value);
            service.AddSingleton<IMongoClient>(sp =>
            new MongoClient(builder.Configuration.GetValue<string>("APIDatabaseSettings:ConnectionString")));
            service.AddScoped<IDeviceService, DeviceService>();
            service.AddScoped<IUserService, UserService>();
            service.AddScoped<IAppConfiguration, AppConfiguration>();
            service.AddSingleton<IAppConfiguration, AppConfiguration>();
            // Add services to the container.

            service.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();
            service.AddAuthentication()
.AddScheme<AuthenticationSchemeOptions, BasicAunteficationHandler>(BasicAuthenticationDefaults.AuthenticationScheme, null);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
 


            app.MapControllers();

            app.Run();
        }
    }
}