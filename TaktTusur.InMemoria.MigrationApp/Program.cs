using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

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
	}
}
