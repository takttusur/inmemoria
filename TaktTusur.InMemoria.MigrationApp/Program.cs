using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using TaktTusur.InMemoria.MigrationApp.Mappings;
using TaktTusur.InMemoria.MigrationApp.Services;

namespace TaktTusur.InMemoria.MigrationApp;

class Program
{
	public const string OldDbConnectionStringName = "MySqlOldDb";

	static void Main(string[] args)
	{
		var configurationBuilder = new ConfigurationBuilder();
		configurationBuilder.AddJsonFile("appsettings.json");
		var configuration = configurationBuilder.Build();

		var serviceCollection = new ServiceCollection();
		serviceCollection.AddTransient(x =>
			new MySqlConnection(configuration.GetConnectionString(OldDbConnectionStringName)));
		serviceCollection.AddSingleton<IConfiguration>(configuration);
		serviceCollection.AddTransient<IMySqlDumpLoader, MySqlDumpLoader>();
		serviceCollection.AddTransient<IDbConnectionTester, DbConnectionTester>();
		serviceCollection.AddTransient<ISettingsExporter, SettingsExporter>();
		serviceCollection.AddTransient<IPersonExporter, PersonsExporter>();
		serviceCollection.AddTransient<IApplicationService, ApplicationService>();
		serviceCollection.AddAutoMapper(config =>
		{
			config.AddProfile<PersonProfile>();
		});
		serviceCollection.AddLogging();
		serviceCollection.Configure<LoggerFilterOptions>(options =>
			options.MinLevel = LogLevel.Information);

		serviceCollection.AddLogging(builder =>
		{
			builder.AddConfiguration(configuration.GetSection("Logging"));
			builder.AddConsole();
		});

		DapperMappings.ApplyMappings();

		var serviceProvider = serviceCollection.BuildServiceProvider();

		// Check DB is ready to accept connections
		var dbTester = serviceProvider.GetRequiredService<IDbConnectionTester>();
		dbTester.Test(5);

		var app = serviceProvider.GetRequiredService<IApplicationService>();
		app.DoExport();
	}
}
