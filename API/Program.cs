using Amazon;
using API.DAL.Entity.APIDatebaseSet;
using API.DAL.Entity.SecrurityClass;
using API.DAL.Entity.SupportClass;
using API.Entity.SecrurityClass;
using API.Services.ForAPI;
using API.Services.ForS3.Configure;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.IO.Compression;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using static API.Controllers.Web.DownloadController;


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
                //ScopeBuilder.InitializerRateLimiter(service);


                service.Configure<APIDatabaseSettings>(
                    builder.Configuration.GetSection(nameof(APIDatabaseSettings)));


                service.Configure<FormOptions>(opt => { opt.MultipartBodyLengthLimit = 600 * 1024 * 1024; });
				

				// ��������� ������� ������

				service.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
                service.AddResponseCompression(options =>
                {
                    options.Providers.Add<GzipCompressionProvider>();
                    options.EnableForHttps = true;
                    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
                });


                // ���������� �����������
                builder.Services.AddMemoryCache();
                

                service.AddScoped<IAPIDatabaseSettings>(sp =>
                    sp.GetRequiredService<IOptions<APIDatabaseSettings>>().Value);
                service.AddScoped<IMongoClient>(sp =>
                    new MongoClient(builder.Configuration.GetValue<string>("APIDatabaseSettings:ConnectionString")));
                service.AddScoped<IAppConfiguration, AppConfiguration>();
                // Add services to the container.
                ScopeBuilder.InitializerServices(service);
                service.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                service.AddEndpointsApiExplorer();
                service.AddSwaggerGen();
                service.AddAuthentication()
                    .AddScheme<AuthenticationSchemeOptions, BasicAunteficationHandler>("Basic", null);
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

                app.UseStaticFiles(); //static files for web

                app.UseHttpsRedirection();
                app.UseAuthorization();

				
				app.MapControllers();
                //app.UseRateLimiter();
                app.UseRouting();
                app.UseAuthentication(); // ��������������
                app.UseAuthorization(); // �����������
                app.Use(async (context, next) =>
                {
                    // �������� �������� �����
                    Endpoint endpoint = context.GetEndpoint();

                    if (endpoint != null)
                    {
                        // �������� ������ ��������, ������� ������������ � �������� ������
                        var routePattern = (endpoint as Microsoft.AspNetCore.Routing.RouteEndpoint)?.RoutePattern
                            ?.RawText;

                        Debug.WriteLine($"Endpoint Name: {endpoint.DisplayName}");
                        Debug.WriteLine($"Route Pattern: {routePattern}");

                        // ���� �������� ����� ����������, �������� ��������� ������
                        await next();
                    }
                    else
                    {
                        Debug.WriteLine("Endpoint: null");
                        // ���� �������� ����� �� ����������, ��������� ���������
                        await context.Response.WriteAsync("You are going too far (Page not found 404)");
                    }
                });
				
				app.UseEndpoints(endpoints =>
                {
                    endpoints.MapGet("/", async context => { context.Response.Redirect("/Web/Log/Login"); });
                });
				

				// app.UseResponseCompression(); // ���������� ������


				app.Run();
            }
            catch (Exception ex)
            {
                Loger.Exception(ex, "Main");
            }
        }
    }
}