using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using System.Collections.Generic;

namespace IdentityServer_Posefiy
{
    public static class Config
    {

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                        new IdentityResources.OpenId(),
                        new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("poseify", "Poseify API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // machine to machine client
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    // scopes that client has access to
                    AllowedScopes = { "poseify" }
                },
                
                new Client
                {
                    ClientId = "PoseifyBff",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    
                    // where to redirect to after login
                    RedirectUris = { "https://localhost:7184/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:7184/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };
    }
}