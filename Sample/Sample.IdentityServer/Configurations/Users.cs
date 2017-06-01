using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Test;

namespace Sample.IdentityServer.Configurations
{
    public class Users
    {
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "B2A443C9-5B35-49E4-9BD5-AB302619ABFC",
                    Username = "Kevin",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Email, "whoiskevinrich@gmail.com"),
                        new Claim(JwtClaimTypes.GivenName, "Kevin"),
                        new Claim(JwtClaimTypes.FamilyName, "Rich"),
                        new Claim(JwtClaimTypes.Role, "admin")
                    }
                }
            };
        }
    }
}