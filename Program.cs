using AccountingForDentists;
using AccountingForDentists.Components;
using AccountingForDentists.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AccountingContext>(options =>
    options.UseCosmos(builder.Configuration.GetConnectionString("AccountingForDentists")!, "main",
        cosmos => cosmos
            .ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Direct)
            .MaxRequestsPerTcpConnection(16)
            .MaxTcpConnectionsPerEndpoint(32)
    ));

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


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

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
