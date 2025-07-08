using AccountingForDentists.Components;
using AccountingForDentists.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AccountingContext>(options =>
    options.UseCosmos(builder.Configuration.GetConnectionString("AccountingForDentists")!, "accounting-for-dentists",
        cosmos => cosmos
            .ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Direct)
            .MaxRequestsPerTcpConnection(16)
            .MaxTcpConnectionsPerEndpoint(32)
    ));


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
