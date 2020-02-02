using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace MangoAccountSystem
{
    public class Config
    {
        private const string RoleType = "role";
        private const string NameType = "name";

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "flash_chat"),
                new ApiResource
                {
                    Name = "mangoblogApi",

                    UserClaims = {RoleType,NameType},

                    Scopes =
                    {
                        new Scope("mangoblogApi")
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "blogjs",
                    ClientName = "mango_blog",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,              

                    //RedirectUris = {"http://localhost:8080/callback.html"},
                    //PostLogoutRedirectUris = {"http://localhost:8080"},
                    //AllowedCorsOrigins = {"http://localhost:8080"},
                    RedirectUris = {configuration["IdentityServerClientUrl:RedirectUris"] },
                    PostLogoutRedirectUris = {configuration["IdentityServerClientUrl:OriginsUrl"] },
                    AllowedCorsOrigins = {configuration["IdentityServerClientUrl:OriginsUrl"]},

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "mangoblogApi",
                        "mango.profile"
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var customProfile = new IdentityResource(
                name: "mango.profile",
                displayName: "mango profile",
                claimTypes: new[] { NameType, RoleType });

            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                customProfile
            };
        }
    }
}
