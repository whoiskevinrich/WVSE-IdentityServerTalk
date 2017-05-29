using System.Collections.Generic;
using IdentityServer4.Models;

namespace Sample.IdentityServer.Configurations
{
    public class Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            // specify available identity resources
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            // specify available API resources
            return new List<ApiResource>
            {
                new ApiResource("exampleApi", "Example API")
                {
                    Description = "Example API Access",
                    UserClaims = new List<string> {"role"},
                    ApiSecrets = new List<Secret> {new Secret("scopeSecret".Sha256())},
                    Scopes = new List<Scope>
                    {
                        new Scope("claims.read"),
                        new Scope("claims.write")
                    }
                }
            };
        }

    }
}