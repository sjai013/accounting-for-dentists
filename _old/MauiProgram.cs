using AccountingForDentists.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AccountingForDentists;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
		const string connectionString = "AccountEndpoint=https://accounting-for-dentists.documents.azure.com:443/;AccountKey=26jyjpay18Exhhn4S71Jy1QStTnf2bAyGXP6PZOMmFHEK7xnA9ixrBs3gWbOGZ1DIemGIqWRUMWwACDblS3UQg==";
		builder.Services.AddDbContextFactory<AccountingContext>(optionsBuilder =>
		  optionsBuilder
			.UseCosmos(
			  connectionString: connectionString,
			  databaseName: "ApplicationDB",
			  cosmosOptionsAction: options =>
			  {
				  options.ConnectionMode(Microsoft.Azure.Cosmos.ConnectionMode.Direct);
				  options.MaxRequestsPerTcpConnection(16);
				  options.MaxTcpConnectionsPerEndpoint(32);
			  }));

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		var app = builder.Build();

#if DEBUG
		// Ensure the database is created
		// This is typically done in a migration step, but for simplicity, we ensure it here.
		// In production, you would want to handle migrations properly.	
		app.Services.GetRequiredService<IDbContextFactory<AccountingContext>>()
			.CreateDbContext()
			.Database.EnsureCreated();
#endif
		return app;
	}
}
