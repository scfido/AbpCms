using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cms.Web.Host.IdentityServer
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
        {
            new ApiResource("default-api", "Default (all) API")
        };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResources.Phone()
        };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "console",
                    AllowedGrantTypes = GrantTypes.ClientCredentials.Union(GrantTypes.ResourceOwnerPassword).ToList(),
                    AllowedScopes = {"default-api"},
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                },
                new Client
                {
                    ClientId = "mvc",
                    AllowedGrantTypes = GrantTypes.ClientCredentials.Union(GrantTypes.Hybrid).ToList(),
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "default-api"
                    },
                    
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RedirectUris={"http://localhost:38372/signin-oidc","http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris={"http://localhost:38372" },
                    FrontChannelLogoutUri="http://localhost:38372/signout-oidc" ,
                    AllowOfflineAccess = true
                }

            };
        }
    }
}
