using Microsoft.AspNetCore.Authorization;

namespace ClothingApi.Infrastructure.Keycloak
{
    public class Requirement : IAuthorizationRequirement
    {
        public Requirement(string policyName)
        {
            PolicyName = policyName;
        }

        public string PolicyName { get; }
    }
}