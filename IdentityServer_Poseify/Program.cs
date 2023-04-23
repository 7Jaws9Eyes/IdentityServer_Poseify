using IdentityServer_Posefiy;
using Duende.IdentityServer;
using Microsoft.IdentityModel.Tokens;
using IdentityServer_Poseify;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;

var builder = WebApplication.CreateBuilder(args);

var migrationsAssembly = typeof(Program).Assembly.GetName().Name;

var seedDb = bool.Parse(builder.Configuration["SeedDb"]);

builder.Services.AddRazorPages();

builder.Services.AddIdentityServer()
    .AddConfigurationStore(options => {
        options.ConfigureDbContext = b => b.UseSqlite(builder.Configuration["connectionString"],
                sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(options => {
        options.ConfigureDbContext = b => b.UseSqlite(builder.Configuration["connectionString"],
            sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddTestUsers(TestUsers.Users);

builder.Services.AddAuthentication()
    .AddGoogle("Google", options =>
    {
        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseDeveloperExceptionPage();
}


if (seedDb) { InitDb(app); }

app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();


static void InitDb(IApplicationBuilder app) { 
    using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
    {
        serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

        var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        context.Database.Migrate();
        if (!context.Clients.Any())
        {
            foreach (var client in Config.Clients)
            {
                context?.Clients.Add(client.ToEntity());
            }
            context?.SaveChanges();
        }

        if (!context.IdentityResources.Any())
        {
            foreach (var resource in Config.IdentityResources)
            {
                context?.IdentityResources.Add(resource.ToEntity());
            }
            context.SaveChanges();
        }

        if (!context.ApiScopes.Any())
        {
            foreach (var resource in Config.ApiScopes)
            {
                context.ApiScopes.Add(resource.ToEntity());
            }
            context?.SaveChanges();
        }
    }
}
