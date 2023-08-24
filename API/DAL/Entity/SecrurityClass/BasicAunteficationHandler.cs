using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using API.Services;
using API.DAL.Entity.Models;
using API.Entity.SecrurityClass;
using API.DAL.Entity.APIResponce;

namespace API.DAL.Entity.SecrurityClass
{
    public class BasicAunteficationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;
        public BasicAunteficationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUserService user) : base(options, logger, encoder, clock)
        {
            _userService = user;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Console.WriteLine("Handler is Called");
            
            // No authorization header, so throw no result.
            if (!Request.Headers.ContainsKey("Authorization"))
            {

                return await Task.FromResult(AuthenticateResult.Fail("Missing Authorization header"));
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var contenttype = Request.Headers["Content-Type"].ToString();
            var bodyStr = "";
            var req = Request;

            using (StreamReader reader
                      = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStr = await reader.ReadToEndAsync();
            }

            // If authorization header doesn't start with basic, throw no result.
            if (!authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return await Task.FromResult(AuthenticateResult.Fail("Authorization header does not start with 'Basic'"));
            }

            // Decrypt the authorization header and split out the client id/secret which is separated by the first ':'
            
            var authBase64Decoded = Encoding.UTF8.GetString(Convert.FromBase64String(authorizationHeader.Replace("Basic ", "", StringComparison.OrdinalIgnoreCase)));
            var authSplit = authBase64Decoded.Split(new[] { ':' }, 2);
            
            // No username and password, so throw no result.
            if (authSplit.Length != 2)
            {
                return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization header format"));
            }
            
            // Store the client ID and secret
            var clientId = authSplit[0];
            var clientSecret = authSplit[1];

            // Client ID and secret are incorrect
            User user = _userService.CheakUser(clientId, clientSecret).data;
            if(bodyStr != null && contenttype !=null )
            {
                WriterLogHandler(authBase64Decoded, bodyStr, contenttype);
            }
            else
            {
                WriterLogHandler(authBase64Decoded);
            }
            
            if (user == null)
            {
                return await Task.FromResult(AuthenticateResult.Fail(string.Format("The secret is incorrect for the client '{0}'", clientId)));
            }

            // Authenicate the client using basic authentication
            var client = new BasicAuthenticationClient
            {
                AuthenticationType = BasicAuthenticationDefaults.AuthenticationScheme,
                IsAuthenticated = true,
                Name = clientId
            };

            // Set the client ID as the name claim type.
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(client, new[]
            {
                new Claim(ClaimTypes.Name, clientId)
            }));

            // Return a success result.
            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }
        public static void WriterLogHandler(string header = "I not have head x(", string body = "I not have body ;(",string contentType = "I not have contenttype")
        {
            Console.WriteLine($"Header : {header},\n ContentType: {contentType}\n Body: {body}");
        }
    }
}
