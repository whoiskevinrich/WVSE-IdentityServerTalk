using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Sample.IdentityServer.Configurations
{
    public class Clients
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // set up a client to be used by non-interactive users
                new Client
                {
                    ClientId = "client",
                    ClientName = "Example Client Application",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secrets for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = {"claims.read", "claims.write"}
                },

                // set up client for use by interactive users
                new Client
                {
                    ClientId = "MVC Client",
                    ClientName = "Example Implicit Client Application",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new []
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                    },

                    // set up the server's contract on what URLs can access the api
                    RedirectUris = new []{ "http://localhost:7001/signin-oidc" },

                    // set up which logout endpoints shall be hit on logout
                    PostLogoutRedirectUris = new []{ "http://localhost:7001" }
                }
            };
        }
    }
}
