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

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = {"exampleApi.read", "exampleApi.write"}
                },

                // set up client for use by interactive users
                new Client
                {
                    ClientId = "oidcClient",
                    ClientName = "Example Implicity Client Application",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new []
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "role",
                        "exampleApi.write"
                    },

                    // set up the server's contract on what URLs can access the api
                    RedirectUris = new []{ "http://localhost:58196/signin-oidc" },

                    // set up which logout endpoints shall be hit on logout
                    PostLogoutRedirectUris = new []{ "http://localhost:58196" }
                }
            };
        }
    }
}
