using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using MySqlConnector.Logging;
using TaktTusur.InMemoria.MigrationApp.Configuration;
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
		serviceCollection.AddLogging();
		serviceCollection.Configure<LoggerFilterOptions>(options =>
			options.MinLevel = LogLevel.Information);

		serviceCollection.AddLogging(builder =>
		{
			builder.AddConfiguration(configuration.GetSection("Logging"));
			builder.AddConsole();
		});

		var serviceProvider = serviceCollection.BuildServiceProvider();

		// Check DB is ready to accept connections
		var dbTester = serviceProvider.GetRequiredService<IDbConnectionTester>();
		dbTester.Test(5);

		// Restore DB using sql file
		var mySqlDumpLoader = serviceProvider.GetRequiredService<IMySqlDumpLoader>();
		var backupSql = File.ReadAllText("./Resources/inmemoria.sql");
		mySqlDumpLoader.Load(backupSql);

		// Read export settings
		var exportConfiguration = configuration.GetSection(ExportConfiguration.SECTION_NAME)
				.Get<ExportConfiguration>();

		// Saving Settings to file
		var settingsExporter = serviceProvider.GetRequiredService<ISettingsExporter>();
		var exported = settingsExporter.ExportTo(exportConfiguration.SettingsFile);
		Console.WriteLine("Settings Exported:" + exported);

		// Saving Persons
		var personExporter = serviceProvider.GetRequiredService<IPersonExporter>();
		var ids = personExporter.GetEntityIdsForExport();
		int personCounter = 0;
		foreach (var id in ids)
		{
			const string ext = ".json";
			var file = Path.Combine(exportConfiguration.PersonsDirectory, id + ext);
			if (File.Exists(file)) continue;
			personExporter.Export(id, file);
			personCounter++;
		}
		Console.WriteLine("Person Exporeted:" + personCounter);
	}
}
