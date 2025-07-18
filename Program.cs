using System.Reflection;
using AccountingForDentists;
using AccountingForDentists.Components;
using AccountingForDentists.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    // Add a route prefix so all Razor Pages are under /portal
    options.Conventions.AddFolderRouteModelConvention("/", model =>
    {
        foreach (var selector in model.Selectors)
        {
            selector.AttributeRouteModel.Template =
                "portal/" + selector.AttributeRouteModel.Template;
        }
    });
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<AccountingContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AccountingForDentists")!), ServiceLifetime.Scoped);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddScoped<TenantProvider>();

Console.WriteLine($"Using secret {builder.Configuration["Authentication:Microsoft:ClientId"]}");

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, oidcOptions =>
{
    oidcOptions.Scope.Add("email");
    //Configure series of OIDC options like flow, authority, etc 
    oidcOptions.Authority = "https://login.microsoftonline.com/common/v2.0/";

    // These are stored as user secrets (dotnet user secrets)
    oidcOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"] ?? "";
    oidcOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"] ?? "";
    oidcOptions.ResponseType = OpenIdConnectResponseType.Code;
    oidcOptions.MapInboundClaims = false;
    oidcOptions.TokenValidationParameters.NameClaimType = "name";
    oidcOptions.TokenValidationParameters.RoleClaimType = "roles";
    oidcOptions.TokenValidationParameters.ValidateIssuer = false;
});
builder.Services.AddAuthorization();
// IdentityModelEventSource.ShowPII = true;
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapRazorPages()
.WithStaticAssets();

app.MapGet("/account/logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, new()
    {
        RedirectUri = "/"
    });

});

app.MapGet("/account/login", [Authorize] async (AccountingContext context) =>
{
    await context.Database.EnsureCreatedAsync();
    return Results.Redirect("/");
});

app.Run();
