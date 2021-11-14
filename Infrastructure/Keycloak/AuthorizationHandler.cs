using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ClothingApi.Infrastructure.Keycloak
{
    public class AuthorizationHandler : AuthorizationHandler<Requirement>
    {
        private readonly IOptions<AuthorizationOptions> _options;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationHandler(IOptions<AuthorizationOptions> options, IHttpContextAccessor httpContextAccessor)
        {
            _options = options;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, Requirement requirement)
        {
            var options = _options.Value;
            var httpContext = _httpContextAccessor.HttpContext;
            var auth = await httpContext.AuthenticateAsync(options.RequiredScheme);
            if (!auth.Succeeded)
            {
                context.Fail();

                return;
            }

            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "grant_type", "urn:ietf:params:oauth:grant-type:uma-ticket" },
                { "response_mode", "decision" },
                { "audience", options.Audience },
                { "permission", $"{requirement.PolicyName}" }
            };

            var client = new HttpClient(options.BackchannelHandler);
            var token = await httpContext.GetTokenAsync(options.RequiredScheme, "access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync(options.TokenEndpoint, new FormUrlEncodedContent(data));
            if (response.IsSuccessStatusCode)
            {
                context.Succeed(requirement);

                return;
            }

            context.Fail();
        }
    }
}