using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServerPoseify;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };


    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
                new ApiScope("poseifyApiScope", "Poseify API Scope")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "PoseifyBff",
                ClientId = "PoseifyBff", // SPA Web Client

                AllowedGrantTypes = GrantTypes.Code,
                    
                // where to redirect to after login
                RedirectUris = { "https://localhost:44462/signin-oidc", "https://poseify.ngrok.app/signin-oidc" },

                // where to redirect to after logout
                PostLogoutRedirectUris = { "https://localhost:44462/signout-callback-oidc", "https://poseify.ngrok.app/signout-callback-oidc" },

                //AllowOfflineAccess = true,
                AlwaysIncludeUserClaimsInIdToken = true,

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "poseifyApiScope"
                }
            
            },
            new Client
            {
                ClientId = "PoseifyNative", // Mobile Client
                
                AllowedGrantTypes = GrantTypes.Code,
                AllowOfflineAccess = true,
                RequirePkce = true,  // Enforce PKCE
                RequireClientSecret = false,
                RefreshTokenUsage = TokenUsage.ReUse,
                RequireConsent = false,
                AlwaysIncludeUserClaimsInIdToken = true,    

                // where to redirect to after login
                RedirectUris = { "exp://192.168.0.17:8081" },
                // where to redirect to after logout
                PostLogoutRedirectUris = { "exp://192.168.0.17:8081" },

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "poseifyApiScope"
                }
            }
        };
}