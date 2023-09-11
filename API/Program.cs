using Amazon;
using API.DAL.Entity.APIDatebaseSet;
using API.DAL.Entity.SecrurityClass;
using API.DAL.Entity.SupportClass;
using API.Entity.SecrurityClass;
using API.Services.ForAPI;
using API.Services.ForS3.Configure;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;


using MongoDB.Driver;
using System.IO.Compression;

namespace API
{
    public class Program
    {
       

        public static void Main(string[] args)
        {
            try
            {
                Loger.CreateFirstTxt();

                var builder = WebApplication.CreateBuilder(args);

            var service = builder.Services;           
            
            service.Configure<APIDatabaseSettings>(
            builder.Configuration.GetSection(nameof(APIDatabaseSettings)));


            // добавляем сервисы сжатия
            service.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            service.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
            });

            

            service.AddSingleton<IAPIDatabaseSettings>(sp => 
            sp.GetRequiredService<IOptions<APIDatabaseSettings>>().Value);
            service.AddSingleton<IMongoClient>(sp =>
            new MongoClient(builder.Configuration.GetValue<string>("APIDatabaseSettings:ConnectionString")));
            service.AddSingleton<IAppConfiguration, AppConfiguration>();
            // Add services to the container.
            ScopeBuilder.InitializerServices(service);
            service.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();
            service.AddAuthentication()
.AddScheme<AuthenticationSchemeOptions, BasicAunteficationHandler>(BasicAuthenticationDefaults.AuthenticationScheme, null);
            service.AddControllersWithViews();
            service.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => //CookieAuthenticationOptions
        {
            options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Web/Log/Login");
        });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            
            app.UseResponseCompression(); // подключаем сжатие

            
            app.UseStaticFiles();        //static files for web
            app.UseAuthentication();    // аутентификация
            app.UseAuthorization();     // авторизация
            app.UseHttpsRedirection();
            app.UseAuthorization();


           
            app.MapControllers();
            
            app.Run();
            
            }
            catch(Exception ex)
            {
                Loger.Exaption(ex,"Main");
            }
        }
      
    }
}