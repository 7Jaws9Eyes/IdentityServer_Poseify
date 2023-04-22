using IdentityServer_Posefiy;
using Duende.IdentityServer;
using Microsoft.IdentityModel.Tokens;
using IdentityServer_Poseify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddIdentityServer()
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddTestUsers(TestUsers.Users);

builder.Services.AddAuthentication()
    .AddGoogle("Google", options =>
    {
        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    })
    .AddOpenIdConnect("oidc", "Duende Demo Server", options =>
    {
        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        options.SignOutScheme = IdentityServerConstants.SignoutScheme;
        options.SaveTokens = true;

        options.Authority = "https://demo.duendesoftware.com";
        options.ClientId = "interactive.confidential";
        options.ClientSecret = "secret";
        options.ResponseType = "code";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(p => { p.WithOrigins("https://play.google.com", "https://play.google.com"); });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.UseCors();

app.MapRazorPages().RequireAuthorization();

app.Run();
