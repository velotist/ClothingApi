using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http;

namespace ClothingApi.Infrastructure.Keycloak
{
    public class AuthorizationOptions
    {
        public string RequiredScheme { get; set; } = JwtBearerDefaults.AuthenticationScheme;
        public string TokenEndpoint { get; set; }
        public HttpMessageHandler BackchannelHandler { get; set; } = new HttpClientHandler();
        public string Audience { get; set; }
    }
}